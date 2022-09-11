using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_TanSpline : MonoBehaviour
{
    public float smoothValue = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //apoliges Catman
     
      
    }



    Vector2 HalfPoint(Vector2 p1, Vector2 p2)
    {
        return (p1 + p2) * 0.5f;
    }


    Vector2[] CatLine(Vector2[] points)
    {
        Vector2[] catLine = new Vector2[points.Length * 4-3];
        int x = 0;
        //Edgecase 0
        Vector2 pOut0 = points[0];
        Vector2 p1 = points[0];
        Vector2 p5 = points[1];
        Vector2 pOut6 = points[2];

        Vector2 dir1 = HalfPoint(p1 - pOut0, p5 - p1);
        Vector2 dir2 = HalfPoint(p5 - pOut6, p1 - p5);
        dir1 *= smoothValue;
        dir2 *= smoothValue;

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

        catLine[x] = p1;
        catLine[x + 1] = p2;
        catLine[x + 2] = p3;
        catLine[x + 3] = p4;
        x += 4;


        //Normal
        for (int i = 1; i < points.Length - 2; i++)
        {
             pOut0 = points[i - 1];
             p1 = points[i];
             p5 = points[i + 1];
             pOut6 = points[i + 2];

             dir1 = HalfPoint(p1 - pOut0, p5 - p1);
             dir2 = HalfPoint(p5 - pOut6, p1 - p5);
            dir1 *= smoothValue;
            dir2 *= smoothValue;

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

            catLine[x] = p1;
            catLine[x + 1] = p2;
            catLine[x + 2] = p3;
            catLine[x + 3] = p4;
            x += 4;
        }

        //Edgecase last
        pOut0 = points[points.Length-3];
        p1 = points[points.Length-2];
        p5 = points[points.Length-1];
        pOut6 = points[points.Length-1];

        dir1 = HalfPoint(p1 - pOut0, p5 - p1);
        dir2 = HalfPoint(p5 - pOut6, p1 - p5);
        dir1 *= smoothValue;
        dir2 *= smoothValue;

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

        catLine[x] = p1;
        catLine[x + 1] = p2;
        catLine[x + 2] = p3;
        catLine[x + 3] = p4;
        catLine[catLine.Length - 1] = points[points.Length - 1];
        return catLine;
    }



    void DrawLine(Vector2[] points, Color col1, Color col2)
    {
        for (int x = 0; x < points.Length - 1; x++)
        {
            Debug.DrawLine(points[x], points[x + 1], Color.Lerp(
                col1, col2,
                (1f / (float)points.Length) * x)
                );
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector2[] points = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(2, 2) };
        DrawLine(points, Color.black, Color.black);
        DrawLine(CatLine(points), Color.magenta, Color.cyan);
    }

        /*
    void Test()
    {
        for (int i = 1; i < points.Length - 2; i++)
        {
            Vector2 pOut0 = points[i - 1];
            Vector2 p1 = points[i];
            Vector2 p5 = points[i + 1];
            Vector2 pOut6 = points[i + 2];

            Vector2 dir1 = HalfPoint(p1 - pOut0, p5 - p1);
            //Debug.DrawRay(p1, dir1, Color.green, 100);

            Vector2 dir2 = HalfPoint(p5 - pOut6, p1 - p5);
            ///Debug.DrawRay(p5, dir2, Color.red, 100);

            dir1 *= smoothValue;
            dir2 *= smoothValue;



            Vector2 p2 = HalfPoint(p1, p1 + dir1);
            Vector2 p4 = HalfPoint(p5, p5 + dir2);

            Debug.DrawLine(p1, p2, Color.gray, 100);
            Debug.DrawLine(p2, p4, Color.gray, 100);
            Debug.DrawLine(p4, p5, Color.gray, 100);

            Vector2 p3 = HalfPoint(p2, p4);
            p2 = HalfPoint(p1, p2);
            p4 = HalfPoint(p4, p5);

            Debug.DrawLine(p1, p2, Color.gray, 100);
            Debug.DrawLine(p2, p3, Color.gray, 100);
            Debug.DrawLine(p3, p4, Color.gray, 100);
            Debug.DrawLine(p4, p5, Color.gray, 100);
          
            Vector2 s1 = HalfPoint(p1, p2);
            Vector2 s2 = HalfPoint(p2, p3);
            Vector2 s3 = HalfPoint(p3, p4);
            Vector2 s4 = HalfPoint(p4, p5);

            Debug.DrawLine(p1, s1, Color.gray, 100);
            Debug.DrawLine(s1, s2, Color.gray, 100);
            Debug.DrawLine(s2, s3, Color.gray, 100);
            Debug.DrawLine(s3, s4, Color.gray, 100);
            Debug.DrawLine(s4, p5, Color.gray, 100);

            Vector2 c1 = HalfPoint(p1, s1);
            Vector2 c2 = HalfPoint(s1, s2);
            Vector2 c3 = HalfPoint(s2, s3);
            Vector2 c4 = HalfPoint(s3, s4);
            Vector2 c5 = HalfPoint(s4, p5);

            Debug.DrawLine(p1, c1, Color.gray, 100);
            Debug.DrawLine(c1, c2, Color.gray, 100);
            Debug.DrawLine(c2, c3, Color.gray, 100);
            Debug.DrawLine(c3, c4, Color.gray, 100);
            Debug.DrawLine(c4, c5, Color.gray, 100);
            Debug.DrawLine(c5, p5, Color.gray, 100);
            
            Vector2 d1 = HalfPoint(p1, c1);
            Vector2 d2 = HalfPoint(c1, c2);
            Vector2 d3 = HalfPoint(c2, c3);
            Vector2 d4 = HalfPoint(c3, c4);
            Vector2 d5 = HalfPoint(c4, c5);
            Vector2 d6 = HalfPoint(c5, p5);

            Debug.DrawLine(p1, d1, Color.red, 100);
            Debug.DrawLine(d1, d2, Color.red, 100);
            Debug.DrawLine(d2, d3, Color.red, 100);
            Debug.DrawLine(d3, d4, Color.red, 100);
            Debug.DrawLine(d4, d5, Color.red, 100);
            Debug.DrawLine(d5, d6, Color.red, 100);
            Debug.DrawLine(d6, p5, Color.red, 100);
            continue;

            p2 = HalfPoint(s1, s2);
            p4 = HalfPoint(s3, s4);
            p3 = HalfPoint(s2, s3);


            Debug.DrawLine(p1, p2, Color.green, 100);
            Debug.DrawLine(p2, p3, Color.green, 100);
            Debug.DrawLine(p3, p4, Color.green, 100);
            Debug.DrawLine(p4, p5, Color.green, 100);

        }
    
    }*/
}
