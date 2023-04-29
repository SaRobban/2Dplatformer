using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPlacement : MonoBehaviour
{
    public LayerMask layerMask;
    //    Collider2D[] boxHits = new Collider2D[32];
    MainCharacter mainCharacter;
    RayCastPortal raycastPortal;
    PlacePortal placePortal;
    [SerializeField] PortalDoor portalGO;


    private class RayCastPortal
    {
        private float frwRange;
        private float rearRange;
        private float portalHalfSize;
        private LayerMask layerMask;
        RaycastHit2D[] rayHits;

        public RayHitInfo forwardPortal;
        public RayHitInfo rearPortal;

        public class RayHitInfo
        {
            public Vector2 point;
            public Vector2 normal;
            public Collider2D collider;
            public RayHitInfo(Vector2 point, Vector2 dir, Collider2D collider)
            {
                this.point = point;
                this.normal = dir;
                this.collider = collider;
            }
        }

        public RayCastPortal(LayerMask layerMask, float rayLengthForward, float rayLengthRear, float portalSize)
        {
            this.frwRange = rayLengthForward;
            this.rearRange = rayLengthRear;
            this.layerMask = layerMask;
            this.portalHalfSize = portalSize * 0.5f;
            rayHits = new RaycastHit2D[8];
        }

        public bool TryPlacePortal(Vector2 pos, Vector2 dir)
        {
            forwardPortal = CanFindPlacementSurface(pos, dir, frwRange);
            if (forwardPortal == null)
                return false;

            rearPortal = CanFindPlacementSurface(pos, -dir, rearRange);
            if (rearPortal == null)
                return false;

            if (!CanFitPortal(forwardPortal))
                return false;
            if (!CanFitPortal(rearPortal))
                return false;

            return true;
        }

        public RayHitInfo CanFindPlacementSurface(Vector2 pos, Vector2 dir, float range)
        {
            int hits = Physics2D.RaycastNonAlloc(pos, dir, rayHits, range, layerMask);
            if (hits <= 0)
                return null;

            int closest = -1;
            float testDist = Mathf.Infinity;
            for (int i = 0; i < hits; i++)
            {
                float dist = rayHits[i].distance;
                if (dist < testDist)
                {
                    closest = i;
                    testDist = dist;
                }
            }

            if (closest == -1)
                return null;

            Vector2 point = rayHits[closest].point;
            Vector2 normal = rayHits[closest].normal;
            Collider2D hitCollider = rayHits[closest].collider;

            Debug.DrawRay(point, normal, Color.cyan, 5);
            return new RayHitInfo(point, normal, hitCollider);

        }

        public bool CanFitPortal(RayHitInfo info)
        {
            Vector2 testPoint = info.point + info.normal * 0.01f;
            Vector2 testDir = new Vector2(info.normal.y, -info.normal.x);

            Debug.DrawRay(testPoint, testDir, Color.yellow, 5);

            int hits = Physics2D.RaycastNonAlloc(testPoint, testDir, rayHits, portalHalfSize, layerMask);
            if (hits > 0)
                return false;

            testDir *= -1;
            Debug.DrawRay(testPoint, testDir, Color.magenta, 5);
            hits = Physics2D.RaycastNonAlloc(testPoint, testDir, rayHits, portalHalfSize, layerMask);
            if (hits > 0)
                return false;

            return true;
        }
    }

    private class PlacePortal
    {
        PortalDoor portalOne;
        PortalDoor portalTwo;

        public PlacePortal(PortalDoor portal)
        {
            portalOne = Instantiate(portal);
            portalTwo = Instantiate(portal);

            portalOne.Connect(portalTwo);
            portalTwo.Connect(portalOne);

            portalOne.gameObject.SetActive(false);
            portalTwo.gameObject.SetActive(false);
        }
        public void At(RayCastPortal.RayHitInfo infoFrw, RayCastPortal.RayHitInfo infoRear)
        {
            if(!portalOne.isActiveAndEnabled)
                portalOne.InitializeWarp(infoFrw.point, infoFrw.normal, infoFrw.collider);
            if(!portalTwo.isActiveAndEnabled)
                portalTwo.InitializeWarp(infoRear.point, infoRear.normal, infoRear.collider);
        }

        public void KillPortals()
        {
            if(portalOne.isActiveAndEnabled)
               portalOne.KillWarp();
    
            if(portalTwo.isActiveAndEnabled)
                portalTwo.KillWarp();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        mainCharacter = PlayerMain.mainCharacter;
        raycastPortal = new RayCastPortal(layerMask, 0.5f, 1000, 0.5f);
        placePortal = new PlacePortal(portalGO);
    }


    //TODO : ACTIVATE BY SUBSCRIBE
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {

            Vector2 aimdir = AimDirection();
            Vector2 point = mainCharacter.GetPosition() + Vector2.up * mainCharacter.stats.WaistHeight;

        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            Vector2 aimdir = AimDirection();
            Vector2 point = mainCharacter.GetPosition() + Vector2.up * mainCharacter.stats.WaistHeight;
            if (!raycastPortal.TryPlacePortal(point, aimdir))
                return;

            placePortal.At(raycastPortal.forwardPortal, raycastPortal.rearPortal);
            Debug.DrawRay(point, aimdir, Color.red, 5);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            placePortal.KillPortals();
        }
    }

    Vector2 AimDirection()
    {
        Vector2 dir = Vector2.right;
        if (mainCharacter.sprite.flipX)
            dir = Vector2.left;
        return dir;
    }


}
