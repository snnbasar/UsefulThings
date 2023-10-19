using Sirenix.OdinInspector;
using UnityEngine;

public class SaveManagerUI : MonoBehaviour
{
    public string savePath = "/save.json";

    [Button]
    public void GetSavePath()
    {
        savePath = Application.dataPath + "/save.json";
    }
    [Button]
    public void SaveFile()
    {
        SaveManager.savePath = this.savePath;
        SaveManager.Save_SaveDataIntoFile();
    }
    [Button]
    public void LoadFile()
    {
        SaveManager.savePath = this.savePath;
        SaveManager.Load_SaveDataFromFile();
    }
}
