using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{

    public static void SaveGame(string currentScene, Vector3 position, bool isBossUnlocked)
    {
        BinaryFormatter formatter = new BinaryFormatter();
	    FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat"); 
	    SaveData data = new SaveData(currentScene, position, isBossUnlocked);


	    formatter.Serialize(file, data);
	    file.Close();
    }

    public static SaveData LoadGame(){
        string path = Application.persistentDataPath + "/saveData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);
            file.Close();
            
            return data;
            
        }
        else{
            return null;
        }
    }

    public static void DeleteData()
    {
        SaveData data = LoadGame();
        if (data != null)
        {
            string path = Application.persistentDataPath + "/saveData.dat";
            File.Delete(path);
        }
    }
}
