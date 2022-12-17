using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationPanel : MonoBehaviour
{
    TMP_Text Title;
    TMP_Text Text;

    public GameObject[] otherUIObjects;
    public GameObject Panel;

    // Start is called before the first frame update
    void Start()
    {
        Title = Panel.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Text = Panel.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    void togglePanel(bool value)
    {
        Panel.SetActive(value);
    }

    void hideOtherUIObjects()
    {
        foreach(GameObject ui in otherUIObjects){
            Debug.Log("disactivate" + ui.name);
            ui.SetActive(false);
        }
    }

    public void show(string title, string text)
    {
        togglePanel(true);
        Title.text = title;
        Text.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("HIDE"+Panel);
            togglePanel(false);
        }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     hideOtherUIObjects();
        //     show("HOLA", "testing testing");
        // }
    }
}
