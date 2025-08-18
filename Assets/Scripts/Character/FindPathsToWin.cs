using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FindPathsToWin
{
    private List<CharacterController> _characters = new List<CharacterController>();
    private Dictionary<CharacterController, List<List<Vector3>>> _currentPaths = new Dictionary<CharacterController, List<List<Vector3>>>();
    private Dictionary<CharacterController, List<Vector3>> _winPaths = new Dictionary<CharacterController, List<Vector3>>();
    private Dictionary<CharacterController, Stack<Vector3>> _startPosPerState = new Dictionary<CharacterController, Stack<Vector3>>();
    private bool _isWin = false;
    private List<Vector3> _directions = new List<Vector3>()
    {
        Vector3.right,
        Vector3.forward,
        Vector3.left,
        Vector3.back
    };
    

    public void FindAllMovePathToWin()
    {
        MapState tempMState = MapManager.Instance.MapState;
        
        if (_isWin) return;

        if (tempMState.IsWin())
        {
            _isWin = true;
            foreach (var character in _characters)
            {
                _winPaths[character] = _currentPaths[character].SelectMany(x => x).ToList();
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
                    _currentPaths[character].Add(new List<Vector3>(character.MovePaths)); 
                }
                
            }
            
            if (isStateChanged)
            {
                FindAllMovePathToWin();
                foreach (var character in _characters)
                {
                    int numberOfMovePaths = _currentPaths[character].Count;
                    
                    for (int i = 0; i < _currentPaths[character][numberOfMovePaths - 1].Count; i++)
                    {
                        tempMState.UnvisitTile(_currentPaths[character][numberOfMovePaths - 1][i] - new Vector3(0, 0.25f, 0));
                    }
                    
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