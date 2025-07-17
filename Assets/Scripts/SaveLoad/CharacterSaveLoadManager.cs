
public class CharacterSaveLoadManager : BaseSaveLoadManager<CharacterData>
{
    public CharacterSaveLoadManager() : base("http://localhost:3000/api/characters") { }
}
