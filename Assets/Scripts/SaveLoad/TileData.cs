using UnityEngine;  
[System.Serializable]
public class TileData
{
    public Vector3 position; // Vị trí của tile
    public bool isWalkable; // Có thể đi qua hay không

    // Constructor để khởi tạo dữ liệu tile
    public TileData(Vector3 position, bool isWalkable)
    {
        this.position = position;
        this.isWalkable = isWalkable;
    }

}

