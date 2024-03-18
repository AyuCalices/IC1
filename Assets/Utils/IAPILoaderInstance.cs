using System.Threading.Tasks;

namespace Utils
{
    public interface IAPILoaderInstance
    {
        public string DirectoryPath { get; }
        public string FileName { get; }
        public Task<bool> StartupFailed();
        public Task Initiate();
    }
}
