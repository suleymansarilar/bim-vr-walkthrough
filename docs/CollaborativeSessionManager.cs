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
        if (roles.Length > 1 && Input.GetKeyDown(switchRoleKey))
        {
            SwitchRole();
        }

        if (tasks.Length > 0)
        {
            if (Input.GetKeyDown(nextTaskKey))
            {
                AdvanceTask(1);
            }
            else if (Input.GetKeyDown(previousTaskKey))
            {
                AdvanceTask(-1);
            }
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
            hintLabel.text = $"Switch Role [{switchRoleKey}] • Next Task [{nextTaskKey}] • Previous Task [{previousTaskKey}]";
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

