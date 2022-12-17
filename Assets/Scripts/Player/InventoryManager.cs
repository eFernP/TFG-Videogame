using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, bool> Inventory = new Dictionary<string, bool>()
    {
            {"Key", false},
    };

    public void addObject(string objectName){
        Inventory[objectName] = true;
    }

    public bool hasObject(string objectName){
        return Inventory[objectName];
    }


    // void Update(){
    //     Debug.Log(Inventory["Key"]);
    // }

}
