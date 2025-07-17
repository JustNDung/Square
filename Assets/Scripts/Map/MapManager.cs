using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    private int _mapWidth;
    private int _mapLength;
    [SerializeField] private GameObject tilePrefab;
    private Transform _tileMapContainer;
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
    }

    public void SetupMap(MapData mapData)
    {
        if (mapData != null)
        {
            _mapWidth = mapData.width;
            _mapLength = mapData.length;
        }
        else
        {
            Debug.LogWarning("⚠️ Map data is null, using default settings.");
        }
        
        CreateMap();
        _mapState = new MapState(_mapWidth, _mapLength, _distanceUnit, _tiles, Vector3.zero);
    }

    public void SetupCharacters(CharacterData characterData)
    {
        GameObject character = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
        character.transform.position = characterData.position;
    }
    
    private void CreateMap()
    {
        _tileMapContainer = new GameObject("TileMapContainer").transform;
        for (int x = 0; x < _mapWidth * _distanceUnit; x += _distanceUnit)
        {
            for (int z = 0; z < _mapLength * _distanceUnit; z += _distanceUnit)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, _tileMapContainer);
                tile.transform.rotation = Quaternion.Euler(90f, 0, 0); 
                
                if (tile.TryGetComponent<Tile>(out Tile tileComponent))
                {
                    _tiles.Add(tileComponent);
                }
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