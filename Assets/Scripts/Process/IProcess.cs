using Cysharp.Threading.Tasks;
namespace Process
{
    public interface IProcess
    {
        public void Initialization();
        public void Destroy();

    }
}
