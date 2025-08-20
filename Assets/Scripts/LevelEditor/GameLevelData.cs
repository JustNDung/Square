using System.Collections.Generic;
[System.Serializable]
public class LevelSaveData
{
    public List<TileData> tiles;
    public List<CharacterData> characters;
    public LevelData level;
    public MapData map;
}