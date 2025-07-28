using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    private int _mapWidth = 5; // Default width of the map
    private int _mapLength = 5; // Default length of the map
    private const int _distanceUnit = 2;
    private List<Tile> _tiles = new List<Tile>();
    private MapEditor _mapEditor;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform tileMapContainer;
    [SerializeField] private Transform obstacleContainer;
    [SerializeField] private Transform characterContainer;
    [SerializeField] private Transform characterBodyContainer;
    [SerializeField] private Vector3 defaultCharacterPosition = new Vector3(0, 0.25f, 0);
    
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
        _mapEditor = GetComponent<MapEditor>();
    }

    private void Start()
    {
        GenerateBasicMapForEditor();
    }
    
    public void GenerateBasicMapForEditor()
    {
        CreateBasicMapForEditor();
        _mapState = null;
        GenerateCharacter(defaultCharacterPosition);        
    }

    public void GenerateMapFromData(GameLevelData data)
    {
        ClearMap();
        _mapState = null;
        
        // For map
        MapData mapData = data.map;
        _mapWidth = mapData.width;
        _mapLength = mapData.length;
        
        // For characters
        List<CharacterData> characterDatas = data.characters;
        foreach (var characterData in characterDatas)
        {
            Vector3 characterPosition = new Vector3(
                characterData.posX,
                characterData.posY,
                characterData.posZ
            );
            GenerateCharacter(characterPosition);
        }
        
        // For tiles
        List<TileData> tileDatas = data.tiles;
        foreach (var tileData in tileDatas)
        {
            Vector3 tilePosition = new Vector3(
                tileData.posX,
                tileData.posY,
                tileData.posZ
            );
            GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, tileMapContainer);
            if (tile.TryGetComponent<Tile>(out Tile tileComponent))
            {
                tileComponent.IsWalkable = tileData.isWalkable;
                if (!tileData.isWalkable)
                {
                    GameObject obstacle = Instantiate(obstaclePrefab, tilePosition + new Vector3(0f, 0.25f, 0f), Quaternion.identity, obstacleContainer);
                    _mapState.AddUnwalkableTile(tilePosition); // MapState is initialized in GenerateCharacter.
                }
                _tiles.Add(tileComponent);
            }
            else
            {
                Debug.LogWarning("Tile prefab does not have Tile component attached.");
            }
        }

    }
    
    public void GenerateCharacter(Vector3 characterPosition)
    {
        if (_mapState != null && _mapState.CanGenerateCharacterAt(characterPosition) || _mapState == null)
        {
            GameObject character = Instantiate(characterPrefab, characterPosition, Quaternion.identity);
            character.transform.SetParent(characterContainer);
        
            if (character.TryGetComponent(out CharacterController characterController))
            {
                characterController.InitialPosition = characterPosition;
                // Thêm vào MapState
                if (_mapState == null)
                {
                    _mapState = new MapState(_mapWidth, _mapLength, _distanceUnit, _tiles, characterController);
                }
                else
                {
                    _mapState.AddCharacter(characterController);
                }
            }
        }
    }
    
    private void CreateBasicMapForEditor()
    {
        ClearMap();
        
        // Create tiles with mapWidth and mapLength
        for (int x = 0; x < _mapWidth * _distanceUnit; x += _distanceUnit)
        {
            for (int z = 0; z < _mapLength * _distanceUnit; z += _distanceUnit)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, tileMapContainer);
                tile.transform.rotation = Quaternion.Euler(0, 0, 0); 
                
                if (tile.TryGetComponent<Tile>(out Tile tileComponent))
                {
                    _tiles.Add(tileComponent);
                }
            }
        }
    }

    private void ClearMap()
    {
        for (int i = 0; i < characterContainer.childCount; i++)
        {
            Destroy(characterContainer.GetChild(i).gameObject); // Xóa các nhân vật cũ nếu có
        }
        
        for (int i = 0; i < tileMapContainer.childCount; i++)
        {
            Destroy(tileMapContainer.GetChild(i).gameObject); // Xóa các tile cũ nếu có
        }
        
        for (int i = 0; i < obstacleContainer.childCount; i++)
        {
            Destroy(obstacleContainer.GetChild(i).gameObject); // Xóa các obstacle cũ nếu có
        }

        for (int i = 0; i < characterBodyContainer.childCount; i++)
        {
            Destroy(characterBodyContainer.GetChild(i).gameObject); // Xóa các body cũ nếu có
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
    
    public Transform TileMapContainer
    {
        get => tileMapContainer;
        set => tileMapContainer = value;
    }
    
    public Transform ObstacleContainer
    {
        get => obstacleContainer;
        set => obstacleContainer = value;
    }
    
    public Transform CharacterContainer
    {
        get => characterContainer;
        set => characterContainer = value;
    }
    
    public Transform CharacterBodyContainer
    {
        get => characterBodyContainer;
        set => characterBodyContainer = value;
    }
    
    public Vector3 DefaultCharacterPosition
    {
        get => defaultCharacterPosition;
        set => defaultCharacterPosition = value;
    }

    public MapEditor MapEditor
    {
        get => _mapEditor;
        set => _mapEditor = value;
    }
    
    #endregion
   
}