using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{

    public static void SaveGame(string currentScene, bool isBossUnlocked, Vector3 position){
        BinaryFormatter formatter = new BinaryFormatter();
	    FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat"); 
	    SaveData data = new SaveData(currentScene, isBossUnlocked, position);


	    formatter.Serialize(file, data);
	    file.Close();
	    Debug.Log("Game data saved!");
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
            Debug.LogError("There is no save data!");
            return null;
        }
    }
}
