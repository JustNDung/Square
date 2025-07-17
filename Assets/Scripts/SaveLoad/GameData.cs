using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public string userId;
    [FormerlySerializedAs("player")] public CharacterData character;
    public MapData map;
    
}
