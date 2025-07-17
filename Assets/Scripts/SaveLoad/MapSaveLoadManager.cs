public class MapSaveLoadManager  : BaseSaveLoadManager<MapData>
{
    public MapSaveLoadManager() : base("http://localhost:3000/api/map") { }
}
