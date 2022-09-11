using UnityEngine;

public class FluidSurface : MonoBehaviour
{

    [Header("Fluid Settings")]
    [SerializeField] private SolidFluidScriptableObject settings;
    private int segments;
    private float localHeight;
    private bool sleep;
    private float sleepTime;
    private Spring[] springs;
    private Vector3[] wavePoints;
    private GeneratedWaveMesh generatedMesh;
    private class Spring
    {
        public Vector2 orgPos;
        public float height = 0;
        public float velocity;
        public float restHeight;

        float tension = 1;
        public Vector2 pos;
        float drag = 5f;

        public Spring(Vector2 orgPos, float tension, float drag, float restHeight = 0)
        {
            this.orgPos = orgPos;
            pos = orgPos;
            height = 0;
            velocity = 0;
            this.restHeight = 0;
            this.tension = tension;
            this.drag = drag;
            this.restHeight = restHeight;
        }

        public void SetUpSpring(float tension, float drag, float restHeight = 0)
        {
            this.tension = tension;
            this.drag = drag;
            this.restHeight = restHeight;
        }

        public void UpdateSpring(float delta)
        {
            height = pos.y - orgPos.y;
            float acceleration = tension * (height - restHeight);
            velocity -= acceleration;
            velocity *= 1 - drag * delta;
            pos.y += velocity * delta;
        }
    }
    private class GeneratedWaveMesh
    {
        MeshFilter mFilter;
        public GeneratedWaveMesh(Transform parent, Vector3 size, int segments, Material material, LayerMask layer, int sortOrder, string name = "NewWaveMesh")
        {
            GameObject wave = new GameObject();
            wave.name = name;
            wave.transform.position = parent.position;
            wave.transform.parent = parent;
            wave.gameObject.layer = layer;

            mFilter = wave.AddComponent<MeshFilter>();
            mFilter.mesh = GenerateMesh(size, segments);

            Renderer rend = wave.AddComponent<MeshRenderer>();
            rend.sharedMaterial = material;
            rend.sortingOrder = sortOrder;
        }
        private Mesh GenerateMesh(Vector3 bounds, int segments)
        {
            Mesh mesh = new Mesh();
            Vector3 startPos = -bounds;
            float step = bounds.x * 2 / (float)(segments);
            float height = bounds.y * 2;

            float uvStep = 1 / (float)segments;

            int arrLength = segments + 1;
            Vector3[] verts = new Vector3[arrLength * 2];
            Vector3[] normals = new Vector3[verts.Length];
            Vector2[] uvs = new Vector2[verts.Length];

            //Upper half
            for (int i = 0; i < arrLength; i++)
            {
                Vector3 vertPos =
                    startPos +
                    Vector3.right * step * i +
                    Vector3.up * height
                    ;

                verts[i] = vertPos;
                normals[i] = -Vector3.forward;
                uvs[i] = new Vector2(uvStep * i, 1);
            }

            //lower half
            int x = 0;
            for (int i = arrLength; i < verts.Length; i++)
            {
                Vector3 vertPos =
                    startPos +
                    Vector3.right * step * x
                    ;

                verts[i] = vertPos;
                normals[i] = -Vector3.forward;
                uvs[i] = new Vector2(uvStep * x, 0);
                x++;
            }


            mesh.name = "NewWaveMesh";
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.triangles = GenerateFaces(verts, segments);
            mesh.RecalculateBounds();

            return mesh;
        }
        private int[] GenerateFaces(Vector3[] verts, int segments)
        {
            int[] faces = new int[segments * 6];

            int x = 0;
            int f = 0;
            for (int i = 0; i < segments; i++)
            {
                f = i * 6;
                faces[f] = i;
                x++;
                faces[f + 1] = i + 1;
                x++;
                faces[f + 2] = i + segments + 1;

                x++;
                faces[f + 3] = i + segments + 1;
                x++;
                faces[f + 4] = i + 1;
                x++;
                faces[f + 5] = i + segments + 2;
            }
            return faces;
        }

        public MeshFilter GetMeshFilter()
        {
            return mFilter;
        }
        public void UpdateWave(Vector3[] verts)
        {
            mFilter.mesh.vertices = verts;
        }
    }


    public void Impact(Vector2 pos, float impact, float size)
    {
        int closest = -1;
        float compDist = Mathf.Infinity;
        for (int i = 0; i < springs.Length; i++)
        {
            float dist = (springs[i].orgPos- pos).sqrMagnitude;
            if (dist<compDist)
            {
                closest = i;
                compDist = dist;
            }
        }

        if (closest == -1)
            return;

#if UNITY_EDITOR
        Debug.DrawRay(springs[closest].pos, Vector2.down * impact, Color.red, 3);
#endif
        springs[closest].velocity += impact;
        WakeUp();
    }


    // Start is called before the first frame update
    public void Init(Vector3 halfSize)
    {

        localHeight = halfSize.y;
        int numberOfSprings = (int)(halfSize.x * 2 * settings.springsPerMeter);
        segments = numberOfSprings * 4;
        numberOfSprings += 1;

        //Create assets
        CreateMesh(halfSize);
        if (settings.createMask)
            CreateMask();
        CreateSprings(numberOfSprings, halfSize);
        CreateWavePoints(generatedMesh.GetMeshFilter());

        if (settings.destroyOriginalRenderer)
        {
            if (TryGetComponent(out SpriteRenderer sr))
            {
                Destroy(sr);
            }
        }
    }

    private void CreateMesh(Vector3 halfSize)
    {
        Material meshMaterial;
        LayerMask layer;
        int rendSortOder = 0;
        if (settings.getFromParent && TryGetComponent(out SpriteRenderer sr))
        {
            meshMaterial = sr.material;
            meshMaterial.color = sr.color;
            meshMaterial.mainTexture = sr.sprite.texture;
            layer = sr.gameObject.layer;
            rendSortOder = sr.sortingOrder;
        }
        else
        {
            meshMaterial = settings.material;
            layer = settings.layer;
        }

        generatedMesh =
            new GeneratedWaveMesh(transform, halfSize, segments, meshMaterial, layer, rendSortOder);
    }
    private void CreateMask()
    {
        GameObject generatedMeshMask = new GameObject();
        generatedMeshMask.name = "maskMesh";
        generatedMeshMask.transform.position = transform.position;
        generatedMeshMask.transform.rotation = Quaternion.identity;
        generatedMeshMask.transform.parent = transform;

        MeshFilter maskFilter = generatedMeshMask.AddComponent<MeshFilter>();
        maskFilter.sharedMesh = generatedMesh.GetMeshFilter().mesh;

        Renderer maskRenderer = generatedMeshMask.AddComponent<MeshRenderer>();
        maskRenderer.material = settings.maskMaterial;
        generatedMeshMask.layer = settings.maskLayer;
    }
    private void CreateSprings(int numberOfSprings, Vector3 bounds)
    {
        Vector3 startPos = transform.position + bounds;
        startPos.x -= bounds.x * 2;
        Vector3 step = Vector3.right * (1f / (float)settings.springsPerMeter);

        springs = new Spring[numberOfSprings];
        for (int i = 0; i < springs.Length; i++)
        {
            springs[i] = new Spring(startPos + step * i, settings.tension, settings.drag, 0);
        }
    }
    private void CreateWavePoints(MeshFilter genMesh)
    {
        wavePoints = genMesh.mesh.vertices;
    }

    // Update is called from manager
    public void UpdateFluid(float delta)
    {
        if (sleep)
            return;

        float combinedVelocity = 0;
        for (int i = 0; i < springs.Length; i++)
        {
            springs[i].UpdateSpring(delta);
            combinedVelocity += springs[i].velocity * springs[i].velocity;
        }
        SpreadWave();
        CheckSleep(combinedVelocity, delta);

        generatedMesh.UpdateWave(wavePoints);
    }
    public void WakeUp()
    {
        sleepTime = settings.timeBeforeSleep;
        sleep = false;
    }
    private void CheckSleep(float combinedVelocity, float delta)
    {
        if (combinedVelocity < settings.sleepDelta)
        {
            sleepTime -= Time.deltaTime;

            if (sleepTime < 0)
            {
                Sleep();
            }
            return;
        }
        sleepTime = settings.timeBeforeSleep;
        sleep = false;
    }
    private void Sleep()
    {
        for (int i = 0; i < segments; i++)
        {
            wavePoints[i].y = localHeight;
        }
        generatedMesh.UpdateWave(wavePoints);
        sleep = true;
    }
    private void SpreadWave()
    {
        float volume = 0;
        springs[0].restHeight = springs[1].height * settings.spread;

        for (int i = 1; i < springs.Length - 1; i++)
        {
            volume = (springs[i - 1].height + springs[i + 1].height) * 0.5f;
            springs[i].restHeight = volume * settings.spread;
        }
        springs[springs.Length - 1].restHeight = springs[1].height * settings.spread;

        CatSplineWave();
    }


    private void CatSplineWave()
    {
        Vector2 pos = transform.position;
        int x = 0;
        //Edgecase 0
        Vector2 pOut0 = springs[0].pos;
        Vector2 p1 = springs[0].pos;
        Vector2 p5 = springs[1].pos;
        Vector2 pOut6 = springs[2].pos;

        Vector2 dir1 = HalfPoint(p1 - pOut0, p5 - p1);
        Vector2 dir2 = HalfPoint(p5 - pOut6, p1 - p5);
        dir1 *= settings.smoothValue;
        dir2 *= settings.smoothValue;

        Vector2 p2 = p1 + dir1;
        Vector2 p4 = p5 + dir2;

        Vector2 p3 = HalfPoint(p2, p4);
        p2 = HalfPoint(p1, p2);
        p4 = HalfPoint(p4, p5);

        Vector2 dropP1 = HalfPoint(p1, p2);
        Vector2 dropP2 = HalfPoint(p2, p3);
        Vector2 dropP3 = HalfPoint(p3, p4);
        Vector2 dropP4 = HalfPoint(p4, p5);

        p2 = HalfPoint(dropP1, dropP2);
        p3 = HalfPoint(dropP2, dropP3);
        p4 = HalfPoint(dropP3, dropP4);

        wavePoints[x] = p1 - pos;
        wavePoints[x + 1] = p2 - pos;
        wavePoints[x + 2] = p3 - pos;
        wavePoints[x + 3] = p4 - pos;
        x += 4;


        //Normal
        for (int i = 1; i < springs.Length - 2; i++)
        {
            pOut0 = springs[i - 1].pos;
            p1 = springs[i].pos;
            p5 = springs[i + 1].pos;
            pOut6 = springs[i + 2].pos;

            dir1 = HalfPoint(p1 - pOut0, p5 - p1);
            dir2 = HalfPoint(p5 - pOut6, p1 - p5);
            dir1 *= settings.smoothValue;
            dir2 *= settings.smoothValue;

            p2 = p1 + dir1;
            p4 = p5 + dir2;

            p3 = HalfPoint(p2, p4);
            p2 = HalfPoint(p1, p2);
            p4 = HalfPoint(p4, p5);

            dropP1 = HalfPoint(p1, p2);
            dropP2 = HalfPoint(p2, p3);
            dropP3 = HalfPoint(p3, p4);
            dropP4 = HalfPoint(p4, p5);

            p2 = HalfPoint(dropP1, dropP2);
            p3 = HalfPoint(dropP2, dropP3);
            p4 = HalfPoint(dropP3, dropP4);

            wavePoints[x] = p1 - pos;
            wavePoints[x + 1] = p2 - pos;
            wavePoints[x + 2] = p3 - pos;
            wavePoints[x + 3] = p4 - pos;
            x += 4;
        }

        //Edgecase last
        pOut0 = springs[springs.Length - 3].pos;
        p1 = springs[springs.Length - 2].pos;
        p5 = springs[springs.Length - 1].pos;
        pOut6 = springs[springs.Length - 1].pos;

        dir1 = HalfPoint(p1 - pOut0, p5 - p1);
        dir2 = HalfPoint(p5 - pOut6, p1 - p5);
        dir1 *= settings.smoothValue;
        dir2 *= settings.smoothValue;

        p2 = p1 + dir1;
        p4 = p5 + dir2;

        p3 = HalfPoint(p2, p4);
        p2 = HalfPoint(p1, p2);
        p4 = HalfPoint(p4, p5);

        dropP1 = HalfPoint(p1, p2);
        dropP2 = HalfPoint(p2, p3);
        dropP3 = HalfPoint(p3, p4);
        dropP4 = HalfPoint(p4, p5);

        p2 = HalfPoint(dropP1, dropP2);
        p3 = HalfPoint(dropP2, dropP3);
        p4 = HalfPoint(dropP3, dropP4);

        wavePoints[x] = p1 - pos;
        wavePoints[x + 1] = p2 - pos;
        wavePoints[x + 2] = p3 - pos;
        wavePoints[x + 3] = p4 - pos;
        wavePoints[x + 4] = springs[springs.Length - 1].pos - pos;
    }
    private Vector2 HalfPoint(Vector2 p1, Vector2 p2)
    {
        return (p1 + p2) * 0.5f;
    }
}
