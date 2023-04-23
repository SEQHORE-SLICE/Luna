namespace Utilities
{
    /// <summary>
    /// Ordinary singleton class
    /// </summary>
    /// <typeparam name="T">The class itself</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        public static T instance { get; } = new T();
    }
}
