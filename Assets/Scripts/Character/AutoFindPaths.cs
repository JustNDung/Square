using UnityEngine;

public class AutoFindPaths : MonoBehaviour
{
    public void AutoPlay()
    {
        MapManager tempMManager = MapManager.Instance;
        tempMManager.FindPathsToWin.FindAllMovePathToWin();

        foreach (var pair in tempMManager.FindPathsToWin.WinPaths)
        {
            pair.Key.MoveCharacter(pair.Value);
        }
    }
}
