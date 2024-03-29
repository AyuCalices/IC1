using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Features.Connection.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public class APILoader : MonoBehaviour
    {
        [SerializeField] private List<BaseAPILoaderInstance> _apiLoaderOrder;
        
        [Header("Timeout Text")]
        [SerializeField] private float _timeoutInSeconds = 100;
        [SerializeField] private GameObject _timeoutContainer;
        
        [Header("Visualization")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onLoadComplete;
        [SerializeField] private UnityEvent<string> _onLoadFailed;
    
        private const int TaskDelay = 1000;
    
        private CancellationTokenSource _cancellationToken;
        private bool _currentlyStartingAPI;
        private float _timeoutDelta;

        private void OnEnable()
        {
            _loadingText.text = string.Empty;
            _timeoutContainer.SetActive(false);
            _cancellationToken = new();
        }

        private void Update()
        {
            _timeoutContainer.SetActive(_timeoutDelta > _timeoutInSeconds);
            
            if (!_currentlyStartingAPI) return;
            
            _timeoutDelta += Time.deltaTime;
        }

        private void OnDisable()
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
                    string errorMessage = $"Server on URL {apiLoaderInstance.URL} is currently not Online!";
                    Debug.LogWarning(errorMessage);
                    _onLoadFailed.Invoke(errorMessage);
                    return;
                }
            }
            
            foreach (BaseAPILoaderInstance apiLoaderInstance in instancesToStartAPI)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;
                
                await StartupAPI(apiLoaderInstance);
            }
            
            _onLoadComplete?.Invoke();
        }
        
        private async Task StartupAPI(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            _currentlyStartingAPI = true;
            
            if (StartProcess(baseAPILoaderInstance))
            {
                _timeoutDelta = 0f;
                await AwaitSetupOobabooga(baseAPILoaderInstance);
            }

            _timeoutDelta = 0f;
            _currentlyStartingAPI = false;
        }

        private bool StartProcess(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            string directory = baseAPILoaderInstance.DirectoryPath.Trim().AddIfLastNotSlash();
            string fileName = baseAPILoaderInstance.FileName.Trim().RemoveIfFirstSlash();
            if (File.Exists( directory + fileName))
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
            
            _cancellationToken.Cancel();
            Debug.LogWarning("File does not exist at path: " + baseAPILoaderInstance.DirectoryPath);
            _onLoadFailed.Invoke("File does not exist at path: " + baseAPILoaderInstance.DirectoryPath);
            return false;
        }
        
        private async Task AwaitSetupOobabooga(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            while (!await baseAPILoaderInstance.TryStartup(UpdateProgressState) && !_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TaskDelay);
            }
        }

        private void UpdateProgressState(string text)
        {
            Debug.Log(text);
            _loadingText.text = text;
        }
    }
}
