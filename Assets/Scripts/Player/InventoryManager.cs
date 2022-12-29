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


    public void addObject(string objectName){
        Inventory[objectName] = true;
        checkSpecialAction(objectName);
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
            addObject("Key");
        }
    }

}
