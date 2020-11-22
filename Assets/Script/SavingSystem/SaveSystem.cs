using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem{
    
    public static void SaveData(Transform playerTransform, Inventory inventory)
    {
        Debug.Log("Save game data");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/gameData.gamedata";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(playerTransform, inventory);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        Debug.Log("Load game data");
        string path = Application.persistentDataPath + "/gameData.gamedata";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("There is no data in the file");
            return null;
        }
    }
}
