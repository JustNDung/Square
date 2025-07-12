using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool _isWalkable = true;
    
    public bool IsWalkable
    {
        get { return _isWalkable; }
        set { _isWalkable = value; }
    }
}
