using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f; // Tăng tốc độ di chuyển
    [SerializeField] private float smoothFactor = 0.8f; // Hệ số làm mượt chuyển động
    [SerializeField] private float characterY = 0.25f;
    private Vector3 _mouseStart = Vector3.zero;
    private Vector3 _mouseEnd = Vector3.zero;
    private bool _isMouseSwiping = false;
    private Coroutine _moveCoroutine;
    private List<Vector3> _movePaths = new List<Vector3>(); // Lưu vị trí các tile đi qua trong 1 lần di chuyển
    
    [Header("Body Parts Settings")]
    [SerializeField] private GameObject bodyPartPrefab; // Prefab cho các phần thân
    [SerializeField] [Range(0f, 1f)] private float spawnThreshold = 0.75f; // Ngưỡng % di chuyển để spawn body part
    
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
        int distanceUnit = MapManager.Instance.DistanceUnit; // Khoảng cách di chuyển theo đơn vị của bản đồ

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
            FindMovePathsOnX(distanceUnit);
        }
        else if (direction == Vector3.forward || direction == Vector3.back)
        {
            FindMovePathsOnZ(distanceUnit);
        }
        
    }

    private IEnumerator Move()
    {
        Vector3 current = transform.position;
        foreach (var nextTile in _movePaths)
        {
            Vector3 start = current;
            Vector3 end = nextTile;

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
                if (!hasSpawnedBodyPart && moveProgress >= spawnThreshold)
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

    private void FindMovePathsOnZ(int distanceMove)
    {
        Vector3 currentPos = transform.position;

        // Duyệt từ vị trí hiện tại đến hết map theo trục Z
        for (float z = currentPos.z + distanceMove; z < MapManager.Instance.MapLength * MapManager.Instance.DistanceUnit && z >= 0; z += distanceMove)
        {
            Vector3 targetTile = new Vector3(currentPos.x, 0, z);

            // Nếu tile này chưa được đi qua
            if (MapManager.Instance.MapState.CanMoveTo(targetTile))
            {
                // Đánh dấu đã đi qua
                MapManager.Instance.MapState.VisitTile(targetTile);
                currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
                _movePaths.Add(currentPos); // Lưu tile vừa rời khoi
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
    }
    
    private void  FindMovePathsOnX(int distanceMove)
    {
        Vector3 currentPos = transform.position;
        
        // Duyệt từ vị trí hiện tại đến hết map theo trục X
        for (float x = currentPos.x + distanceMove; x < MapManager.Instance.MapWidth * MapManager.Instance.DistanceUnit && x >= 0; x += distanceMove)
        {
            Vector3 targetTile = new Vector3(x, 0, currentPos.z);

            // Nếu tile này chưa được đi qua
            if (MapManager.Instance.MapState.CanMoveTo(targetTile))
            {
                // Đánh dấu đã đi qua
                MapManager.Instance.MapState.VisitTile(targetTile); // Đánh dấu tất cả tile đã đi qua
                currentPos = new Vector3(targetTile.x, characterY, targetTile.z);
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
        Instantiate(bodyPartPrefab, spawnPosition, Quaternion.identity, MapManager.Instance.CharacterBodyContainer);
    }
}
