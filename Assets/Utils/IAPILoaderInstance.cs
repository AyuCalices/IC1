using System;
using System.Threading.Tasks;

namespace Utils
{
    public interface IAPILoaderInstance
    {
        public string DirectoryPath { get; }
        public string FileName { get; }
        public bool CanStartupAPI { get; }
        public Task<bool> TryStartup(Action<string> updateProgressMethod);
        public Task OnStart(Action<string> updateProgressMethod);
    }
}
