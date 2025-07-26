
public class GameLevelSaveLoadManager : BaseSaveLoadManager<GameLevelData>
{
    public GameLevelSaveLoadManager() : base("http://localhost:3000/api/level") { }
}
