using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public class APILoader : MonoBehaviour
    {
        [SerializeField] private bool _loadOnStart;
        [SerializeField] private List<BaseAPILoaderInstance> _apiLoaderOrder;
        [SerializeField] private UnityEvent _onLoadComplete;
    
        private const int TaskDelay = 1000;
    
        private readonly CancellationTokenSource _cancellationToken = new();

        private async void Start()
        {
            if (_loadOnStart)
            {
                LoadAPIInstances();
            }
        }

        public async void LoadAPIInstances()
        {
            if (_apiLoaderOrder.Any(x => !x.CanBeStarted()))
            {
                Debug.LogWarning("Couldn't be started, because a loader is not properly configured!");
                return;
            }
            
            foreach (BaseAPILoaderInstance apiLoaderInstance in _apiLoaderOrder)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;
                
                bool startupFailed = await apiLoaderInstance.StartupFailed();
                if (startupFailed)
                {
                    await StartupAPI(apiLoaderInstance);
                }

                await apiLoaderInstance.Initiate();
            }
            
            _onLoadComplete?.Invoke();
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }
        
        private async Task StartupAPI(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            if (StartProcess(baseAPILoaderInstance))
            {
                await AwaitSetupOobabooga(baseAPILoaderInstance);
            }
        }

        private bool StartProcess(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            if (System.IO.File.Exists(baseAPILoaderInstance.DirectoryPath + "\\" + baseAPILoaderInstance.FileName))
            {
                // Create a new ProcessStartInfo instance
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = baseAPILoaderInstance.FileName,
                    WorkingDirectory = baseAPILoaderInstance.DirectoryPath,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                return true;
            }

            Debug.LogError("File does not exist at path: " + baseAPILoaderInstance.DirectoryPath);
            return false;
        }
        
        private async Task AwaitSetupOobabooga(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            while (await baseAPILoaderInstance.StartupFailed() && !_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TaskDelay);
            }
        }
    }
}
