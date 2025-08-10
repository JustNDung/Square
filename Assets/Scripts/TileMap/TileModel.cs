using UnityEngine;
public class TileModel
{
    private TileType _type = TileType.None;
    private Vector3 _tilePos;
    
    public TileModel(Vector3 tilePos, TileType type)
    {
        _tilePos = tilePos;
        _type = type;
    }
    
    public Vector3 TilePos
    {
        get { return _tilePos; }
        set { _tilePos = value; }
    }
    
    public TileType Type
    {
        get { return _type; }
        set { _type = value; }
    }
    
}
