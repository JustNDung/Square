using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private int _distanceMove = 2; // Khoảng cách di chuyển mỗi lần
    [SerializeField] private float _moveSpeed = 5f;
    private Vector3 _mouseStart = Vector3.zero;
    private Vector3 _mouseEnd = Vector3.zero;
    private bool _isMouseSwiping = false;
    private Coroutine _moveCoroutine;
    private List<Vector3> _movePaths = new List<Vector3>(); // Lưu vị trí các tile đi qua trong 1 lần di chuyển
    
    [Header("Body Parts Settings")]
    [SerializeField] private GameObject _bodyPartPrefab; // Prefab cho các phần thân
    [SerializeField] private string _bodyContainerName = "CharacterBody"; // Tên của container chứa các phần thân
    private GameObject _bodyPartsContainer; // Container chứa các phần thân
    
    private MapState _mapState;

    private void Start()
    {
        _bodyPartsContainer = new GameObject(_bodyContainerName);
        _mapState = MapManager.Instance.MapState; // Lấy MapState từ MapManager
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
                FindMovePaths(direction);
                
                if (_movePaths.Count > 0)
                {
                    _moveCoroutine = StartCoroutine(Move());
                }
            }
        }
    }
    #endregion
    
    #region Find move paths and move character

    private void FindMovePaths(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        int distanceMove = _distanceMove;

        if (direction == Vector3.right || direction == Vector3.forward)
        {
            distanceMove *= 1; // Chiều dương
        }
        else if (direction == Vector3.left || direction == Vector3.back)
        {
            distanceMove *= -1; // Chiều âm
        }
        
        _movePaths.Clear();

        if (direction == Vector3.right || direction == Vector3.left)
        {
            FindMovePathsOnX(distanceMove);
        }
        else if (direction == Vector3.forward || direction == Vector3.back)
        {
            FindMovePathsOnZ(distanceMove);
        }
        
    }

    private IEnumerator Move()
    {
        Vector3 current = transform.position;
        foreach (var nextTile in _movePaths)
        {
            Vector3 start = current;
            Vector3 end = nextTile;

            // Move from start to end
            while (Vector3.Distance(transform.position, end) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, end, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = end;
            
            // TODO: Spawn body part một cách mượt mà hơn
            
            // Spawn body part tại tile vừa rời
            SpawnBodyParts(start);

            current = end;
        }
        
        _moveCoroutine = null;

    }
    #endregion
    
    #region Tile finding methods

    private void FindMovePathsOnZ(int distanceMove)
    {
        Vector3 currentPos = transform.position;

        // Duyệt từ vị trí hiện tại đến hết map theo trục Z
        for (float z = currentPos.z + distanceMove; z < _mapState.Length * _mapState.DistanceUnit && z >= 0; z += distanceMove)
        {
            Vector3 targetTile = new Vector3(currentPos.x, 0, z);

            // Nếu tile này chưa được đi qua
            if (_mapState.CanMoveTo(targetTile))
            {
                // Đánh dấu đã đi qua
                _mapState.VisitTile(targetTile);
                currentPos = new Vector3(targetTile.x, 0.25f, targetTile.z);
                _movePaths.Add(currentPos); // Lưu tile vừa rời khoi
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
    }
    
    private void FindMovePathsOnX(int distanceMove)
    {
        Vector3 currentPos = transform.position;

        // Duyệt từ vị trí hiện tại đến hết map theo trục Z
        for (float x = currentPos.x + distanceMove; x < _mapState.Width * _mapState.DistanceUnit && x >= 0; x += distanceMove)
        {
            Vector3 targetTile = new Vector3(x, 0, currentPos.z);

            // Nếu tile này chưa được đi qua
            if (_mapState.CanMoveTo(targetTile))
            {
                // Đánh dấu đã đi qua
                _mapState.VisitTile(targetTile); // Đánh dấu tất cả tile đã đi qua
                currentPos = new Vector3(targetTile.x, 0.25f, targetTile.z);
                _movePaths.Add(currentPos); // Lưu các tile đi qua trong 1 lần di chuyển
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
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
        Instantiate(_bodyPartPrefab, spawnPosition, Quaternion.identity, _bodyPartsContainer.transform);
    }
}
