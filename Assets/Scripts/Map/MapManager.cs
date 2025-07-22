using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    private int _mapWidth;
    private int _mapLength;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileMapContainer;
    [SerializeField] private Transform obstacleContainer;
    [SerializeField] private Vector3 defaultCharacterPosition = new Vector3(0, 0.25f, 0);
    private const int _distanceUnit = 2;
    private List<Tile> _tiles = new List<Tile>();
    
    [Header("Character Settings")]
    [SerializeField] private GameObject characterPrefab;
    
    public static MapManager Instance { get; private set; }

    private MapState _mapState;

    private void Awake()
    {
        // Nếu đã có một instance khác, thì hủy object hiện tại
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Gán instance và đánh dấu không bị hủy khi load scene khác
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GenerateMap(5, 5); // Tạo map mặc định khi khởi tạo
    }

    public void SetupMapWithData(MapData mapData)
    {
        if (mapData != null)
        {
            _mapWidth = Mathf.Max(1, mapData.width); // đảm bảo kích thước >= 1
            _mapLength = Mathf.Max(1, mapData.length);
        }
        else
        {
            Debug.LogWarning("⚠️ Map data is null, using default settings.");
        }
        
        CreateMap();
        _mapState = new MapState(_mapWidth, _mapLength, _distanceUnit, _tiles, Vector3.zero);
    }
    
    public void GenerateMap(int width, int length)
    {
        _mapWidth = Mathf.Max(1, width); // đảm bảo kích thước >= 1
        _mapLength = Mathf.Max(1, length);
        
        CreateMap();
        GenerateCharacter();
        _mapState = new MapState(_mapWidth, _mapLength, _distanceUnit, _tiles, Vector3.zero);
    }

    private void GenerateCharacter()
    {
        GameObject character = Instantiate(characterPrefab, defaultCharacterPosition, Quaternion.identity);
        character.transform.SetParent(transform);
    }
    
    private void CreateMap()
    {
        ClearMap();
        
        // Create tiles with mapWidth and mapLength
        for (int x = 0; x < _mapWidth * _distanceUnit; x += _distanceUnit)
        {
            for (int z = 0; z < _mapLength * _distanceUnit; z += _distanceUnit)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, tileMapContainer);
                tile.transform.rotation = Quaternion.Euler(90f, 0, 0); 
                
                if (tile.TryGetComponent<Tile>(out Tile tileComponent))
                {
                    _tiles.Add(tileComponent);
                }
            }
        }
        
    }

    private void ClearMap()
    {
        for (int i = 0; i < tileMapContainer.childCount; i++)
        {
            Destroy(tileMapContainer.GetChild(i).gameObject); // Xóa các tile cũ nếu có
        }
        
        for (int i = 0; i < obstacleContainer.childCount; i++)
        {
            Destroy(obstacleContainer.GetChild(i).gameObject); // Xóa các obstacle cũ nếu có
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "TileMapContainer" && transform.GetChild(i).name != "ObstacleContainer")
            {
                Destroy(transform.GetChild(i).gameObject); // Xóa các object khác nếu có
            }
        }
    }

    #region Getters and Setters
    
    public int MapWidth
    {
        get => _mapWidth;
        set => _mapWidth = Mathf.Max(1, value); // đảm bảo >= 1C
    }

    public int MapLength
    {
        get => _mapLength;
        set => _mapLength = Mathf.Max(1, value);
    }
    
    public int DistanceUnit => _distanceUnit; // chỉ getter vì là hằng số
    
    public List<Tile> Tiles
    {
        get => _tiles;
        set => _tiles = value ?? new List<Tile>(); // tránh gán null
    }
    
    public MapState MapState
    {
        get => _mapState;
        set => _mapState = value;
    }
    
    #endregion
   
}