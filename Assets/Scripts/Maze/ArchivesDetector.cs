using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchivesDetector : MonoBehaviour
{

    public SpawnRandomEnviroment SceneManager;
    public Subtitles SubtitleManager;
    public Audio AudioManager;
    private bool hasUnlockedRoom = false;

    public AudioClip unlockedRoomClip;

    void Start()
    {
        Subtitles.onFinishDialogue += onFinishDialogue;
    }

    void onFinishDialogue()
    {
        SceneManager.addBossRoom();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!hasUnlockedRoom && collider.gameObject.tag == "Player")
        {
            SubtitleManager.setDialogue(Constants.MazeDialogues[0]);
            hasUnlockedRoom = true;
            AudioManager.PlaySound(unlockedRoomClip);
        }
    }

    void OnDisable()
    {
        Subtitles.onFinishDialogue -= onFinishDialogue;
    }
}
