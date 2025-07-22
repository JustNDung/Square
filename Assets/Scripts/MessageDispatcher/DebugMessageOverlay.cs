using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMessageOverlay : MonoBehaviour
{
    public static DebugMessageOverlay Instance;

    [SerializeField] private Text logText;
    [SerializeField] private int maxLogs = 20;

    private readonly Queue<string> _logs = new();

    private void Awake()
    {
        Instance = this;
        logText.text = "";
    }

    public void AddLog(string eventName, string data)
    {
        string logEntry = $"<b>{eventName}</b>: {data}";
        _logs.Enqueue(logEntry);

        if (_logs.Count > maxLogs)
            _logs.Dequeue();

        logText.text = string.Join("\n", _logs);
    }
}