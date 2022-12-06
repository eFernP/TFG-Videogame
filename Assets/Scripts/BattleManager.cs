using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor;




public class BattleManager : MonoBehaviour
{

    public GameObject PanelPrefab;
    public Sprite selectedPanelSprite;
    public Sprite defaultPanelSprite;
    public GameObject HeroCanvas;
    public SliderBar friendshipBar;
    public GameObject FinishMenuUI;

    private float panelSize = 0f;
    private int selectedPanel;

    private int canvasPadding = 10;

    private AudioClip testClip;

    private static int MAX_NUMBER_OF_PANELS = 4;
    private static int MAX_FRIENDSHIP_VALUE = 100;
    private static int MAX_ROUND_NUMBER = 2;

    private BreathManager playerBreathManager;

    Audio AudioManager;
    BossSubtitles BossText;

    private int numberOfPanels;

    private bool hasBlockedButtons;

    private int currentPhase;

    private Dictionary<int, HeroDialogue> HeroDialogues;
    private Dictionary<int, BossDialogue> BossDialogues;

    private bool hasBattleStarted;

    private Dictionary<int, int> currentDialogues;

    private GameObject[] currentPanels;

    private int[] RoundValues = {60, 100}; //60 is the sum of the max points the player can have in the first phase 1 plus 10

    private int RoundNumber = 1;

    private bool isDialogueActive;

    private float SCREEN_WIDTH_REFERENCE = 1920;

    public int GetCurrentPhase()
    {
        return currentPhase;
    }


    // Start is called before the first frame update
    void Start()
    {

        Physics.IgnoreLayerCollision(3, 6);

        BossSubtitles.onFinishDialogue += onFinishBossDialogue;

        GameObject hero = GameObject.Find("Hero");
        AudioManager = hero.GetComponent<Audio>();
        playerBreathManager = hero.GetComponent<BreathManager>();
        BossText = GameObject.Find("Subtitle").GetComponent<BossSubtitles>();

        friendshipBar.SetMaxValue(MAX_FRIENDSHIP_VALUE);
        currentPanels = new GameObject[MAX_NUMBER_OF_PANELS];
        currentPhase = 0;
        HeroDialogues = Constants.HeroDialogues[0];
        BossDialogues = Constants.BossDialogues[0];

        currentDialogues = new Dictionary<int, int>()
        {
            {1, 1},
            {2, 2},
        };

        hasBattleStarted = false;
        isDialogueActive = false;

        //instantiatePanel(3);
        //addDialogue(3, 3);
        //instantiatePanel(4);
        //addDialogue(4, 4);
        //RectTransform rectTransform = panel1.GetComponent<RectTransform>();

        //width = 10;
        //rectTransform.sizeDelta = new Vector2(0, rectTransform.rect.height); 
        //Debug.Log(rectTransform.rect.width);
    }



    void addDialogue(int panelNumber, int dialogueNumber)
    {
        //GameObject panel = GameObject.Find("Panel_" + panelNumber);
        GameObject panel = currentPanels[panelNumber - 1];
        TMP_Text dialogue = panel.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        dialogue.text = HeroDialogues[dialogueNumber].Text;

    }

    void calculatePanelSize()
    {
        panelSize = (SCREEN_WIDTH_REFERENCE - canvasPadding * 2 - (canvasPadding * (numberOfPanels - 1))) / numberOfPanels;
    }


    void updatePanelSprite(GameObject panel, Sprite newSprite)
    {
        Image panelImage = panel.GetComponent<Image>();
        panelImage.sprite = newSprite;
    }


    void updateSelectedPanelSprite(int newSelectedPanel)
    {
        
        //GameObject oldPanel = GameObject.Find("Panel_" + selectedPanel);
        GameObject oldPanel = currentPanels[selectedPanel - 1];
        updatePanelSprite(oldPanel, defaultPanelSprite);
        //GameObject panel = GameObject.Find("Panel_" + newSelectedPanel);
        GameObject panel = currentPanels[newSelectedPanel-1];
        updatePanelSprite(panel, selectedPanelSprite);
        selectedPanel = newSelectedPanel;
    }


    void instantiatePanel(int index)
    {
        Debug.Log("INSTANTIATE"+index);
        Debug.Log(PanelPrefab);
        GameObject newPanel = Instantiate(PanelPrefab, new Vector3(0,0,0), Quaternion.identity, HeroCanvas.transform);
        Debug.Log(newPanel);

        newPanel.name = "Panel_"+ index;
        RectTransform rectTransform = newPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(panelSize, rectTransform.rect.height);

        RectTransform rectTransformText = newPanel.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransformText.sizeDelta = new Vector2((panelSize/100)*80, rectTransformText.rect.height);
        Debug.Log("INSTANTIATE 2");
        if (selectedPanel == index)
        {
            updatePanelSprite(newPanel, selectedPanelSprite);
        }

        if (hasBlockedButtons)
        {
            changePanelTransparency(newPanel, 0.5f);
        }
        Debug.Log("instance" + index);
        currentPanels[index - 1] = newPanel;

    }


    void removePanels()
    {
        foreach (KeyValuePair<int, int> currentDialogueId in currentDialogues)
        {
            //GameObject panel = GameObject.Find("Panel_" + currentDialogueId.Key);
            GameObject panel = currentPanels[currentDialogueId.Key - 1];
            currentPanels[currentDialogueId.Key - 1] = null;
            Destroy(panel);
            Debug.Log("PANEL AFTER DESTROY??");
        }
    }

    void initPanels()
    {
        numberOfPanels = currentDialogues.Count;
        selectedPanel = 1;
        calculatePanelSize();


        Debug.Log("INIT");
        foreach (KeyValuePair<int, int> currentDialogueId in currentDialogues)
        {
            Debug.Log("PAIRS "+currentDialogueId.Key + " " + currentDialogueId.Value);
            instantiatePanel(currentDialogueId.Key);
            addDialogue(currentDialogueId.Key, currentDialogueId.Value);


        }
    }


    void onFinishBossDialogue()
    {
        if(currentPhase == -1)
        {
            StartCoroutine(FinishGame());
            return;
        }

        int[] newDialogues;
        int[] incompatibleDialogues;

        if (selectedPanel == -1)
        {
            newDialogues = new int[] {1,2};
            incompatibleDialogues = new int[] { };
        }
        else
        {

            newDialogues = HeroDialogues[currentDialogues[selectedPanel]].NextHeroDialogues;
            incompatibleDialogues = HeroDialogues[currentDialogues[selectedPanel]].IncompatibleDialogues;
        }


        List<int> remainingDialogues = new List<int>();

        bool removeAll = incompatibleDialogues.Length > 0 && incompatibleDialogues[0] == -1;

        int panelsIndex = 1;

        foreach (KeyValuePair<int, int> dialogue in currentDialogues)
        {
            if (!removeAll && dialogue.Key != selectedPanel && !incompatibleDialogues.Contains(dialogue.Value))
            {
                remainingDialogues.Add(dialogue.Value);
            }
        }

        currentDialogues.Clear();

        for (int i = 0; i < newDialogues.Length; i++)
        {
            currentDialogues.Add(panelsIndex, newDialogues[i]);
            panelsIndex++;
        }
        foreach (int dialogue in remainingDialogues)
        {
            if (!currentDialogues.ContainsValue(dialogue))
            {
                currentDialogues.Add(panelsIndex, dialogue);
                panelsIndex++;
            }
        }

        isDialogueActive = false;

        if (currentDialogues.Count == 0)
        {
            currentPhase = 2;
        }
        else
        {
            initPanels();
        }

    }

    void changePanelTransparency(GameObject panel, float value)
    {
        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, value);
        TMP_Text panelText = panel.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        panelText.color = new Color(panelText.color.r, panelText.color.g, panelText.color.b, value);
    }

    void changePanelsTransparency(float value)
    {
        foreach (KeyValuePair<int, int> currentDialogueId in currentDialogues)
        {
            //GameObject panel = GameObject.Find("Panel_" + currentDialogueId.Key);
            GameObject panel = currentPanels[currentDialogueId.Key-1];

            if(panel)
            {
                changePanelTransparency(panel, value);
            }

        }
    }


    void checkBlockedButtons()
    {
        if (!hasBlockedButtons && playerBreathManager.GetBreath() < 10f)
        {
            hasBlockedButtons = true;
            changePanelsTransparency(0.5f);
        }
        if (hasBlockedButtons && playerBreathManager.GetBreath() > 10f)
        {
            hasBlockedButtons = false;
            changePanelsTransparency(1f);
        }
    }

    void startBossDialogue(Vocals[] vocals)
    {
        //AudioManager.StartDialogue(vocals); TODO: Enable when boss clips are done
        BossText.setDialogue(vocals);
        isDialogueActive = true;
    }


    void ShowDialogues()
    {
        checkBlockedButtons();

        if (Input.GetKeyUp(KeyCode.LeftArrow) && selectedPanel > 1)
        {
            updateSelectedPanelSprite(selectedPanel - 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && selectedPanel < numberOfPanels)
        {
            updateSelectedPanelSprite(selectedPanel + 1);
        }


        if (Input.GetKeyUp(KeyCode.Return) && !hasBlockedButtons)
        {
            removePanels();
            int selectedDialogueId = currentDialogues[selectedPanel];
            int bossDialogueId = HeroDialogues[selectedDialogueId].BossAnswer;


            startBossDialogue(BossDialogues[bossDialogueId].Messages);
            

            friendshipBar.SetValue(friendshipBar.GetValue() + HeroDialogues[selectedDialogueId].Friendship);
            playerBreathManager.DecreaseBreath(HeroDialogues[selectedDialogueId].Breath);

        }
    }


    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1.5f);
        FinishMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SELECTED PANEL VALUE" + selectedPanel);
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (currentPhase == 1 &&!isDialogueActive)
        {
            ShowDialogues();
        }

        if(currentPhase == 2 && friendshipBar.GetValue() >= RoundValues[RoundNumber-1] )
        {
            if(RoundNumber < MAX_ROUND_NUMBER)
            {
                    currentPhase = 1;
                    BossDialogues = Constants.BossDialogues[RoundNumber];
                    HeroDialogues = Constants.HeroDialogues[RoundNumber];
                    RoundNumber++;
                    selectedPanel = -1;
                    startBossDialogue(BossDialogues[1].Messages);
              
            }
            else
            {
                Debug.Log("FINISH COMBAT");
                currentPhase = -1;
                startBossDialogue(Constants.LastBattleDialogues[0]);
            }

        }

    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.name == "Hero" && !hasBattleStarted)
        {
            currentPhase = 1;
            hasBattleStarted = true;
            Debug.Log("INIT THE PANELS");
            initPanels();
        }
    }

    void OnDisable()
    {
        Debug.Log("DISABLE");
        BossSubtitles.onFinishDialogue -= onFinishBossDialogue;
    }
}
