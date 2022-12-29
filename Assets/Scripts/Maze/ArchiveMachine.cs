using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveMachine : InteractiveObject
{
    public InformationPanel Panel;
    public GameObject Detector;
    public SpawnRandomEnviroment SceneManager;

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
        if(Inventory.hasObject("Key")){
            base.checkText();
        }

        if (base.hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            Panel.show("(En proceso)", "(En este archivo se explica cómo funciona la magia)");

            if (!SceneManager.hasBossRoom())
            {
                Detector.SetActive(true);
            }
        }
    }

}
