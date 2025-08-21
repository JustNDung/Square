using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool; // có từ Unity 2021

public class FindPathsToWin
{
    private List<CharacterController> _characters = new List<CharacterController>();
    private Dictionary<CharacterController, List<List<Vector3>>> _currentPaths = new Dictionary<CharacterController, List<List<Vector3>>>();
    private Dictionary<CharacterController, List<Vector3>> _winPaths = new Dictionary<CharacterController, List<Vector3>>();
    private Dictionary<CharacterController, Stack<Vector3>> _startPosPerState = new Dictionary<CharacterController, Stack<Vector3>>();
    private HashSet<int> _visitedStates = new HashSet<int>();
    private bool _isWin = false;

    private Vector3[] _directions = new Vector3[]
    {
        Vector3.left,
        Vector3.forward,
        Vector3.back,
        Vector3.right
    };

    private readonly Vector3 _offset = new Vector3(0, 0.25f, 0);

    public void FindAllMovePathToWin()
    {
        if (_isWin) return;

        if (MapManager.Instance.MapState.IsWin())
        {
            _isWin = true;
            foreach (var character in _characters)
            {
                // dùng pool để giảm alloc thay vì ToList()
                var pooledList = ListPool<Vector3>.Get();
                foreach (var path in _currentPaths[character])
                {
                    pooledList.AddRange(path);
                }
                _winPaths[character] = pooledList;
            }
            return;
        } // Win thì dừng và lưu lại winpath.

        foreach (var character in _characters)
        {
            _startPosPerState[character].Push(character.CurrentPosByCalculate);
        } // đẩy start position của character mỗi lần move vào stack để backtrack.
        
        int stateHash = GetStateHash();
        if (_visitedStates.Contains(stateHash)) return; // gặp state trung lap thì return ko xét tiếp
        _visitedStates.Add(stateHash);
        
        foreach (var direction in _directions)
        {
            bool isStateChanged = false;
            foreach (var character in _characters)
            {
                character.FindMovePaths(direction, character.CurrentPosByCalculate);
                if (character.MovePaths.Count > 0)
                {
                    isStateChanged = true;
                }
            }
            // tìm duong di cho mỗi character với 1 hướng cụ thể
            // map state chỉ thay đổi khi mà 1 nhân vật tìm duoc đường đi

            if (isStateChanged)
            {
                foreach (var character in _characters)
                {
                    // dùng pool thay vì new List
                    var pooledList = ListPool<Vector3>.Get();
                    pooledList.AddRange(character.MovePaths);
                    _currentPaths[character].Add(pooledList);
                } // nếu map state thay đổi thì add các move path tương ứng với mỗi character vừa tìm được vào _currentPaths để còn backtrack.

                FindAllMovePathToWin(); // đệ quy để tìm đường đi tiếp nếu state có thay đổi.
                
                // nếu không tìm được duong di với mọi hướng và mọi character, thoát nhánh đệ quy và backtrack với đoạn code dưới đây:
                foreach (var character in _characters)
                {
                    int numberOfMovePaths = _currentPaths[character].Count;
                    if (numberOfMovePaths == 0) continue;

                    var lastPath = _currentPaths[character][numberOfMovePaths - 1];
                    for (int i = 0; i < lastPath.Count; i++)
                    {
                        MapManager.Instance.MapState.UnvisitTile(lastPath[i] - _offset);
                    } // Unvisit các tile vưa đi qua
                    
                    ListPool<Vector3>.Release(lastPath); // Trả lại list vào pool

                    _currentPaths[character].RemoveAt(numberOfMovePaths - 1); // remove last move paths ra khỏi _currentPaths.

                    if (_startPosPerState[character].Count > 0)
                    {
                        _startPosPerState[character].Pop(); // remove vị trí bắt đầu của nhân vật.
                    }

                    if (_startPosPerState[character].Count > 0)
                    {
                        character.CurrentPosByCalculate = _startPosPerState[character].Peek(); // gán vị trí hiện tại của nhân vat là vị trí bắt đầu của state truoc do.
                    }
                }
            }
        }
    }

    private int GetStateHash()
    {
        unchecked
        {
             int hash = 17;
             
             foreach (var character in _characters) {
                 hash = hash * 31 + character.CurrentPosByCalculate.GetHashCode();
             }

             foreach (var tile in MapManager.Instance.MapState.VisitedTiles)
             {
                 hash = hash * 31 + tile.GetHashCode();
             }
             
             return hash;
            
        }
    }
    public void AddCharacter(CharacterController character)
    {
        _characters.Add(character);
        _currentPaths.Add(character, new List<List<Vector3>>());
        _winPaths.Add(character, new List<Vector3>());
        _startPosPerState.Add(character, new Stack<Vector3>());
    }

    public List<CharacterController> Characters => _characters;
    public Dictionary<CharacterController, List<Vector3>> WinPaths => _winPaths;
}
