using System;
using System.Collections.Generic;
using UnityEngine;

public static class MessageDispatcher
{
    private static readonly Dictionary<string, List<Action<object>>> _eventTable = new();

    // Debugging toggle
    public static bool EnableDebugLog = true;

    public static void Subscribe(string eventName, Action<object> callback)
    {
        if (!_eventTable.ContainsKey(eventName))
        {
            _eventTable[eventName] = new List<Action<object>>();
        }

        _eventTable[eventName].Add(callback);

        if (EnableDebugLog)
            Debug.Log($"[MessageDispatcher] Subscribed to <b>{eventName}</b> ➜ {callback.Method.DeclaringType}.{callback.Method.Name}");
    }

    public static void Unsubscribe(string eventName, Action<object> callback)
    {
        if (_eventTable.TryGetValue(eventName, out var callbackList))
        {
            callbackList.Remove(callback);
            if (callbackList.Count == 0)
                _eventTable.Remove(eventName);
        }

        if (EnableDebugLog)
            Debug.Log($"[MessageDispatcher] Unsubscribed from <b>{eventName}</b> ➜ {callback.Method.DeclaringType}.{callback.Method.Name}");
    }

    public static void Send(string eventName, object data = null)
    {
        if (EnableDebugLog)
        {
            string message;

            if (data == null)
            {
                message = "null";
            }
            else if (data.GetType().IsSerializable)
            {
                try
                {
                    message = JsonUtility.ToJson(data);
                }
                catch
                {
                    message = data.ToString();
                }
            }
            else
            {
                message = data.ToString();
            }

            Debug.Log($"<color=cyan>[MessageDispatcher] Fired:</color> <b>{eventName}</b> ➜ Data: <i>{message}</i>");
            DebugMessageOverlay.Instance?.AddLog(eventName, message);
        }

        if (_eventTable.TryGetValue(eventName, out var callbackList))
        {
            foreach (var callback in callbackList)
            {
                callback?.Invoke(data);
            }
        }
    }


    public static void Clear()
    {
        _eventTable.Clear();
        if (EnableDebugLog)
            Debug.Log("<color=red>[MessageDispatcher] All listeners cleared.</color>");
    }
}