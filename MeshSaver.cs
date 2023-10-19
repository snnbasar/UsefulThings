//using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MeshSaver : MonoBehaviour
{
    public SkinnedMeshRenderer renderer;
    public MeshFilter meshFilter;
    public string folder = "Assets/0-Project/MeshSaverMeshes/";
    public string fileName = "save";

    //[Button]
    public void SaveAsset(SkinnedMeshRenderer renderer, MeshFilter meshFilter, string folder = "Assets/0-Project/MeshSaverMeshes/", string fileName = "save")
    {
#if UNITY_EDITOR
        
        string meshName = folder + fileName + ".mesh";
        //AssetDatabase.DeleteAsset(chunkName);
        //AssetDatabase.DeleteAsset(col_chunkName);

        if (renderer != null)
            AssetDatabase.CreateAsset(renderer.sharedMesh, meshName);
        else if (meshFilter != null)
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, meshName);
        AssetDatabase.SaveAssets();
#endif
    }
    [ContextMenu(nameof(SaveAsset))]
    public void SaveAsset()
    {
#if UNITY_EDITOR

        string meshName = folder + fileName + ".mesh";
        //AssetDatabase.DeleteAsset(chunkName);
        //AssetDatabase.DeleteAsset(col_chunkName);

        if (renderer != null)
            AssetDatabase.CreateAsset(renderer.sharedMesh, meshName);
        else if (meshFilter != null)
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, meshName);
        AssetDatabase.SaveAssets();
#endif
    }
}