using UnityEngine;
using System.Collections.Generic;

// Ghi chú:
// MapState luu các visited tile với pos y = 0 nhưng character có pos y = 0.25f 
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
    
    // Lưu các tile khng thể đi được
    public HashSet<Vector3> UnwalkableTiles { get; private set; } = new HashSet<Vector3>();
    
    // Mỗi vị trí chỉ có một nhân vật
    public Dictionary<Vector3, CharacterController> CharacterAtPosition { get; private set; } = new Dictionary<Vector3, CharacterController>();
    
    // Mỗi nhân vật chỉ có một vị trí
    public Dictionary<CharacterController, Vector3> PositionOfCharacter { get; private set; } = new Dictionary<CharacterController, Vector3>();

    // Lưu thông tin khác nếu cần (ví dụ: tile đặc biệt, trạng thái thắng thua...)
    public bool IsGameOver { get; set; }

    // Khởi tạo
    public MapState(int width, int length, int distanceUnit, List<Tile> tiles, CharacterController characterController)
    {
        Width = width;
        Length = length;
        DistanceUnit = distanceUnit;
        Tiles = tiles;
        AddCharacter(characterController);
        
        Vector3 tileVisited = new Vector3(characterController.transform.position.x, 0, characterController.transform.position.z); // Tọa độ ô đã đi qua pos y = 0
        VisitTile(tileVisited); // Đánh dấu ô đã đi qua khi khởi tạo
    }

    // Kiểm tra ô có thể đi được không
    public bool CanMoveTo(Vector3 pos)
    {
        return IsInBounds(pos) && !VisitedTiles.Contains(pos) 
               && !UnwalkableTiles.Contains(pos);
    }   

    // Đánh dấu ô đã đi qua
    public void VisitTile(Vector3 pos)
    {
        VisitedTiles.Add(pos);
    }
    
    public void UnvisitTile(Vector3 pos)
    {
        VisitedTiles.Remove(pos);
    }

    // Kiểm tra tọa độ trong map
    public bool IsInBounds(Vector3 pos)
    {
        return pos.x >= 0 && pos.x <= (Width - 1) * DistanceUnit 
                          && pos.z >= 0 && pos.z <= (Length - 1) * DistanceUnit;
    }
    
    // Đánh dấu một ô là không thể đi qua
    public void AddUnwalkableTile(Vector3 pos)
    {
        if (!UnwalkableTiles.Contains(pos))
        {
            UnwalkableTiles.Add(pos);
        }
    }

    // Bỏ đánh dấu một ô là không thể đi
    public void RemoveUnwalkableTile(Vector3 pos)
    {
        if (UnwalkableTiles.Contains(pos))
        {
            UnwalkableTiles.Remove(pos);
        }
    }

    // Kiểm tra ô có phải là unwalkable không
    public bool IsUnwalkable(Vector3 pos)
    {
        return UnwalkableTiles.Contains(pos);
    }
    
    public void AddCharacter(CharacterController characterController)
    {
        Vector3 pos = characterController.transform.position;
        if (CharacterAtPosition.ContainsKey(pos))
        {
            UIHelpers.Instance.ShowPopUpUI($"Vị trí {pos} đã có nhân vật khác!");
            return;
        }

        if (PositionOfCharacter.ContainsKey(characterController))
        {
            UIHelpers.Instance.ShowPopUpUI($"Nhân vật {characterController.name} đã có trong map!");
            return;
        }
        
        CharacterAtPosition[pos] = characterController;
        PositionOfCharacter[characterController] = pos; 
        
        VisitTile(ToTilePosition(pos)); // Đánh dấu ô đã đi qua
    }
    
    public void ModifyCharacterPosition(CharacterController characterController, Vector3 newPos)
    {
        // Kiểm tra nhân vật đã có trong map chưa
        if (!PositionOfCharacter.TryGetValue(characterController, out var oldPos))
        {
            UIHelpers.Instance.ShowPopUpUI($"Nhân vật {characterController.name} chưa tồn tại trong map!");
            return;
        }

        // Kiểm tra xem vị trí mới đã có nhân vật khác chưa
        if (CharacterAtPosition.ContainsKey(newPos))
        {
            UIHelpers.Instance.ShowPopUpUI($"Vị trí {newPos} đã có nhân vật khác!");
            return;
        }
        
        UnvisitTile(ToTilePosition(oldPos)); // Bỏ đánh dấu ô cũ

        // Cập nhật map
        CharacterAtPosition.Remove(oldPos);                 // Xóa vị trí cũ
        CharacterAtPosition[newPos] = characterController;  // Gán vị trí mới
        PositionOfCharacter[characterController] = newPos;  // Cập nhật map vị trí nhân vật

        // Đánh dấu đã đi qua vị trí mới
        VisitTile(ToTilePosition(newPos)); // Đánh dấu ô mới đã đi qua
        characterController.transform.position = newPos; // Cập nhật vị trí của nhân vật
    }
    
    public void RemoveCharacter(CharacterController characterController)
    {
        if (!PositionOfCharacter.TryGetValue(characterController, out var pos))
            return;

        CharacterAtPosition.Remove(pos);
        PositionOfCharacter.Remove(characterController);
    }
    
    public bool HasCharacterAt(Vector3 pos)
    {
        if (CharacterAtPosition.ContainsKey(pos))
        {
            UIHelpers.Instance.ShowPopUpUI($"Vị trí {pos} đã có nhân vật khác!");
        }
        return CharacterAtPosition.ContainsKey(pos);
    }
    
    private Vector3 ToTilePosition(Vector3 pos)
    {
        return new Vector3(pos.x, 0, pos.z); // Chỉ lấy x và z, y = 0
    }

}