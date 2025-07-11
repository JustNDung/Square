using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private int _distanceMove = 2; // Khoảng cách di chuyển mỗi lần
    [SerializeField] private float _moveSpeed = 5f;
    private Vector3 _mouseStart = Vector3.zero;
    private Vector3 _mouseEnd = Vector3.zero;
    private bool _isMouseSwiping = false;
    private Vector3 _targetPosition;
    private bool _isMoving = false;

    private MapState _mapState;

    private void Start()
    {
        _targetPosition = transform.position;
        _mapState = MapManager.Instance.MapState; // Lấy MapState từ MapManager
    }

    private void Update()
    {
        HandleSwipe();
        MoveSmooth();
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

            if (direction != Vector3.zero && !_isMoving)
            {
                _isMoving = true;
                FindFinalTargetPosition(direction);
            }
        }
    }
    #endregion
    
    #region Find target and move to

    private void FindFinalTargetPosition(Vector3 direction)
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

        if (direction == Vector3.right || direction == Vector3.left)
        {
            _targetPosition = FindTileToMoveOnX(distanceMove);
        }
        else if (direction == Vector3.forward || direction == Vector3.back)
        {
            _targetPosition = FindTileToMoveOnZ(distanceMove);
        }
        
    }
    
    private void MoveSmooth()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }
    #endregion
    
    #region Tile Finding Methods

    private Vector3 FindTileToMoveOnZ(int distanceMove)
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
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
        
        return currentPos;
    }
    
    private Vector3 FindTileToMoveOnX(int distanceMove)
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
                _mapState.VisitTile(targetTile);
                currentPos = new Vector3(targetTile.x, 0.25f, targetTile.z);
            }
            else
            {
                break; // Dừng lại khi tìm thấy tile đã đi qua
            }
            
        }
        
        return currentPos;
    }
    #endregion
    
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
}
