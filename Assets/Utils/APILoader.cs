using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public class APILoader : MonoBehaviour
    {
        [SerializeField] private bool _loadOnStart;
        [SerializeField] private List<BaseAPILoaderInstance> _apiLoaderOrder;
        
        [Header("Visualization")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onLoadComplete;
        [SerializeField] private UnityEvent _onLoadFailed;
    
        private const int TaskDelay = 1000;
    
        private readonly CancellationTokenSource _cancellationToken = new();

        private async void Start()
        {
            if (_loadOnStart)
            {
                LoadAPIInstances();
            }
        }
        
        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }

        public async void LoadAPIInstances()
        {
            List<BaseAPILoaderInstance> instancesToStartAPI = new();
            foreach (BaseAPILoaderInstance apiLoaderInstance in _apiLoaderOrder)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;
                
                if (await apiLoaderInstance.TryStartup(UpdateProgressState)) continue;

                if (apiLoaderInstance.CanStartupAPI)
                {
                    instancesToStartAPI.Add(apiLoaderInstance);
                }
                else
                {
                    Debug.LogError("Load failed on " + apiLoaderInstance.GetType());
                    _onLoadFailed.Invoke();
                    return;
                }
            }
            
            foreach (BaseAPILoaderInstance apiLoaderInstance in instancesToStartAPI)
            {
                await StartupAPI(apiLoaderInstance);
            }
            
            foreach (BaseAPILoaderInstance apiLoaderInstance in _apiLoaderOrder)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;

                await apiLoaderInstance.OnStart(UpdateProgressState);
            }
            
            _onLoadComplete?.Invoke();
        }
        
        private async Task StartupAPI(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            if (StartProcess(baseAPILoaderInstance))
            {
                await AwaitSetupOobabooga(baseAPILoaderInstance);
            }
            else
            {
                _cancellationToken.Cancel();
                _onLoadFailed.Invoke();
            }
        }

        private bool StartProcess(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            string directory = UpdateSlashOnDirectory(baseAPILoaderInstance.DirectoryPath);
            string fileName = UpdateSlashOnFileName(baseAPILoaderInstance.FileName);
            if (System.IO.File.Exists( directory + fileName))
            {
                // Create a new ProcessStartInfo instance
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    WorkingDirectory = directory,
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
            while (!await baseAPILoaderInstance.TryStartup(UpdateProgressState) && !_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TaskDelay);
            }
        }

        private string UpdateSlashOnDirectory(string firstString)
        {
            if (!firstString.EndsWith("\\"))
            {
                return firstString + "\\";
            }
            
            return firstString;
        }
        
        private string UpdateSlashOnFileName(string secondString)
        {
            if (secondString.StartsWith("\\"))
            {
                return secondString.Remove(0);
            }

            return secondString;
        }

        private void UpdateProgressState(string text)
        {
            Debug.Log(text);
            _loadingText.text = text;
        }
    }
}
