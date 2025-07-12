using UnityEngine;
using System.Collections.Generic;
public class MapState
{
    // Kích thước map
    public int Width { get; private set; }
    public int Length { get; private set; }
    public int DistanceUnit { get; private set; }

    // Danh sách tất cả tile trên map
    public List<Tile> Tiles { get; private set; }

    // Lưu các tile đã đi qua (để không đi lại)
    public HashSet<Vector3> VisitedTiles { get; private set; } = new HashSet<Vector3>();

    // Vị trí hiện tại của vật thể
    public Vector3 PlayerPosition { get; set; }

    // Lưu thông tin khác nếu cần (ví dụ: tile đặc biệt, trạng thái thắng thua...)
    public bool IsGameOver { get; set; }

    // Khởi tạo
    public MapState(int width, int length, int distanceUnit, List<Tile> tiles, Vector3 startPos)
    {
        Width = width;
        Length = length;
        DistanceUnit = distanceUnit;
        Tiles = tiles;
        PlayerPosition = startPos;
        VisitedTiles.Add(startPos); // đánh dấu ô đầu tiên là đã đi qua
    }

    // Kiểm tra ô có thể đi được không
    public bool CanMoveTo(Vector3 pos)
    {
        return IsInBounds(pos) && !VisitedTiles.Contains(pos);
    }

    // Đánh dấu ô đã đi qua
    public void VisitTile(Vector3 pos)
    {
        VisitedTiles.Add(pos);
        PlayerPosition = new Vector3(pos.x, 0.25f, pos.z); 
    }

    // Kiểm tra tọa độ trong map
    public bool IsInBounds(Vector3 pos)
    {
        return pos.x >= 0 && pos.x <= (Width - 1) * DistanceUnit 
                          && pos.z >= 0 && pos.z <= (Length - 1) * DistanceUnit;
    }
}