using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject infoPanel;     
    public Text titleText;           
    public Text infoText;            

    [Header("Demo Info")]
    public string demoTitle = "BIM VR Walkthrough Demo";
    public string[] demoInfo = {
        "BIM Model: Architectural Design",
        "Technology: Unity + OpenXR",
        "Features: First Person Navigation",
        "Purpose: Construction Safety Training"
    };

    private int currentInfoIndex = 0;
    private CanvasGroup panelGroup;

    void Awake()
    {
        if (infoPanel != null)
        {
            panelGroup = infoPanel.GetComponent<CanvasGroup>();
            if (panelGroup == null)
            {
                panelGroup = infoPanel.AddComponent<CanvasGroup>();
            }
        }
    }

    void Start()
    {
        
        SetPanelVisible(true);
        UpdateInfo();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.T))
        {
            TogglePanel();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextInfo();
        }
    }

    private void TogglePanel()
    {
        if (panelGroup == null) return;
        bool visible = panelGroup.alpha > 0.5f;
        SetPanelVisible(!visible);
    }

    private void SetPanelVisible(bool visible)
    {
        if (panelGroup == null) return;
        panelGroup.alpha = visible ? 1f : 0f;
        panelGroup.interactable = visible;
        panelGroup.blocksRaycasts = visible;
        
    }

    private void NextInfo()
    {
        currentInfoIndex = (currentInfoIndex + 1) % demoInfo.Length;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (titleText != null) titleText.text = demoTitle;
        if (infoText != null) infoText.text = demoInfo[currentInfoIndex];
    }
}