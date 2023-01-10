using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : InteractiveObject
{
    public string[] names;

    private InventoryManager Inventory;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Inventory = base.hero.GetComponent<InventoryManager>();
        SaveData saveData = SaveSystem.LoadGame();

        if((saveData != null && saveData.IsBossUnlocked) || Inventory.hasObject("Key"))
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.checkText();

        if (base.hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            Inventory.addObjects(names);
            Destroy(this.gameObject);
        }
    }
}
