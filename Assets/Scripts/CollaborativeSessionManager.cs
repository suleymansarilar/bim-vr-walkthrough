using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minimal co-working layer: lets you swap between roles, cycle through small PBL-style tasks,
/// and broadcasts the current role to the logger + marker tool. No networking here – just a
/// lightweight way to stage collaborative walkthroughs or remote call sessions.
/// </summary>
public class CollaborativeSessionManager : MonoBehaviour
{
    private static CollaborativeSessionManager _instance;

    [System.Serializable]
    public class RoleDefinition
    {
        public string roleName = "Engineer";
        public Color markerColor = new Color(0.1f, 0.7f, 1f);
    }

    [System.Serializable]
    public class TaskDefinition
    {
        [TextArea]
        public string description = "Identify a potential safety risk in the main lobby.";
    }

    [Header("Roles")]
    public RoleDefinition[] roles =
    {
        new RoleDefinition { roleName = "Site Engineer", markerColor = new Color(0.1f, 0.7f, 1f) },
        new RoleDefinition { roleName = "Safety Officer", markerColor = new Color(1f, 0.5f, 0.2f) }
    };

    [Header("Tasks")]
    public TaskDefinition[] tasks =
    {
        new TaskDefinition
        {
            description = "Task 1: Walk the main circulation path and mark any chokepoints that would slow emergency egress."
        },
        new TaskDefinition
        {
            description = "Task 2: Discuss visibility around the atrium staircase. Place a marker where additional signage may help."
        },
        new TaskDefinition
        {
            description = "Task 3: Agree on a safe route for material delivery. Draw the route and note why it was chosen."
        }
    };

    [Header("UI Hooks (Optional)")]
    public Text roleLabel;
    public Text taskLabel;
    public Text hintLabel;

    [Header("Shortcuts")]
    public KeyCode switchRoleKey = KeyCode.Tab;
    public KeyCode nextTaskKey = KeyCode.Space;
    public KeyCode previousTaskKey = KeyCode.Backspace;

    private int currentRoleIndex;
    private int currentTaskIndex;
    private SessionLogger logger;

    // Public accessor for task index (for TaskCardUI)
    public int CurrentTaskIndex => currentTaskIndex;
    public int TotalTaskCount => tasks != null ? tasks.Length : 0;

    public static CollaborativeSessionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CollaborativeSessionManager>();
            }

            return _instance;
        }
    }

    public RoleDefinition CurrentRole => roles != null && roles.Length > 0
        ? roles[currentRoleIndex]
        : new RoleDefinition();

    public TaskDefinition CurrentTask => tasks != null && tasks.Length > 0
        ? tasks[currentTaskIndex]
        : new TaskDefinition();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        logger = SessionLogger.Instance;
        LogRolesAndTasks();
        UpdateUI();
    }

    private void Update()
    {
        // Check if UI input field is focused (but allow keyboard shortcuts)
        bool isInputFieldFocused = false;
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (currentSelected != null && currentSelected.GetComponent<UnityEngine.UI.InputField>() != null)
            {
                isInputFieldFocused = true;
            }
        }

        // Don't process keyboard shortcuts if input field is focused
        if (isInputFieldFocused)
        {
            return;
        }

        // Tab key might be captured by Unity Editor in Play Mode, so we check both
        if (roles.Length > 1 && (Input.GetKeyDown(switchRoleKey) || Input.GetKeyDown(KeyCode.Tab)))
        {
            SwitchRole();
        }

        if (tasks.Length > 0)
        {
            // Next task: Space
            if (Input.GetKeyDown(nextTaskKey) || Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceTask(1);
            }
            // Previous task: Backspace (check both key and KeyCode)
            else if (Input.GetKeyDown(previousTaskKey) || Input.GetKeyDown(KeyCode.Backspace))
            {
                AdvanceTask(-1);
            }
        }

        // Reset task: Q (stop/reset current task)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetCurrentTask();
        }
    }

    public void SwitchRole()
    {
        currentRoleIndex = (currentRoleIndex + 1) % roles.Length;
        logger.LogEvent("role_switched", CurrentRole.roleName, $"Switched to {CurrentRole.roleName}");
        UpdateUI();
    }

    public void AdvanceTask(int direction)
    {
        if (tasks.Length == 0) return;

        currentTaskIndex = (currentTaskIndex + direction) % tasks.Length;
        if (currentTaskIndex < 0)
        {
            currentTaskIndex = tasks.Length - 1;
        }

        logger.LogEvent("task_selected", CurrentRole.roleName, CurrentTask.description);
        UpdateUI();
    }

    public void MarkTaskCompleted()
    {
        if (tasks.Length == 0) return;
        logger.LogEvent("task_completed", CurrentRole.roleName, CurrentTask.description);
    }

    public void ResetCurrentTask()
    {
        if (tasks.Length == 0) return;
        logger.LogEvent("task_reset", CurrentRole.roleName, $"Reset task: {CurrentTask.description}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (roleLabel != null)
        {
            roleLabel.text = $"Role: {CurrentRole.roleName}";
        }

        if (taskLabel != null)
        {
            taskLabel.text = CurrentTask.description;
        }

        if (hintLabel != null)
        {
            hintLabel.text = $"Tab: Switch Role • Space: Next Task • Backspace: Previous Task • Q: Reset Task";
        }
    }

    private void LogRolesAndTasks()
    {
        if (logger == null) return;

        var roleNames = new List<string>();
        foreach (var role in roles)
        {
            roleNames.Add(role.roleName);
        }

        var taskSummaries = new List<string>();
        foreach (var task in tasks)
        {
            taskSummaries.Add(task.description);
        }

        logger.LogEvent("session_roles_initialised", "system", string.Join(" | ", roleNames));
        logger.LogEvent("session_tasks_initialised", "system", string.Join(" | ", taskSummaries));
    }
}

