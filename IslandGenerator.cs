using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [Header("GENERATE ISLANDS")]
    public GameObject[] islands;
    public int islandCount;
    public float islandOffset;
    public float islandScaleMin = 1;
    public float islandScaleMax = 2;
    public float randomness = 1;
    public List<GameObject> islandList = new List<GameObject>();

    [Button]
    public void GenerateIslands()
    {
#if UNITY_EDITOR
        for (int i = 0; i < islandCount; i++)
        {
            GameObject island = PrefabUtility.InstantiatePrefab(islands.GetRandomItem(), transform) as GameObject;
            islandList.Add(island);
            Transform t = island.transform;
            t.localScale = Vector3.one * Random.Range(islandScaleMin, islandScaleMax);
            t.localPosition = Vector3.zero;
            float z = islandOffset * i /*+ Random.Range(-randomness, randomness)*/;
            float x = Random.Range(-randomness, randomness);
            t.localPosition = new Vector3(x, t.localPosition.y, z);
            //t.localEulerAngles = new Vector3(t.localEulerAngles.x, Random.Range(0f, 1f) * 360f, t.localEulerAngles.z);
        } 
#endif


    }

    [Button]
    public void DeleteIslands()
    {
        for (int i = 0; i < islandList.Count; i++)
        {
            DestroyImmediate(islandList[i]);
        }
        islandList.Clear();
    }
}
