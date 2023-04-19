namespace Utilities
{
    /// <summary>
    /// Ordinary singleton class
    /// </summary>
    /// <typeparam name="T">The class itself</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;
        public static T instance => _instance ??= new T();
    }
}
