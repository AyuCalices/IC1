using System;
using System.Threading.Tasks;

namespace Features.Connection.Scripts.APILoader
{
    public interface IAPILoaderInstance
    {
        public string DirectoryPath { get; }
        public string FileName { get; }
        public bool CanStartupAPI { get; }
        public Task<bool> TryStartup(Action<string> updateProgressMethod);
    }
}
