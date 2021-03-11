using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public static class SaveSystem
{
    public static void SaveProgress(ProgressData data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProgressData LoadProgress()
    {
        string path = Application.persistentDataPath + "/progress.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressData data = binaryFormatter.Deserialize(stream) as ProgressData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            Debug.LogError("Returning empty save file.");
            return new ProgressData();
        }
    }

    public static void ClearProgress()
    {
        SaveProgress(new ProgressData());
    }
}