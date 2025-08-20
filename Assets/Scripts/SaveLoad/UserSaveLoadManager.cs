public class UserSaveLoadManager : BaseSaveLoadManager<UserData>
{
    public UserSaveLoadManager() : base("http://localhost:3000/api/user") { }
}
