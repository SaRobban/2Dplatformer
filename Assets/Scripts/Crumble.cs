using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crumble : MonoBehaviour
{
    private Rigidbody2D[] childs;

    // Start is called before the first frame update
    void Start()
    {
        List<Rigidbody2D> list = new List<Rigidbody2D>();
        foreach(Transform t in transform)
        {
            Rigidbody2D rb;
            if (t.gameObject.TryGetComponent(out rb))
            {
                list.Add(rb);
            }
        }

        List<Rigidbody2D> SortedList = list.OrderBy(o => o.transform.position.y).ToList();
        childs = SortedList.ToArray();
        StartCoroutine(BreakApart());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator BreakApart()
    {
        yield return new WaitForSeconds(5);
        for(int i = 0; i < childs.Length; i++)
        {
            yield return new WaitForFixedUpdate();
            childs[i].isKinematic = false;
            childs[i].AddTorque(Random.Range(-1, 1),ForceMode2D.Impulse);
            yield return new WaitForSeconds(Random.Range(0f,0.25f));
        }
    }
}
