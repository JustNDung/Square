public class LevelSaveLoadManager : BaseSaveLoadManager<LevelData>
{
    public LevelSaveLoadManager() : base("http://localhost:3000/api/level") { }
}
