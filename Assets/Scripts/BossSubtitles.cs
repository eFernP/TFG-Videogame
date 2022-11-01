using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossSubtitles : MonoBehaviour
{
    // Start is called before the first frame update

    TMP_Text subtitle; 

    void Start()
    {
        subtitle = GetComponent<TMPro.TextMeshProUGUI>();
    }

    IEnumerator showSubtitles(Vocals[] vocals)
    {

        foreach (Vocals v in vocals)
        {
            subtitle.text = v.Text;
            yield return new WaitForSeconds(1);
        }
        subtitle.text = null;
    }


    public void setDialogue(Vocals[] vocals)
    {
        StartCoroutine(showSubtitles(vocals));
    }

}
