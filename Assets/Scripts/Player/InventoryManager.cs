using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private Player playerManager;
    private Audio audioManager;

    public AudioClip eyeClip;

    private Dictionary<string, bool> Inventory = new Dictionary<string, bool>()
    {
            {"Key", false},
            {"Eye", false},
    };


    void checkSpecialAction(string objectName){
        if(objectName == "Eye"){
            playerManager.changeEye();  //UNCOMMENT
            audioManager.PlaySound(eyeClip);
        }
    }

    public void addObjects(string[] names, bool enabledSpecialActions = true)
    {
        foreach (string name in names)
        {
            Inventory[name] = true;
            if (enabledSpecialActions)
            {
                checkSpecialAction(name);
            }
            
        }

    }

    public bool hasObject(string objectName){
        return Inventory[objectName];
    }

    void Start()
    {
        playerManager = this.GetComponent<Player>();
        audioManager = this.GetComponent<Audio>();

        SaveData saveData = SaveSystem.LoadGame();

        if (saveData != null && saveData.IsBossUnlocked)
        {
            addObjects(new string[] { "Key", "Eye" }, false);
        }

    }

}
