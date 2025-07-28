using UnityEngine;  
[System.Serializable]
public class TileData
{
    public int levelId; // ID của bản đồ
    public bool isWalkable; // Có thể đi qua hay không
    public float posX;
    public float posY;
    public float posZ; // Vị trí của ô trên bản đồ
}

