using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningMessage : MonoBehaviour
{
    TMP_Text message;

    string BREATH_WARNING_TEXT = "Insuficiente aliento";

    // Start is called before the first frame update
    void Start()
    {
        message = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public IEnumerator ShowMessage()
    {
        if(message.text == "")
        {
            message.text = BREATH_WARNING_TEXT;
            yield return new WaitForSeconds(1);
            message.text = "";
        }
    }
}
