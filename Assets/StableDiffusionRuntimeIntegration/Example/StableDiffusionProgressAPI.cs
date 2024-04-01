using System;
using System.Threading;
using System.Threading.Tasks;
using DataStructures.Variables;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionProgressAPI : MonoBehaviour
    {
        [Header("Request")]
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;

        [Header("Slider")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderText;
        
        private const int TaskDelay = 1000;
        private bool _startedUpdateProgressBar;
        private CancellationTokenSource _cancellationToken;

        private void OnDisable()
        {
            FinishProgressBar();
        }

        private void OnDestroy()
        {
            FinishProgressBar();
        }

        public async void UpdateRequestProgressAsync(Task task)
        {
            _startedUpdateProgressBar = true;
            _cancellationToken = new CancellationTokenSource();
            
            InitializeProgressbar("Waiting ...");
            while (!_cancellationToken.IsCancellationRequested)
            {
                var content = await _stableDiffusionAPIVariable.Get().GetProgressAsync();
                if (content.Response.IsError) break;
                    
                SDOutProgress progress = content.Data;
                if (task.IsCompleted) return;
                
                UpdateProgressBar(progress);

                await Task.Delay(TaskDelay);
            }
        }
        
        public void FinishProgressBar()
        {
            if (!_startedUpdateProgressBar) return;
            
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.ClearProgressBar();
            }
#endif
            if (Application.isPlaying)
            {
                UpdateSlider(1, "Done");
            }

            _cancellationToken.Cancel();
            _startedUpdateProgressBar = false;
        }
        
        private void InitializeProgressbar(string message)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayProgressBar("Generation in progress", message, 0);
            }
#endif
            if (Application.isPlaying)
            {
                UpdateSlider(0, message);
            }
        }

        private void UpdateProgressBar(SDOutProgress progress)
        {
            string info = (progress.progress * 100).ToString("F0") + "%";
            
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayProgressBar("Generation in progress", info, progress.progress);
            }
#endif
            if (Application.isPlaying)
            {
                UpdateSlider(progress.progress, $"{info} ETA: {progress.eta_relative:F0}s");
            }
        }

        private void UpdateSlider(float percent, string message)
        {
            _sliderText.text = message;
            _slider.value = percent;
        }
    }
}
