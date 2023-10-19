using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFormation : FormationBase
{

    public GameObject Hexagon;
    public uint Radius;
    public float HexSideMultiplier = 1;

    private const float sq3 = 1.7320508075688772935274463415059F;
    Vector3 currentPoint;
    //Vector3[] mv;
    private void Start()
    {

        //currentPoint = transform.position;

        ////Spawn scheme: nDR, nDX, nDL, nUL, nUX, End??, UX, nUR
        //Vector3[] a = {
        //    new Vector3(1.5f, 0, -sq3 * 0.5f),       //DR
        //    new Vector3(0, 0, -sq3),               //DX
        //    new Vector3(-1.5f, 0, -sq3 * 0.5f),      //DL
        //    new Vector3(-1.5f, 0, sq3 * 0.5f),       //UL
        //    new Vector3(0, 0, sq3),                //UX
        //    new Vector3(1.5f, 0, sq3 * 0.5f)         //UR
        //};
        //mv = a;

    }

    public override IEnumerable<Vector3> EvaluatePoints()
    {

        #region OldCode
        //int lmv = mv.Length;
        //float HexSide = Hexagon.transform.localScale.x * HexSideMultiplier;

        //Radius = (uint)(Amount / lmv + 1);
        //int spawnCount = (int)Amount;


        //for (int mult = 0; mult <= Radius; mult++)
        //{
        //    int hn = 0;
        //    for (int j = 0; j < spawnCount; j++)
        //    {
        //        if (j > lmv - 1)
        //            break;
        //        for (int i = 0; i < mult; i++, hn++)
        //        {
        //            yield return currentPoint;

        //            currentPoint += (mv[j] * HexSide);
        //        }

        //        if (j == 4)
        //        {
        //            yield return currentPoint;


        //            currentPoint += (mv[j] * HexSide);
        //            hn++;
        //            if (mult == Radius)
        //                break;      //Finished
        //        }

        //    }
        //    if (mult > 0)
        //        spawnCount -= lmv;
        //}

        //Vector3 pos = currentPoint;

        //for (int i = 0; i < Amount; i++)
        //{
        //    yield return pos;
        //    int sa = i % (lmv - 1);
        //    pos += (mv[sa] * HexSide);
        //    print(pos);

        //    if (sa == 4)
        //    {

        //        if (i == Amount)
        //            break;      //Finished
        //    }
        //}

        #endregion


        #region OriginalCode
        ////Point of the next hexagon to be spawned
        //Vector3 currentPoint = transform.position;


        ////Spawn scheme: nDR, nDX, nDL, nUL, nUX, End??, UX, nUR
        //Vector3[] mv = {
        //    new Vector3(1.5f,0, -sq3*0.5f),       //DR
        //    new Vector3(0,0, -sq3),               //DX
        //    new Vector3(-1.5f,0, -sq3*0.5f),      //DL
        //    new Vector3(-1.5f,0, sq3*0.5f),       //UL
        //    new Vector3(0,0, sq3),                //UX
        //    new Vector3(1.5f,0, sq3*0.5f)         //UR
        //};

        //int lmv = mv.Length;
        //float HexSide = Hexagon.transform.localScale.x * HexSideMultiplier;

        //for (int mult = 0; mult <= Radius; mult++)
        //{
        //    for (int j = 0; j < lmv; j++)
        //    {
        //        for (int i = 0; i < mult; i++)
        //        {
        //            yield return currentPoint;
        //            currentPoint += (mv[j] * HexSide);
        //        }
        //        if (j == 4)
        //        {
        //            yield return currentPoint;
        //            currentPoint += (mv[j] * HexSide);
        //            if (mult == Radius)
        //                break;      //Finished
        //        }
        //    }
        //} 
        #endregion

        if (amount <= 0)
            yield break;
        //Point of the next hexagon to be spawned
        Vector3 currentPoint = transform.position;

        //X,Z formation
        //Spawn scheme: nDR, nDX, nDL, nUL, nUX, End??, UX, nUR
        //Vector3[] mv = {
        //    new Vector3(1.5f,0, -sq3*0.5f),       //DR
        //    new Vector3(0,0, -sq3),               //DX
        //    new Vector3(-1.5f,0, -sq3*0.5f),      //DL
        //    new Vector3(-1.5f,0, sq3*0.5f),       //UL
        //    new Vector3(0,0, sq3),                //UX
        //    new Vector3(1.5f,0, sq3*0.5f)         //UR
        //};

        //X, Y Formation
        //Vector3[] mv = {
        //    new Vector3(0, 1.5f, -sq3*0.5f),       //DR
        //    new Vector3(0, 0, -sq3),               //DX
        //    new Vector3(0, -1.5f, -sq3*0.5f),      //DL
        //    new Vector3(0, -1.5f, sq3*0.5f),       //UL
        //    new Vector3(0, 0, sq3),                //UX
        //    new Vector3(0, 1.5f, sq3*0.5f)         //UR
        //};

        //Y,Z Formation
        Vector3[] mv = {
            new Vector3(1.5f,-sq3*0.5f, 0),       //DR
            new Vector3(0,-sq3, 0),               //DX
            new Vector3(-1.5f,-sq3*0.5f, 0),      //DL
            new Vector3(-1.5f,sq3*0.5f, 0),       //UL
            new Vector3(0,sq3, 0),                //UX
            new Vector3(1.5f,sq3*0.5f, 0)         //UR
        };

        int lmv = mv.Length;
        float HexSide = Hexagon.transform.localScale.y * HexSideMultiplier;

        int spawnedCount = 0;
        for (int mult = 0; mult <= Radius; mult++)
        {
            if (spawnedCount == amount)
                yield break;
            for (int j = 0; j < lmv; j++)
            {
                for (int i = 0; i < mult; i++)
                {
                    yield return currentPoint;
                    currentPoint += (mv[j] * HexSide);

                    //currentPoint += GetNoise(currentPoint);
                    //currentPoint *= Spread;

                    spawnedCount++;
                    if (spawnedCount == amount)
                        yield break;
                }
                if (j == 4)
                {
                    yield return currentPoint;
                    currentPoint += (mv[j] * HexSide);

                    //currentPoint += GetNoise(currentPoint);
                    //currentPoint *= Spread;

                    spawnedCount++;
                    if (spawnedCount == amount)
                        yield break;
                    if (mult == Radius)
                        break;      //Finished
                }
            }
        }
    }
}