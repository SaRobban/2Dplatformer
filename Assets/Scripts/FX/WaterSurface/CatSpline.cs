using UnityEngine;

public class CatSpline
{
    //private Vector2[] catSpline;
    public float smoothValue;
    public CatSpline(float smoothValue = 0.5f)
    {
        this.smoothValue = smoothValue;
    }

    public Vector2[] GetCatSpline(Vector2[] points)
    {
        Vector2[] toArray = new Vector2[points.Length * 4 - 3];
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

        toArray[x] = p1;
        toArray[x + 1] = p2;
        toArray[x + 2] = p3;
        toArray[x + 3] = p4;
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

            Debug.DrawLine(p1, p2);
            Debug.DrawLine(p2, p3);
            Debug.DrawLine(p3, p4);
            Debug.DrawLine(p4, p5);

            toArray[x] = p1;
            toArray[x + 1] = p2;
            toArray[x + 2] = p3;
            toArray[x + 3] = p4;
            x += 4;
        }

        //Edgecase last
        pOut0 = points[points.Length - 3];
        p1 = points[points.Length - 2];
        p5 = points[points.Length - 1];
        pOut6 = points[points.Length - 1];

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

        toArray[x] = p1;
        toArray[x + 1] = p2;
        toArray[x + 2] = p3;
        toArray[x + 3] = p4;
        toArray[toArray.Length - 1] = points[points.Length - 1];
        return toArray;
    }
    private Vector2 HalfPoint(Vector2 p1, Vector2 p2)
    {
        return (p1 + p2) * 0.5f;
    }
}
