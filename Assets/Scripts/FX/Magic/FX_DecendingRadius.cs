using UnityEngine;

public class FX_DecendingRadius : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve radialDecent;
    [SerializeField] private Gradient fadeOverTime;
    [SerializeField] private AnimationCurve movement;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int numberOfStars;
    [SerializeField] private float timeScale;
    private class TrailedStar
    {
        public GameObject go;
        public SpriteRenderer sprite;
        public TrailRenderer trail;
        public float rotationOffset;
        public TrailedStar(GameObject go, SpriteRenderer sprite, TrailRenderer trail, float rotationOffset)
        {
            this.go = go;
            this.sprite = sprite;
            this.trail = trail;
            this.rotationOffset = rotationOffset;
        }

        public void SetColor(Color color)
        {
            sprite.color = color;
            trail.startColor = color;
            trail.endColor = color;
        }
        public void Enable()
        {
            trail.Clear();
            go.SetActive(true);
        }
        public void Disable()
        {
            go.SetActive(false);
        }
    }
    private TrailedStar[] stars;
    private float time;
    private float constantPIAdd;



    // Start is called before the first frame update
    void Awake()
    {
        CreateStars();
    }
    private void CreateStars()
    {
        numberOfStars = transform.childCount;
        constantPIAdd = Mathf.PI * 2 / numberOfStars;

        stars = new TrailedStar[numberOfStars];
        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject newStar = transform.GetChild(i).gameObject;
            TrailRenderer trail = newStar.GetComponent<TrailRenderer>();
            SpriteRenderer sprite = newStar.GetComponent<SpriteRenderer>();
            float rotationOffset = constantPIAdd * i;
            stars[i] = new TrailedStar(newStar, sprite, trail, rotationOffset);
            stars[i].Disable();
        }
    }


    private void OnEnable()
    {
        time = 0;
        Animate(0);
        foreach (TrailedStar star in stars)
            star.Enable();
    }

    private void OnDisable()
    {
        foreach (TrailedStar star in stars)
            star.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (time >= 1)
        {
            gameObject.SetActive(false);
            return;
        }
        Animate(Time.deltaTime);
    }

    public void Animate(float delta)
    {
        time += delta * timeScale;

        float radius = radialDecent.Evaluate(time);
        float move = movement.Evaluate(time);
        Color color = fadeOverTime.Evaluate(time);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].go.transform.position = transform.position + Movement(Vector2.right, move) + Rotation(i, radius);
            stars[i].SetColor(color);
        }
    }

    private Vector3 Movement(Vector2 dir, float move)
    {
        return dir * move;
    }
    private Vector3 Rotation(int i, float radius)
    {
        float rotation = constantPIAdd * i + time * rotationSpeed;
        float x = Mathf.Sin(rotation) * radius;
        float y = Mathf.Cos(rotation) * radius;
        return (Vector3.right * x + Vector3.up * y);
    }

}
