
using System.IO;
using UnityEngine;

public class SaveLoadService : MonoBehaviour
{
    public BuildManager build;
    string savePath;


    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "scaffold.json");
    }


    public void Save()
    {
        var data = build.GetSaveData();
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("Saved to " + savePath);
    }


    public void Load()
    {
        if (!File.Exists(savePath)) return;
        string json = File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<BuildSave>(json);
        build.LoadFromSave(data);
        Debug.Log("Loaded from " + savePath);
    }
}
