using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var runnerGO = new GameObject("CoroutineRunner");
                Object.DontDestroyOnLoad(runnerGO);
                _instance = runnerGO.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }
}