using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : InteractiveObject
{
    public string name;

    private InventoryManager Inventory;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Inventory = base.hero.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        base.checkText();

        if (base.hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            Inventory.addObject(name);
            Destroy(this.gameObject);
        }
    }
}
