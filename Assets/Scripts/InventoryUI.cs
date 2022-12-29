using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryManager Inventory;
    public GameObject KeyUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Inventory.hasObject("Key") && !KeyUI.active)
        {
            KeyUI.SetActive(true);
        }
    }
}
