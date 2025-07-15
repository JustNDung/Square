public interface ISaveLoadManager<T>
{
    void Save(T data, System.Action<bool> onComplete = null);
    void Load(string key, System.Action<T> onLoaded);
}