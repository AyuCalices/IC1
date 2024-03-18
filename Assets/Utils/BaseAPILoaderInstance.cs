using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public abstract class BaseAPILoaderInstance : MonoBehaviour, IAPILoaderInstance
    {
        [SerializeField, TextArea] private string _startupPath;
        [SerializeField, TextArea] private string _fileName;
    
        public string DirectoryPath => _startupPath;
        public string FileName => _fileName;

        public virtual Task<bool> StartupFailed()
        {
            return Task.FromResult(true);
        }

        public virtual Task Initiate()
        {
            return Task.CompletedTask;
        }
    }
}
