using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Task Card UI - Shows/hides PBL task cards on screen with T key
/// </summary>
public class TaskCardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject taskCardPanel;
    public Text taskTitleText;
    public Text taskDescriptionText;
    public Text roleText;
    public Text taskNumberText;
    public Image backgroundImage;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.T;
    public bool showOnStart = false;
    public float fadeSpeed = 5f;

    private CollaborativeSessionManager sessionManager;
    private CanvasGroup canvasGroup;
    private bool isVisible = false;

    private void Start()
    {
        sessionManager = CollaborativeSessionManager.Instance;

        // Create UI if not assigned
        if (taskCardPanel == null)
        {
            CreateTaskCardUI();
        }

        // Get or add CanvasGroup for fade effect
        canvasGroup = taskCardPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = taskCardPanel.AddComponent<CanvasGroup>();
        }

        // Set initial visibility
        isVisible = showOnStart;
        UpdateVisibility();
        UpdateTaskCard();
    }

    private void Update()
    {
        // Check if input field is focused (don't block keyboard shortcuts)
        bool isInputFieldFocused = false;
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (currentSelected != null && currentSelected.GetComponent<UnityEngine.UI.InputField>() != null)
            {
                isInputFieldFocused = true;
            }
        }

        // Toggle with T key (ignore if input field is focused)
        if (!isInputFieldFocused && Input.GetKeyDown(toggleKey))
        {
            ToggleTaskCard();
        }

        // Update task card content if visible
        if (isVisible)
        {
            UpdateTaskCard();
        }

        // Smooth fade in/out
        float targetAlpha = isVisible ? 1f : 0f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    public void ToggleTaskCard()
    {
        isVisible = !isVisible;
        UpdateVisibility();
    }

    public void ShowTaskCard()
    {
        isVisible = true;
        UpdateVisibility();
    }

    public void HideTaskCard()
    {
        isVisible = false;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (taskCardPanel != null)
        {
            taskCardPanel.SetActive(isVisible);
        }
    }

    private void UpdateTaskCard()
    {
        if (sessionManager == null) return;

        var currentRole = sessionManager.CurrentRole;
        var currentTask = sessionManager.CurrentTask;
        var currentTaskIndex = GetCurrentTaskIndex();
        var totalTasks = GetTotalTaskCount();

        // Update role text
        if (roleText != null)
        {
            roleText.text = $"Role: {currentRole.roleName}";
            roleText.color = currentRole.markerColor;
        }

        // Update task number
        if (taskNumberText != null)
        {
            taskNumberText.text = $"Task {currentTaskIndex + 1} / {totalTasks}";
        }

        // Update task title
        if (taskTitleText != null)
        {
            taskTitleText.text = $"PBL Task {currentTaskIndex + 1}";
        }

        // Update task description
        if (taskDescriptionText != null)
        {
            taskDescriptionText.text = currentTask.description;
        }

        // Update background color based on role
        if (backgroundImage != null)
        {
            Color bgColor = currentRole.markerColor;
            bgColor.a = 0.2f; // Semi-transparent
            backgroundImage.color = bgColor;
        }
    }

    private int GetCurrentTaskIndex()
    {
        if (sessionManager == null) return 0;
        return sessionManager.CurrentTaskIndex;
    }

    private int GetTotalTaskCount()
    {
        if (sessionManager == null) return 0;
        return sessionManager.TotalTaskCount;
    }

    private void CreateTaskCardUI()
    {
        // Find or create Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create Task Card Panel
        GameObject panel = new GameObject("TaskCardPanel");
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(600, 300);
        panelRect.anchoredPosition = new Vector2(0, 200); // Top center

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);

        // Add CanvasGroup for fade
        CanvasGroup cg = panel.AddComponent<CanvasGroup>();

        // Create Background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(panel.transform, false);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.5f, 1f, 0.2f);
        backgroundImage = bgImage;

        // Create Role Text
        GameObject roleObj = new GameObject("RoleText");
        roleObj.transform.SetParent(panel.transform, false);
        RectTransform roleRect = roleObj.AddComponent<RectTransform>();
        roleRect.anchorMin = new Vector2(0.05f, 0.85f);
        roleRect.anchorMax = new Vector2(0.95f, 0.95f);
        roleRect.sizeDelta = Vector2.zero;
        roleText = roleObj.AddComponent<Text>();
        roleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        roleText.fontSize = 20;
        roleText.fontStyle = FontStyle.Bold;
        roleText.color = Color.white;
        roleText.alignment = TextAnchor.UpperLeft;

        // Create Task Number Text
        GameObject taskNumObj = new GameObject("TaskNumberText");
        taskNumObj.transform.SetParent(panel.transform, false);
        RectTransform taskNumRect = taskNumObj.AddComponent<RectTransform>();
        taskNumRect.anchorMin = new Vector2(0.05f, 0.75f);
        taskNumRect.anchorMax = new Vector2(0.95f, 0.85f);
        taskNumRect.sizeDelta = Vector2.zero;
        taskNumberText = taskNumObj.AddComponent<Text>();
        taskNumberText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        taskNumberText.fontSize = 16;
        taskNumberText.color = Color.white;
        taskNumberText.alignment = TextAnchor.UpperLeft;

        // Create Task Title Text
        GameObject titleObj = new GameObject("TaskTitleText");
        titleObj.transform.SetParent(panel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.05f, 0.60f);
        titleRect.anchorMax = new Vector2(0.95f, 0.75f);
        titleRect.sizeDelta = Vector2.zero;
        taskTitleText = titleObj.AddComponent<Text>();
        taskTitleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        taskTitleText.fontSize = 22;
        taskTitleText.fontStyle = FontStyle.Bold;
        taskTitleText.color = Color.white;
        taskTitleText.alignment = TextAnchor.MiddleLeft;

        // Create Task Description Text
        GameObject descObj = new GameObject("TaskDescriptionText");
        descObj.transform.SetParent(panel.transform, false);
        RectTransform descRect = descObj.AddComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0.05f, 0.05f);
        descRect.anchorMax = new Vector2(0.95f, 0.60f);
        descRect.sizeDelta = Vector2.zero;
        taskDescriptionText = descObj.AddComponent<Text>();
        taskDescriptionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        taskDescriptionText.fontSize = 16;
        taskDescriptionText.color = Color.white;
        taskDescriptionText.alignment = TextAnchor.UpperLeft;
        taskDescriptionText.horizontalOverflow = HorizontalWrapMode.Wrap;
        taskDescriptionText.verticalOverflow = VerticalWrapMode.Overflow;

        taskCardPanel = panel;
        canvasGroup = cg;
    }
}

