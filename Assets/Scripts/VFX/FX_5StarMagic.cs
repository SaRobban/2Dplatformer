/*
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FX_5StarMagic : MonoBehaviour
{
    [SerializeField] private CastStarAnimation castStar = new CastStarAnimation();

    [Serializable]
    public class CastStarAnimation
    {
        FX_5StarMagic owner;

        [Header("Animation")]
        [SerializeField] AnimationCurve shrinkCurve;
        [SerializeField] AnimationCurve moveCurve;
        [SerializeField] float evaluationPoint;
        [SerializeField] float startRadius;
        [SerializeField] float speed;

        [Header("Setup")]
        [SerializeField] private GameObject starPreFab;
        [SerializeField] private int numberOfStars = 5;
        [SerializeField] private float rotationSpeed = 3;

        private float radius = 1.5f;
        private float constantPIplus;
        private Vector3 magicPos;
        private StarTrail[] starPool;
        public class StarTrail
        {
            public GameObject star;
            TrailRenderer trail;
            public StarTrail(GameObject preFab, Transform parent)
            {
                star = Instantiate(preFab, parent);
                trail = star.GetComponent<TrailRenderer>();
                Diseble();
            }

            public void UpdatePos(Vector3 pos)
            {
                star.transform.position = pos;
            }

            public void Move(Vector3 force)
            {
                star.transform.position += force;
            }

            public void Eneble()
            {
                star.gameObject.SetActive(true);
                trail.enabled = true;
                trail.Clear();
            }
            public void Diseble()
            {
                trail.Clear();
                trail.enabled = false;
                star.gameObject.SetActive(false);
            }
        }
        public void Init(Transform parent)
        {
            constantPIplus = Mathf.PI * 2 / numberOfStars;
            starPool = new StarTrail[numberOfStars];
            for (int i = 0; i < starPool.Length; i++)
            {
                StarTrail newStar = new StarTrail(starPreFab, parent);
                starPool[i] = newStar;
            }

            DisebleStars();
        }
        public void Begin(FX_5StarMagic owner, Vector3 pos)
        {
            this.owner = owner;
            magicPos = pos;
            evaluationPoint = 0;
            radius = startRadius;
            EnebleStarts();
            owner.UpdateAnims += Update;
        }
        public void Update(float delta)
        {
            if (evaluationPoint >= 1)
            {
                DisebleStars();
                owner.UpdateAnims -= Update;
            }


                EvaluateAnim();
            evaluationPoint += delta;
        }

        public void EvaluateAnim()
        {
            radius = shrinkCurve.Evaluate(evaluationPoint);
            magicPos = Vector3.right * moveCurve.Evaluate(evaluationPoint) * speed;
            RotateStar();
        }

        public void Stop()
        {
            DisebleStars();
        }
        void EnebleStarts()
        {
            foreach (StarTrail s in starPool)
                s.Eneble();
        }



        void ShrinkStarRadius()
        {
            radius -= Time.deltaTime * rotationSpeed;
        }
        void RotateStar()
        {
            float rotationTime = Time.time * rotationSpeed;
            for (int i = 0; i < starPool.Length; i++)
            {
                float x = Mathf.Sin(rotationTime + constantPIplus * i) * radius;
                float y = Mathf.Cos(rotationTime + constantPIplus * i) * radius;
                starPool[i].UpdatePos(magicPos + new Vector3(x, y, 0));
            }
        }

 
        void DisebleStars()
        {
            foreach (StarTrail s in starPool)
                s.Diseble();

        }
    }


    public Action<float> UpdateAnims;

    public float castTime = 4;
    public float currentTime;

    private void Start()
    {
        castStar.Init(transform);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateAnims?.Invoke(Time.deltaTime);
       

        if (currentTime <= 0)
            castStar.Stop();

        if (currentTime <= -1)
        {
            castStar.Begin(this,UnityEngine.Random.insideUnitCircle * 2);
            currentTime = castTime;
        }


        currentTime -= Time.deltaTime;
    }


}
*/