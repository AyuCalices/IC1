using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        [SerializeField] private List<BaseAPILoaderInstance> _apiLoaderOrder;
        
        [Header("Timeout Text")]
        [SerializeField] private float _timeoutInSeconds = 100;
        [SerializeField] private TMP_Text _timeoutText;
        
        [Header("Visualization")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onLoadComplete;
        [SerializeField] private UnityEvent<string> _onLoadFailed;
    
        private const int TaskDelay = 1000;
    
        private readonly CancellationTokenSource _cancellationToken = new();
        private bool _currentlyOpening;
        private float _timeoutDelta;

        private void Start()
        {
            _loadingText.text = string.Empty;
            _timeoutText.gameObject.SetActive(_timeoutDelta > _timeoutInSeconds);
        }

        private void Update()
        {
            _timeoutText.gameObject.SetActive(_timeoutDelta > _timeoutInSeconds);
            
            if (!_currentlyOpening) return;
            
            _timeoutDelta += Time.deltaTime;
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }

        public async void LoadAPIInstances()
        {
            UpdateProgressState("Initialize ...");
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
            _currentlyOpening = true;
            
            if (StartProcess(baseAPILoaderInstance))
            {
                _timeoutDelta = 0f;
                await AwaitSetupOobabooga(baseAPILoaderInstance);
            }

            _timeoutDelta = 0f;
            _currentlyOpening = false;
        }

        private bool StartProcess(BaseAPILoaderInstance baseAPILoaderInstance)
        {
            string directory = UpdateSlashOnDirectory(baseAPILoaderInstance.DirectoryPath);
            string fileName = UpdateSlashOnFileName(baseAPILoaderInstance.FileName);
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

        //TODO: update slash
        private string UpdateSlashOnDirectory(string firstString)
        {
            if (!firstString.EndsWith("\\"))
            {
                return firstString + "\\";
            }
            
            return firstString;
        }
        
        //TODO: update slash
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
