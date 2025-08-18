using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    private CharacterType _characterType = CharacterType.None;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f; // Tăng tốc độ di chuyển
    [SerializeField] private float smoothFactor = 0.8f; // Hệ số làm mượt chuyển động
    [SerializeField] private float characterY = 0.25f;
    private Vector3 _initialPosition; // Vị trí ban đầu của nhân vật
    private Vector3 _currentPosByCalculate;
    private Vector3 _initialPosPerMovePath;
    private Vector3 _mouseStart = Vector3.zero;
    private Vector3 _mouseEnd = Vector3.zero;
    private bool _isMouseSwiping = false;
    private Coroutine _moveCoroutine;
    private List<Vector3> _movePaths = new List<Vector3>(); // Lưu vị trí các tile đi qua trong 1 lần di chuyển
    private List<Vector3> _visitedTiles = new List<Vector3>(); // Lưu các tile đã đi qua của nhân vật
    
    [Header("Body Parts Settings")]
    [SerializeField] private GameObject bodyPartPrefab; // Prefab cho các phần thân
    [SerializeField] [Range(0f, 1f)] private float spawnThreshold = 0.75f; // Ngưỡng % di chuyển để spawn body part
    private GameObject _characterBodyContainer; // Container chứa các phần thân của nhân vật

    private void Awake()
    {
        _characterBodyContainer = new GameObject("BodyPartsOf" + gameObject.name);
        _characterBodyContainer.transform.SetParent(MapManager.Instance.CharacterBodyContainer);
        
        _initialPosition = transform.position;
        _currentPosByCalculate = transform.position;
        _visitedTiles.Add(transform.position - new Vector3(0, 0.25f, 0)); // Lưu vị trí ban đầu vào danh sách đã đi qua
    }
    
    private void Update()
    { 
        HandleSwipe();
    }
    
    #region Handle input

    private void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStart = Input.mousePosition;
            _isMouseSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && _isMouseSwiping)
        {
            _mouseEnd = Input.mousePosition;
            _isMouseSwiping = false;

            Vector3 swipeDirection = _mouseEnd - _mouseStart;
            Vector3 direction = ConvertDirection(swipeDirection);

            if (direction != Vector3.zero && _moveCoroutine == null)
            {
                FindMovePaths(direction, transform.position);
                
                if (_movePaths.Count > 0)
                {
                    MoveCharacter(_movePaths);
                }
            }
        }
    }
    #endregion
    
    #region Find move paths and move character
    public void FindMovePaths(Vector3 direction, Vector3 currentPos)
    {
        if (direction == Vector3.zero) return;
        int distanceUnit = MapManager.Instance.DistanceUnit; // Khoảng cách di chuyển theo đơn vị của bản đồ

        if (_characterType == CharacterType.Confused)
        {
            distanceUnit = -distanceUnit;
        }

        if (direction == Vector3.right || direction == Vector3.forward)
        {
            distanceUnit *= 1; // Chiều dương
        }
        else if (direction == Vector3.left || direction == Vector3.back)
        {
            distanceUnit *= -1; // Chiều âm
        }
        
        _movePaths.Clear();

        if (direction == Vector3.right || direction == Vector3.left)
        {
            FindMovePathsOnX(distanceUnit, currentPos, direction);
        }
        else if (direction == Vector3.forward || direction == Vector3.back)
        {
            FindMovePathsOnZ(distanceUnit, currentPos, direction);
        }
        
    }

    public void MoveCharacter(List<Vector3> movePaths)
    {
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(Move(movePaths));
        } 
    }

    private IEnumerator Move(List<Vector3> movePaths)
    {
        Vector3 current = transform.position;
        for (int i = 0; i < movePaths.Count; i++)
        {
            Vector3 movePath = movePaths[i];
            
            Vector3 start = current;
            Vector3 end = movePath;

            float distanceToTravel = Vector3.Distance(start, end);
            float distanceTraveled = 0f;
            bool hasSpawnedBodyPart = false;
            float spawnThreshold = this.spawnThreshold; // Use the configurable threshold

            while (distanceTraveled < distanceToTravel)
            {
                float step = moveSpeed * Time.deltaTime;
                distanceTraveled += step;
                float moveProgress = distanceTraveled / distanceToTravel;
                
                Vector3 targetPosition = Vector3.Lerp(start, end, moveProgress);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor);

                // Spawn body part when we've moved 75% of the distance
                if (!hasSpawnedBodyPart && moveProgress >= spawnThreshold && !MapManager.Instance.MapState.IsSpecialTile(start - new Vector3(0, 0.25f, 0)))
                {
                    SpawnBodyParts(start);
                    hasSpawnedBodyPart = true;
                }

                yield return null;
            }

            transform.position = end;
            current = end;
        }
        
        _moveCoroutine = null;
    }
    #endregion
    
    #region Tile finding methods

    private void FindMovePathsOnZ(int distanceMove, Vector3 currentPos, Vector3 direction)
    {
        MapState tempMState = MapManager.Instance.MapState;
        Vector3 characterTile = currentPos - new Vector3(0, 0.25f, 0);
        _initialPosPerMovePath = currentPos;
        
        if (tempMState.IsSpecialTile(characterTile) && tempMState.SpecialTiles[characterTile].Type == TileType.Horizontal)
        {
            return;
        }

        // Duyệt từ vị trí hiện tại đến hết map theo trục Z
        for (float z = currentPos.z + distanceMove; z < MapManager.Instance.MapLength * MapManager.Instance.DistanceUnit && z >= 0; z += distanceMove)
        {
            Vector3 targetTile = new Vector3(currentPos.x, 0, z);

            // Nếu tile này chưa được đi qua
            if (tempMState.CanMoveTo(targetTile))
            {
                if (tempMState.IsSpecialTile(targetTile))
                {
                    currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
                    UpdateMovePathsWithSpecialTile(targetTile, distanceMove, currentPos, direction);
                    break;
                }
                
                // Đánh dấu đã đi qua
                MapManager.Instance.MapState.VisitTile(targetTile);
                _visitedTiles.Add(targetTile); // Lưu tổng các tile đã đi qua
                currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
                _movePaths.Add(currentPos); // Lưu tile đi qua trong 1 lần di chuyển
                _currentPosByCalculate = currentPos; // Lưu vị trí cuối cùng trong 1 lần di chuyển khi tính toán hướng đi.
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
    }
    
    private void FindMovePathsOnX(int distanceMove, Vector3 currentPos, Vector3 direction)
    {
        MapState tempMState = MapManager.Instance.MapState;
        Vector3 characterTile = currentPos - new Vector3(0, 0.25f, 0);
        _initialPosPerMovePath = currentPos;

        if (tempMState.IsSpecialTile(characterTile) && tempMState.SpecialTiles[characterTile].Type == TileType.Vertical)
        {
            return;
        }
        
        // Duyệt từ vị trí hiện tại đến hết map theo trục X
        for (float x = currentPos.x + distanceMove; x < MapManager.Instance.MapWidth * MapManager.Instance.DistanceUnit && x >= 0; x += distanceMove)
        {
            Vector3 targetTile = new Vector3(x, 0, currentPos.z);

            // Nếu tile này chưa được đi qua
            if (tempMState.CanMoveTo(targetTile))
            {
                if (tempMState.IsSpecialTile(targetTile))
                {
                    currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
                    UpdateMovePathsWithSpecialTile(targetTile, distanceMove, currentPos, direction);
                    break;
                }
                
                MapManager.Instance.MapState.VisitTile(targetTile); // Đánh dấu tất cả tile đã đi qua
                _visitedTiles.Add(targetTile); // Lưu tổng các tile đã đi qua
                currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
                _movePaths.Add(currentPos); // Lưu các tile đi qua trong 1 lần di chuyển
                _currentPosByCalculate = currentPos;
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
    }
    
    // Find _movePaths with each type of tile.
    private void UpdateMovePathsWithSpecialTile(Vector3 tilePos, int distanceMove, Vector3 currentPos, Vector3 direction)
    {
        MapState tempMState = MapManager.Instance.MapState;
        switch (MapManager.Instance.MapState.SpecialTiles[tilePos].Type)
        {
            case TileType.Teleport:
                
                Vector3 targetTile = currentPos - new Vector3(0, 0.25f, 0);
                _movePaths.Add(currentPos); // Thêm vị trí của teleport tile vào _movePaths
                
                currentPos = new Vector3(tempMState.TeleportPair[targetTile].x, characterY, tempMState.TeleportPair[targetTile].z); // teleport character den teleport tile con lai
                _movePaths.Add(currentPos); // Thêm teleport tile con lai vao _movePaths
                
                if (direction == Vector3.left || direction == Vector3.right)
                {
                    FindMovePathsOnX(distanceMove, currentPos, direction);
                } 
                else if (direction == Vector3.forward || direction == Vector3.back)
                {
                    FindMovePathsOnZ(distanceMove, currentPos, direction);
                }
                
                break;
            case TileType.Up:

                if (direction == Vector3.down)
                {
                    _movePaths.Add(currentPos);
                    return;
                }
                direction = Vector3.forward;
                distanceMove = 2;
                
                _movePaths.Add(currentPos);
                FindMovePathsOnZ(distanceMove, currentPos, direction);
                
                break;
            case TileType.Down:

                if (direction == Vector3.up)
                {
                    _movePaths.Add(currentPos);
                    return;
                }
                direction = Vector3.back;
                distanceMove = -2;
                
                _movePaths.Add(currentPos);
                FindMovePathsOnZ(distanceMove, currentPos, direction);
                
                break;
            case TileType.Right:

                if (direction == Vector3.left)
                {
                    _movePaths.Add(currentPos);
                    return;
                }
                direction = Vector3.right;
                distanceMove = 2;
                
                _movePaths.Add(currentPos);
                FindMovePathsOnX(distanceMove, currentPos, direction);
                
                break;
            case TileType.Left:

                if (direction == Vector3.right)
                {
                    _movePaths.Add(currentPos);
                    return;
                }
                direction = Vector3.left;
                distanceMove = -2;
                
                _movePaths.Add(currentPos);
                FindMovePathsOnX(distanceMove, currentPos, direction);
                
                break;
            case TileType.Horizontal:
                
                if (direction == Vector3.forward || direction == Vector3.back)
                {
                    return;
                }
                
                _movePaths.Add(currentPos);
                FindMovePathsOnX(distanceMove, currentPos, direction);
                
                break;
            case TileType.Vertical:
                
                if (direction == Vector3.right || direction == Vector3.left)
                {
                    return;
                }
                
                _movePaths.Add(currentPos);
                FindMovePathsOnZ(distanceMove, currentPos, direction);
                
                break;
            default:
                break;
        }
    }
    
    #endregion
    
    // Chuyển đồi hướng vuốt chuột thành hướng di chuyển
    private Vector3 ConvertDirection(Vector3 direction)
    {
        if (direction.magnitude < 50f) return Vector3.zero; // Ngưỡng tối thiểu

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return direction.y > 0 ? Vector3.forward : Vector3.back;
        }
    }
    
    private void SpawnBodyParts(Vector3 spawnPosition)
    {
        Instantiate(bodyPartPrefab, spawnPosition, Quaternion.identity, _characterBodyContainer.transform);
    }
    
    // Getters and Setters
    public Vector3 InitialPosition 
    {
        get => _initialPosition;
        set => _initialPosition = value;
    }
    public GameObject CharacterBodyContainer
    {
        get => _characterBodyContainer;
        set => _characterBodyContainer = value; // Cần thiết nếu muốn thay đổi container    
    }
    public List<Vector3> VisitedTiles
    {
        get => _visitedTiles;
        set => _visitedTiles = value; // Cần thiết nếu muốn thay đổi danh sách đã đi qua
    }

    public CharacterType CharacterType
    {
        get => _characterType;
        set => _characterType = value;
    }
    
    public List<Vector3> MovePaths
    {
        get => _movePaths;
        set => _movePaths = value;
    }
    
    public Vector3 CurrentPosByCalculate
    {
        get => _currentPosByCalculate;
        set => _currentPosByCalculate = value;
    }

    public Vector3 InitialPosPerMovePath
    {
        get => _initialPosPerMovePath;
        set => _initialPosPerMovePath = value;
    }
}
