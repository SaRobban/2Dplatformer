using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurfaceOld : MonoBehaviour
{
    public int pixelsPerUnit = 32;

    [SerializeField] private GameObject preFab;
    public float surfaceTension = 0.025f;
    public float drag = 0.1f;

    public float maxImpact = 1;

    float spread;

    class Spring
    {
        public Vector2 orgPos;
        public float restOffset = 0f;
        public float height;
        public float velocity;
        public Vector2 pos;
        public GameObject preFab;

        public Spring(Vector2 orgPos, Vector2 pos, float velocity, GameObject preFab)
        {
            this.orgPos = orgPos;
            this.pos = pos;
            this.velocity = velocity;
            this.preFab = preFab;
            this.height = 0;
        }

        public void UpdateSpring(float tension, float drag, float delta)
        {
            height = pos.y - orgPos.y;
            height -= restOffset;
            float acceleration = tension * height;
            velocity -= acceleration;
            velocity *= 1 - drag * delta;

            
            pos.y += velocity * delta;
            preFab.transform.position = pos;
          
            Vector3 scale = preFab.transform.localScale;
            scale.y += height*2;
            preFab.transform.localScale = scale;
        }

       

        public void ChangeColor(Color color)
        {
            preFab.GetComponent<Renderer>().material.color = color;
        }
    }
    Spring[] springs;

    public GameObject[] gbjects;

    // Start is called before the first frame update
    void Start()
    {
        SetUpBySize stats = new SetUpBySize(this.gameObject, pixelsPerUnit, true);


        springs = new Spring[stats.segments];
        for (int i = 0; i < springs.Length; i++)
        {
            Vector2 pos =
                stats.startPos +
                Vector2.right * i * stats.step +
                Vector2.right * +stats.step * 0.5f
                ;

            GameObject inst = Instantiate(preFab, pos, Quaternion.identity);
            inst.transform.localScale = new Vector3(stats.step, stats.heigth, 1);

            springs[i] = new Spring(pos, pos, 0, inst);
        }
    }

    class SetUpBySize
    {
        public Vector2 startPos;
        public float lenght;
        public float heigth;
        public float step;
        public int segments;
        public SetUpBySize(GameObject go, int pxU, bool destroySource)
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            startPos = sr.transform.position + Vector3.left * sr.bounds.extents.x;
            lenght = sr.bounds.extents.x * 2;
            heigth = sr.bounds.extents.y * 2;
            step = 1f / (float)pxU;

            float segsFloat = lenght / step;
            if (segsFloat - Mathf.Floor(segsFloat) > step)
                segments = (int)(lenght / step) + 1;
            else
                segments = (int)(lenght / step);

            if (destroySource)
            {
                Destroy(sr);
            }
        }
    }

    bool test = false;
    // Update is called once per frame
    void Update()
    {

        //TestRest();
        //DrawWave();
        MovePointArr();
        SpreedTree();
        if (test)
            Time.timeScale = 0.1f;

        if (!GetMouseClick(out Vector2 pos))
            return;

        SetImpact(pos);
        // test = true;
    }

    void DrawWave()
    {
        int x = 0;
        int y = 1;
        for (int z = 2; z < springs.Length; z += 2)
        {
            Vector2 dirOne = springs[y].pos - springs[x].pos;
            Vector2 dirTwo = springs[z].pos - springs[y].pos;

            Vector2 p0 = springs[x].pos;
            Vector2 p1 = springs[x].pos + dirTwo * 0.5f;

            Vector2 p2 = springs[y].pos;
            Vector2 p3 = springs[y].pos + dirOne * 0.5f;
            Vector2 p4 = springs[z].pos;

            Debug.DrawLine(p0, p1);
            Debug.DrawLine(p1, p2);
            Debug.DrawLine(p2, p3);
            Debug.DrawLine(p3, p4);
            x = z;
            y = z + 1;
        }
    }

    void MovePointArr()
    {
        for (int i = 0; i < springs.Length; i++)
        {
            springs[i].UpdateSpring(surfaceTension, drag, Time.deltaTime);
        }
    }

    void Spread()
    {
        float[] leftH = new float[springs.Length];
        float[] rightH = new float[springs.Length];
        for (int itt = 0; itt < 18; itt++)
        {
            for (int i = 0; i < springs.Length; i++)
            {
                leftH[i] = 0;
                rightH[i] = 0;

                if (i > 0)
                {
                    leftH[i] = spread * (springs[i - 1].height - springs[i].height);
                    springs[i].velocity += leftH[i];
                }
                if (i < springs.Length - 1)
                {
                    rightH[i] = spread * (springs[i + 1].height - springs[i].height);
                    springs[i].velocity += rightH[i];
                }
            }

            for (int i = 0; i < springs.Length; i++)
            {
                if (i > 0)
                {
                    springs[i].height += leftH[i] * leftH[i];
                }
                if (i < springs.Length - 1)
                {
                    springs[i].height += rightH[i] * leftH[i];
                }
            }
        }
    }
    void SpreedTree()
    {
        float volume = 0;
        float volumeLoss = 0.1f;
        float volumeRise = 0;
        for(int x = 1; x < springs.Length; x++)
        {
            volume += springs[x-1].height;
            springs[x].restOffset = volume * volumeLoss;
            volume -= volume * volumeLoss;
        }

        volumeRise += volume;

        volume = 0;
        for (int x = springs.Length-2; x >-1; x--)
        {
            volume += springs[x + 1].height;
            springs[x].restOffset = (springs[x].restOffset + volume * volumeLoss)*0.5f;
            volume -= volume * volumeLoss;
        }

        volumeRise += volume;
        volumeRise /= springs.Length;
        for(int i = 0; i < springs.Length; i++)
        {
          //  springs[i].restOffset += volumeRise;
        }

        if (volume != 0)
        {
            Debug.Log(volume);
            return;
        }
    }
    void SpreedTwo()
    {
        float volume = 0;
        float[] heightL = new float[springs.Length];
        float[] heigthR = new float[springs.Length];

        for (int x = 0; x < springs.Length; x++)
        {
            volume += springs[x].height;
            if (x > 0)
                heightL[x] = springs[x - 1].height;
            else
                heightL[x] = 0;

            if (x < springs.Length - 1)
                heigthR[x] = springs[x + 1].height;
            else
                heigthR[x] = 0;
        }

        volume /= springs.Length;
        for (int y = 0; y < springs.Length; y++)
        {
            springs[y].restOffset = (heigthR[y] + heightL[y]) * 0.5f * drag;
            springs[y].restOffset -= volume;
        }

        /*
        springs[lowest].ChangeColor(Color.red);

        int r = lowest;
        int l = lowest;

        int rDir = 1;
        int lDir = -1;

        while(volume > 0.01f)
        {
            if (r == 0 || r == springs.Length - 1)
                rDir *= -1;

            if (l == 0 || l == springs.Length - 1)
                rDir *= -1;
            r += rDir;
            l += lDir;

            springs[r].pos.y += volume * 0.25f * spread;
            springs[l].pos.y += volume * 0.25f * spread;
            volume -= volume * 0.5f * spread;
        }
        */

    }

    void TestRest()
    {
        foreach (Spring s in springs)
        {
            s.restOffset -= Time.deltaTime * 0.1f;
        }
    }
    bool GetMouseClick(out Vector2 pos)
    {
        pos = Vector2.zero;
        if (Input.GetButtonDown("Fire1"))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return true;
        }
        return false;
    }

    void SetImpact(Vector2 point)
    {
       // int closest = 0;
       // float distComp = 100000;
        for (int i = 0; i < springs.Length; i++)
        {
            float dist = (springs[i].pos - point).sqrMagnitude;

            if (dist <1)
            {
                springs[i].velocity = -maxImpact * (1 - dist);
            }
            /*
            if (dist < distComp)
            {
                closest = i;
                distComp = dist;
            }
            */
        }
        //springs[closest].velocity = -maxImpact;
    }
}
