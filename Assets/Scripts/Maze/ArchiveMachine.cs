using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveMachine : InteractiveObject
{
    public InformationPanel Panel;
    public SpawnRandomEnviroment SceneManager;
    public Subtitles SubtitleManager;

    private InventoryManager Inventory;
    private bool hasUnlockedRoom = false;
        
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Inventory = base.hero.GetComponent<InventoryManager>();
        Subtitles.onFinishDialogue += onFinishDialogue;
    }


    void onFinishDialogue(){
        SceneManager.addBossRoom();
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
            if(!hasUnlockedRoom){
                SubtitleManager.setDialogue(Constants.MazeDialogues[0]);
                hasUnlockedRoom = true;
            }
            
            
        }
    }

    void OnDisable()
    {
        Subtitles.onFinishDialogue -= onFinishDialogue;
    }
}
