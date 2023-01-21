using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveData(PlayerManager pm, InventoryManager im, RecipeManager rm, WorkerManager wm, FurnitureManager fm) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(pm, im, rm, wm, fm);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Game Saved!");
    }

    public static PlayerData LoadData() {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
