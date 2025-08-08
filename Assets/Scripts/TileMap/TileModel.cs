using UnityEngine;
public class TileModel
{
    private TileType _tileType = TileType.None;
    private Vector3 _tilePos;
    
    public TileModel(Vector3 tilePos, TileType tileType)
    {
        _tilePos = tilePos;
        _tileType = tileType;
    }
    
    public Vector3 TilePos
    {
        get { return _tilePos; }
        set { _tilePos = value; }
    }
    
    public TileType TileType
    {
        get { return _tileType; }
        set { _tileType = value; }
    }
    
}
