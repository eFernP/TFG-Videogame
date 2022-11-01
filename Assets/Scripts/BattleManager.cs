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


    private float panelSize = 0f;
    private int selectedPanel;

    private int canvasPadding = 10;

    private AudioClip testClip;

    private static int MAX_NUMBER_OF_PANELS = 4;
    private static int MAX_FRIENDSHIP_VALUE = 100;

    private Player playerScript;

    Audio AudioManager;
    BossSubtitles BossText;

    private int numberOfPanels;

    private bool hasBlockedButtons;

    private int currentPhase = 1;


    private Dictionary<int, HeroDialogue> HeroDialogues = new Dictionary<int, HeroDialogue>()
     {
            {1, new HeroDialogue(1, "No me hagas daño, ¡por favor!", 1, new int[] { 3 }, new int[]{}, 10, 10)},
            {2, new HeroDialogue(2, "Detente o lo lamentarás.", 2, new int[] { 4 }, new int[]{ }, -10, 10)},
            {3, new HeroDialogue(3, "He venido a buscar a alguien.", 3, new int[]{ },  new int[]{ }, 5, 10)}, //CHANGE 3 FOR REAL DIALOGUE
            {4, new HeroDialogue(4, "¿Sabes acaso quién soy?", 3, new int[]{ },  new int[]{ }, -5, 10)}
     };


    private Dictionary<int, BossDialogue> BossDialogues = new Dictionary<int, BossDialogue>()
     {
            {1, new BossDialogue(1, new Vocals[]{ new Vocals("testClip", "No debiste entrar aquí.") }) },
            {2, new BossDialogue(2, new Vocals[]{ new Vocals("testClip", "Eres débil."), new Vocals("testClip", "No supones ninguna amenaza para mí.") }) },
            {3, new BossDialogue(3, new Vocals[]{ new Vocals("testClip", "Allí fuera eres Irik el Lucero, el Esplendoroso,"), new Vocals("testClip", "el hijo predilecto de un reino que se desmorona."), new Vocals("testClip", "Aquí dentro no eres nadie.") }) }
     };



    private Dictionary<int, int> currentDialogues = new Dictionary<int, int>()
     {
            {1, 1},
            {2, 2},
     };


    private GameObject[] currentPanels = new GameObject[MAX_NUMBER_OF_PANELS];

    private int[] RoundValues = { 50, 75, 100 };
    private int RoundNumber = 1;

    public int GetCurrentPhase()
    {
        return currentPhase;
    }


    // Start is called before the first frame update
    void Start()
    {
        Audio.onFinishDialogue += onFinishBossDialogue;

        GameObject hero = GameObject.Find("Hero");
        AudioManager = hero.GetComponent<Audio>();
        playerScript = hero.GetComponent<Player>();
        BossText = GameObject.Find("Subtitle").GetComponent<BossSubtitles>();

        friendshipBar.SetMaxValue(MAX_FRIENDSHIP_VALUE);
        initPanels();

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
        panelSize = (Screen.width - canvasPadding * 2 - (canvasPadding * (numberOfPanels - 1))) / numberOfPanels;
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
        GameObject newPanel = Instantiate(PanelPrefab, new Vector3(0,0,0), Quaternion.identity, HeroCanvas.transform);
        newPanel.name = "Panel_"+ index;
        RectTransform rectTransform = newPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(panelSize, rectTransform.rect.height);

        RectTransform rectTransformText = newPanel.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransformText.sizeDelta = new Vector2((panelSize/100)*80, rectTransformText.rect.height);

        if (selectedPanel == index)
        {
            updatePanelSprite(newPanel, selectedPanelSprite);
        }

        if (hasBlockedButtons)
        {
            changePanelTransparency(newPanel, 0.5f);
        }

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
            Debug.Log("PANEL AFTER DESTROY??" + currentPanels[currentDialogueId.Key - 1]);
        }
    }

    void initPanels()
    {
        numberOfPanels = currentDialogues.Count;
        selectedPanel = 1;
        calculatePanelSize();

        foreach (KeyValuePair<int, int> currentDialogueId in currentDialogues)
        {

            instantiatePanel(currentDialogueId.Key);
            addDialogue(currentDialogueId.Key, currentDialogueId.Value);


        }
    }


    void onFinishBossDialogue()
    {
        int[] newDialogues = HeroDialogues[currentDialogues[selectedPanel]].NextHeroDialogues;
        int[] incompatibleDialogues = HeroDialogues[currentDialogues[selectedPanel]].IncompatibleDialogues;

        int dialoguesIndex = 0;
        for (int i = 1; i <= MAX_NUMBER_OF_PANELS; i++)
        {
            if (i == selectedPanel || (currentDialogues.ContainsKey(i) && incompatibleDialogues.Contains(currentDialogues[i])))
            {
                currentDialogues.Remove(i);
            }

            if (dialoguesIndex < newDialogues.Length && !currentDialogues.ContainsKey(i))
            {
                currentDialogues.Add(i, newDialogues[dialoguesIndex]);
                dialoguesIndex++;
                
            }

        }
        
        if(newDialogues.Length == 0)
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
        if (!hasBlockedButtons && playerScript.GetBreath() < 10f)
        {
            hasBlockedButtons = true;
            changePanelsTransparency(0.5f);
        }
        if (hasBlockedButtons && playerScript.GetBreath() > 10f)
        {
            hasBlockedButtons = false;
            changePanelsTransparency(1f);
        }
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

            if (bossDialogueId == -1)
            {
                Debug.Log("FINISH DIALOGUE");
            }
            else
            {
                AudioManager.StartDialogue(BossDialogues[bossDialogueId].Messages);
                BossText.setDialogue(BossDialogues[bossDialogueId].Messages);
            }

            friendshipBar.SetValue(friendshipBar.GetValue() + HeroDialogues[selectedDialogueId].Friendship);
            playerScript.DecreaseBreath(HeroDialogues[selectedDialogueId].Breath);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (currentPhase == 1)
        {
            ShowDialogues();
        }


        if(currentPhase == 2 && friendshipBar.GetValue() >= RoundValues[RoundNumber-1] )
        {
            if(RoundValues[RoundNumber - 1] < MAX_FRIENDSHIP_VALUE)
            {
                currentPhase = 1;
                RoundNumber++;
            }
            else
            {
                Debug.Log("FINISH COMBAT");
            }

        }

    }


    void onDisable()
    {
        Audio.onFinishDialogue += onFinishBossDialogue;
    }
}
