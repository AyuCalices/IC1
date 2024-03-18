using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public class APILoader : MonoBehaviour
    {
        [SerializeField] private List<BaseAPILoaderInstance> _apiLoaderOrder;
    
        private const int TaskDelay = 1000;
    
        private readonly CancellationTokenSource _cancellationToken = new();

        private async void Start()
        {
            foreach (BaseAPILoaderInstance apiLoaderInstance in _apiLoaderOrder)
            {
                bool startupFailed = await apiLoaderInstance.StartupFailed();
                if (startupFailed)
                {
                    await StartupAPI(apiLoaderInstance);
                }

                await apiLoaderInstance.Initiate();
            }
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }
        
        private async Task StartupAPI(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            if (StartOobaboogaProcess(baseAPILoaderInstance))
            {
                await AwaitSetupOobabooga(baseAPILoaderInstance);
            }
        }

        private bool StartOobaboogaProcess(BaseAPILoaderInstance baseAPILoaderInstance)
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
