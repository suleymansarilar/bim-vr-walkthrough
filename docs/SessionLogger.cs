using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Lightweight session logger that writes one JSON line per interaction.
/// Keeps things human-readable so you can quickly inspect logs after a walkthrough.
/// </summary>
public class SessionLogger : MonoBehaviour
{
    private static SessionLogger _instance;

    [Header("Log Settings")]
    [Tooltip("Leave empty to auto-generate a log file under persistentDataPath/SessionLogs.")]
    public string explicitFileName;

    public static SessionLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SessionLogger>();
                if (_instance == null)
                {
                    var go = new GameObject("SessionLogger");
                    _instance = go.AddComponent<SessionLogger>();
                }
            }

            return _instance;
        }
    }

    private string logDirectory;
    private string logPath;
    private bool hasInitialised;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Initialise();
    }

    private void Initialise()
    {
        if (hasInitialised) return;

        logDirectory = Path.Combine(Application.persistentDataPath, "SessionLogs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        logPath = string.IsNullOrWhiteSpace(explicitFileName)
            ? Path.Combine(logDirectory, $"session_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jsonl")
            : Path.Combine(logDirectory, explicitFileName);

        hasInitialised = true;
        LogEvent("session_started", "system", "Session logger initialised");
    }

    public void LogEvent(string eventType, string role, string detail)
    {
        LogEvent(eventType, role, detail, null);
    }

    public void LogEvent(string eventType, string role, string detail, Vector3? position)
    {
        if (!hasInitialised)
        {
            Initialise();
        }

        var entry = new SessionLogEntry
        {
            timestamp = DateTime.UtcNow.ToString("o"),
            eventType = eventType,
            role = role,
            detail = detail,
            px = position?.x,
            py = position?.y,
            pz = position?.z
        };

        string json = JsonUtility.ToJson(entry);
        File.AppendAllText(logPath, json + Environment.NewLine);
    }

    private struct SessionLogEntry
    {
        // ReSharper disable InconsistentNaming - fields intentionally lower case for compact JSON.
        public string timestamp;
        public string eventType;
        public string role;
        public string detail;
        public float? px;
        public float? py;
        public float? pz;
        // ReSharper restore InconsistentNaming
    }
}

