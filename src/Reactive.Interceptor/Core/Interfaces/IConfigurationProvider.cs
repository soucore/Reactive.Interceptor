namespace Reactive.Interceptor.Core.Interfaces
{
    public interface IConfigurationProvider<T>
    {
        /// <summary>
        /// Access to the root project's source settings.
        /// </summary>
        /// <returns>T genric type</returns>
        T GetBySource();

        /// <summary>
        /// Access to the root project's sink settings.
        /// </summary>
        /// <returns>T genric type</returns>
        T GetBySink();
    }
}
