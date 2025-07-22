using System.Collections.Generic;
[System.Serializable]
public class MapData
{
    public int mapId;
    public int levelId;
    public int width;
    public int length;
    
    public MapData (int mapId, int levelId, int width, int length)
    {
        this.mapId = mapId;
        this.levelId = levelId;
        this.width = width;
        this.length = length;
    }
}
