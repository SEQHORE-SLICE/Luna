using Cysharp.Threading.Tasks;
namespace Framework
{
    /// <summary>
    ///     System service interface
    /// </summary>
    internal interface IService
    {
        /// <summary>
        ///     Initialize the service,allocate system resources
        /// </summary>
        public UniTaskVoid InitializeAsync();

        /// <summary>
        ///     Service calls each other
        /// </summary>
        public void PostInitialize();

        /// <summary>
        ///     Stop the service, release all the held resources
        /// </summary>
        public void Destroy();
    }
}
