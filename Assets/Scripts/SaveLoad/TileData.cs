using UnityEngine;  
[System.Serializable]
public class TileData
{
    public int levelId; // ID của bản đồ
    public TileType type; // Loại ô (ví dụ: đất, nước, cỏ, v.v.)
    public float posX;
    public float posY;
    public float posZ; // Vị trí của ô trên bản đồ
}

