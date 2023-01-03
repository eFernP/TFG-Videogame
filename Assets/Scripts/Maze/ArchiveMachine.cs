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
            Panel.show("Sección 12 del plan de emergencia de la instalación K903", "Por motivos de seguridad, todos los elementos presentes en la instalación albergan una modificación en su tejido para reaccionar físicamente a una determinada orden gesticular.\r\n\r\nEl ejecutor de la orden debe adoptar la postura que se muestra a continuación y pronunciar el nombre oculto de dicho elemento. \r\n\r\nEl objeto elegido será propulsado para ser alejado de la posición del ejecutor. Se recomienda usar esta orden solo en caso de vías obstaculizadas o averías.\r\n");

            if (!SceneManager.hasBossRoom())
            {
                Detector.SetActive(true);
            }
        }
    }

}
