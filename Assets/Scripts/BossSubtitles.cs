using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossSubtitles : MonoBehaviour
{
    // Start is called before the first frame update

    TMP_Text subtitle;

    float WPM = 150;

    public delegate void FinishDialogue();
    public static event FinishDialogue onFinishDialogue;

    void Start()
    {
        subtitle = GetComponent<TMPro.TextMeshProUGUI>();
    }

    IEnumerator showSubtitles(Vocals[] vocals)
    {

        foreach (Vocals v in vocals)
        {
            subtitle.text = v.Text;
            Debug.Log(v.Text);
            string[] SplittedText = v.Text.Split(" ");
            float seconds = (float)SplittedText.Length / WPM * 60;
            yield return new WaitForSeconds(1f + seconds);
        }
        subtitle.text = null;
        onFinishDialogue();
    }


    public void setDialogue(Vocals[] vocals)
    {
        StartCoroutine(showSubtitles(vocals));
    }

}
