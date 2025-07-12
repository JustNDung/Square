using System;
using System.Collections.Generic;

public static class MessageDispatcher
{
    private static Dictionary<GameEvent, Action<object>> _eventTable = new();

    public static void Subscribe(GameEvent eventType, Action<object> callback)
    {
        if (!_eventTable.ContainsKey(eventType))
            _eventTable[eventType] = delegate { };

        _eventTable[eventType] += callback;
    }

    public static void Unsubscribe(GameEvent eventType, Action<object> callback)
    {
        if (_eventTable.ContainsKey(eventType))
            _eventTable[eventType] -= callback;
    }

    public static void Dispatch(GameEvent eventType, object data = null)
    {
        if (_eventTable.TryGetValue(eventType, out var action))
            action?.Invoke(data);
    }

    public static void Clear()
    {
        _eventTable.Clear();
    }
}