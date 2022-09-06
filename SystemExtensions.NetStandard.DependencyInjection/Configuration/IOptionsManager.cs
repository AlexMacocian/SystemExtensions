namespace System.Configuration;

public interface IOptionsManager
{
    T GetOptions<T>()
        where T : class;
    void UpdateOptions<T>(T value)
        where T : class;
}
