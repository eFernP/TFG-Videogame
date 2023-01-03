using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public string CurrentScene;
    public bool IsBossUnlocked;
    public float[] Position; 

    public SaveData(string currentScene, Vector3 position, bool isBossUnlocked)
    {
        
        Position = new float[3];

        Position[0] = position.x;
        Position[1] = position.y;
        Position[2] = position.z;

        IsBossUnlocked = isBossUnlocked;
        CurrentScene = currentScene;
    }
}
