/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_MagicManager : MonoBehaviour
{
    //classes
    private class TrailedStar
    {
        public Transform transform;
        public SpriteRenderer sprite;
        public TrailRenderer trail;
        public I_MagicAnimation owner;
        public TrailedStar(Transform transform, SpriteRenderer sprite, TrailRenderer trail)
        {
            this.transform = transform;
            this.sprite = sprite;
            this.trail = trail;
            this.owner = null;
        }
        public void StopIfOccupied()
        {
            if (owner != null)
                owner.ForceStop();
        }
        public void SetColor(Color color)
        {
            sprite.color = color;
            trail.startColor = color;
            trail.endColor = color;
        }
        public void Enable(I_MagicAnimation magic)
        {
            this.owner = magic;
            trail.Clear();
            trail.emitting = true;
            transform.gameObject.SetActive(true);
        }
        public void Disable()
        {
            trail.emitting = false;
            trail.Clear();
            transform.gameObject.SetActive(false);
        }
    }
    private interface I_MagicAnimation
    {
        public void ForceStop();
        public void Update(float delta);
        public void Stop();

    }
    private class Magic_RadialDecent : I_MagicAnimation
    {
        FX_MagicManager manager;
        public TrailedStar[] stars;
        private float time;
        private float constantPIAdd;
        private Vector3 pos;
        Settings_RadialDecent settings => manager.radialSettings;
        public Magic_RadialDecent(FX_MagicManager manager, TrailedStar[] stars, Vector3 pos)
        {
            this.manager = manager;
            this.stars = stars;
            this.pos = pos;
            this.time = 0;
            constantPIAdd = Mathf.PI * 2 / stars.Length;

            //Setup stars;
            Update(0);
            foreach (TrailedStar star in stars)
            {
                star.Enable(this);
            }

            manager.runningAnimations.Add(this);
            manager.UpdateStars += Update;
        }
        public void Update(float delta)
        {
            time += delta * 0.25f;

            float radius = settings.radialDecent.Evaluate(time);
            float move = settings.movement.Evaluate(time);
            Color color = settings.fadeOverTime.Evaluate(time);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].transform.position = pos + Movement(Vector2.right, move) + Rotation(i, radius);
                stars[i].SetColor(color);
            }

            if (time <= 1)
                return;

            Stop();
        }
        public void ForceStop()
        {
            Debug.Log("Force Stoped");
            Stop();
        }
        public void Stop()
        {
            foreach (TrailedStar star in stars)
            {
                star.Disable();
            }
            manager.UpdateStars -= Update;
            manager.runningAnimations.Remove(this);
        }
        private Vector3 Movement(Vector2 dir, float move)
        {
            return dir * move;
        }
        private Vector3 Rotation(int i, float radius)
        {
            float rotation = constantPIAdd * i + time * settings.rotationSpeed;
            float x = Mathf.Sin(rotation) * radius;
            float y = Mathf.Cos(rotation) * radius;
            return (Vector3.right * x + Vector3.up * y);
        }
    }
    [System.Serializable]
    private struct Settings_RadialDecent
    {
        public AnimationCurve radialDecent;
        public Gradient fadeOverTime;
        public AnimationCurve movement;
        public float rotationSpeed;
    }
    [SerializeField] private Settings_RadialDecent radialSettings;

    [SerializeField] GameObject starPrefab;
    //Pool
    private TrailedStar[] starPool;
    private int currentPoolNum;
    private System.Action<float> UpdateStars;
    private List<I_MagicAnimation> runningAnimations;
    public void RadialDesendingMagic(int numberOfStars, Color color)
    {
        Color c = color;
        c.a = 1;
        TrailedStar[] arrayOfStars = GetArrayFromPool(numberOfStars, c);
        new Magic_RadialDecent(this, arrayOfStars, Random.insideUnitCircle * 0.2f);
    }
    private TrailedStar[] GetArrayFromPool(int numberOfStars, Color color)
    {
        TrailedStar[] stars = new TrailedStar[numberOfStars];
        for (int i = 0; i < numberOfStars; i++)
        {
            int poolNum = (i + currentPoolNum) % starPool.Length;
            starPool[poolNum].StopIfOccupied();
            stars[i] = starPool[poolNum];
            stars[i].SetColor(color);
        }
        currentPoolNum = (currentPoolNum + numberOfStars) % starPool.Length;
        return stars;
    }
    // Start is called before the first frame update
    private void Start()
    {
        runningAnimations = new List<I_MagicAnimation>();

        int starPoolSize = 25;
        starPool = new TrailedStar[starPoolSize];
        currentPoolNum = 0;
        for (int i = 0; i < starPoolSize; i++)
        {
            GameObject newStar = Instantiate(starPrefab, transform);
            TrailRenderer trail = newStar.GetComponent<TrailRenderer>();
            SpriteRenderer sprite = newStar.GetComponent<SpriteRenderer>();
            Transform t = newStar.transform;
            starPool[i] = new TrailedStar(t, sprite, trail);
            starPool[i].Disable();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateStars?.Invoke(Time.deltaTime);
    }



    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 500, 20), "magic Length : " + runningAnimations.Count);
    }
}
*/