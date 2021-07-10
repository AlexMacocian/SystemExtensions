namespace System.Configuration
{
    public interface ILiveUpdateableOptions<T> : ILiveOptions<T>, IUpdateableOptions<T>
        where T : class
    {
    }
}
