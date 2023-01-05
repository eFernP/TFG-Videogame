using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Subtitles : MonoBehaviour
{

    TMP_Text subtitle;

    float WPM = 200; //words for minute

    public delegate void FinishDialogue();
    public static event FinishDialogue onFinishDialogue;

    // Start is called before the first frame update
    void Start()
    {
        subtitle = GetComponent<TMPro.TextMeshProUGUI>();
    }

    IEnumerator showSubtitles(Vocals[] vocals)
    {

        foreach (Vocals v in vocals)
        {
            subtitle.text = v.Text;
            string[] SplittedText = v.Text.Split(" ");
            float seconds = (float)SplittedText.Length / WPM * 60;
            yield return new WaitForSeconds(0.5f);
        }
        subtitle.text = null;
        onFinishDialogue();
    }


    public void setDialogue(Vocals[] vocals)
    {
        StartCoroutine(showSubtitles(vocals));
    }

}
