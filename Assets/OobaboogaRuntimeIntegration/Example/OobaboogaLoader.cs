using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace OobaboogaRuntimeIntegration.Example
{
    public class OobaboogaLoader : MonoBehaviour
    {
        [SerializeField] private string _startupPath;
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;
        
        private const int TaskDelay = 1000;

        private readonly CancellationTokenSource _cancellationToken = new();
        
        private async void Start()
        {
            if ((await _oobaboogaModelsVariable.SetupAllModelsAsync()).Result != UnityWebRequest.Result.Success)
            {
                await StartupAPI();
            }
            
            if (_oobaboogaModelsVariable.CurrentModelIndex >= _oobaboogaModelsVariable.ModelList.model_names.Count)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            string currentModel = (await OobaboogaAPI.GetCurrentModelAsync()).Data.model_name;
            if (currentModel == _oobaboogaModelsVariable.ModelList.model_names[_oobaboogaModelsVariable.CurrentModelIndex])
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            await _oobaboogaModelsVariable.LoadModelAsync();
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }
        
        private async Task StartupAPI()
        {
            if (StartOobaboogaProcess())
            {
                await CheckProgressAsync();
            }
        }

        private bool StartOobaboogaProcess()
        {
            if (System.IO.File.Exists(_startupPath))
            {
                Process.Start(_startupPath);
                return true;
            }

            Debug.LogError("File does not exist at path: " + _startupPath);
            return false;
        }
        
        private async Task CheckProgressAsync()
        {
            while ((await _oobaboogaModelsVariable.SetupAllModelsAsync()).Result != UnityWebRequest.Result.Success && !_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TaskDelay);
            }
        }
    }
}
