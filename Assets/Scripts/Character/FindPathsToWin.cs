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
        
        int stateHash = GetStateHash();
        if (_visitedStates.Contains(stateHash)) return;
        _visitedStates.Add(stateHash);

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
        }

        foreach (var character in _characters)
        {
            _startPosPerState[character].Push(character.CurrentPosByCalculate);
        }
        
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

            if (isStateChanged)
            {
                foreach (var character in _characters)
                {
                    // dùng pool thay vì new List
                    var pooledList = ListPool<Vector3>.Get();
                    pooledList.AddRange(character.MovePaths);
                    _currentPaths[character].Add(pooledList);
                }

                FindAllMovePathToWin();

                foreach (var character in _characters)
                {
                    int numberOfMovePaths = _currentPaths[character].Count;
                    if (numberOfMovePaths == 0) continue;

                    var lastPath = _currentPaths[character][numberOfMovePaths - 1];
                    for (int i = 0; i < lastPath.Count; i++)
                    {
                        MapManager.Instance.MapState.UnvisitTile(lastPath[i] - _offset);
                    }

                    // Trả lại list vào pool
                    ListPool<Vector3>.Release(lastPath);

                    _currentPaths[character].RemoveAt(numberOfMovePaths - 1);

                    if (_startPosPerState[character].Count > 0)
                    {
                        _startPosPerState[character].Pop();
                    }

                    if (_startPosPerState[character].Count > 0)
                    {
                        character.CurrentPosByCalculate = _startPosPerState[character].Peek();
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
