using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightnigMoveIng : MonoBehaviour
{
    [SerializeField] Transform target;
     
    [SerializeField] GameObject linePreFab;
    [SerializeField] GameObject sparkPreFab;
    [SerializeField] float flashRange;
    [SerializeField] float flashFadeTime = 1f;
    Flash[] flash;

    float deltaStep = 0.1f;
    float maxDeltaStep = 2;
    int flashArrLength = 40;
    Vector3 lastPos;


    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        maxDeltaStep *= maxDeltaStep;

        Transform lightningHolder = new GameObject().transform;
        lightningHolder.name = "FX_LightningHolder";


        flash = new Flash[flashArrLength];

        for (int i = 0; i < flash.Length; i++)
        {
            GameObject line = Instantiate(linePreFab, lightningHolder);
            GameObject spark = Instantiate(sparkPreFab, lightningHolder);
            spark.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            flash[i] = new Flash(-10, transform.position, transform.position, line.transform, spark.transform);
            flash[i].DontRender();
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        UpdateFlash();

        if (!target.gameObject.activeSelf)
            return;

        float dist = (lastPos - transform.position).sqrMagnitude;
        if (dist > deltaStep * deltaStep)
        {
            SetAtIntervall();
            lastPos = transform.position;
        }

    }


    void SetAtIntervall()
    {
        Flash modF = flash[flash.Length - 1];
        for (int i = flash.Length - 1; i > 0; i--)
        {
            flash[i] = flash[i - 1];
        }
        flash[0] = modF;
        flash[0].SetNewPos(transform.position, flashRange, flashFadeTime, flash[1].flashPos);
    }
#if UNITY_EDITOR
    private void OnGUI()
    {
        //GUI.TextField(new Rect(new Vector2(100, 200), Vector2.one * 200), "sleep" + sleep.ToString());
    }
#endif

    int b = 0;
    bool sleep = false;
    void UpdateFlash()
    {
        if (flash[0].time < 0)
        {
            sleep = true;
            return;
        }
        else
        {
            sleep = false;
        }


        for (int i = 0; i < flash.Length-1; i++)
        {
            b += flash[i].HasTimeLeft();
            flash[i].DrawFlash(AboveMaxDeltaStep(i,i+1));
        }
        flash[flash.Length - 1].DontRender();
    }
    bool AboveMaxDeltaStep(int x, int y)
    {
        if ((flash[x].realPos - flash[y].realPos).sqrMagnitude > maxDeltaStep)
        {
            return true;
        }
        return false;
    }

    class Flash
    {
        public float time;
        public Vector3 realPos;
        public Vector3 flashPos;
        public Transform line;
        public Transform spark;

        private SpriteRenderer spLine;
        private SpriteRenderer spSpark;

        float colorMul;
        public Flash(float time, Vector3 realPos, Vector3 flashPos, Transform line, Transform spark)
        {
            this.time = time;
            this.realPos = realPos;
            this.flashPos = flashPos;
            this.line = line;
            this.spark = spark;

            spLine = this.line.GetComponent<SpriteRenderer>();
            spSpark = this.spark.GetComponent<SpriteRenderer>();

        }

        public void SetNewPos(Vector3 pos, float flashRange, float time, Vector3 nextFlashPos)
        {
            realPos = pos;
            flashPos = pos + Random.insideUnitSphere * flashRange;
            this.time = time;

            colorMul = 1 / time;
            spark.transform.position = flashPos;

            Vector3 dir = nextFlashPos - flashPos;
            dir.z = 0;
            float dist = dir.magnitude;

            line.position = flashPos + dir * 0.5f;
            line.up = dir;

            Vector3 scale = line.localScale;
            scale.y = dist;
            line.localScale = scale;
        }

        public int HasTimeLeft()
        {

            time -= Time.deltaTime;
            if (time > 0)
            {
                Render();
                return 0;
            }
            DontRender();
            return 1;
        }
        public void Render()
        {
            spark.gameObject.SetActive(true);
            line.gameObject.SetActive(true);
        }

        public void DrawFlash(bool skip)
        {
            if (time < 0 || skip)
                DontRender();
            else
                Render();

            spLine.color = Color.white * time * colorMul;
            spSpark.color = Color.white * time * colorMul;
        }

        public void DontRender()
        {
            spark.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
        }
    }
}
