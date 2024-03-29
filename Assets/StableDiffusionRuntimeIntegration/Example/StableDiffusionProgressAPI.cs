using System.Threading.Tasks;
using DataStructures.Variables;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionProgressAPI : Text2ImageTaskCallback
    {
        [Header("Request")]
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;

        [Header("Slider")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderText;
        
        private const int TaskDelay = 1000;
        
        public override async void OnPerformTaskCallback(Task task)
        {
            await RequestProgressAsync(task);
        }

        public override void OnTaskCompletedCallback(string imagePath)
        {
            FinishProgressBar();
        }

        private async Task RequestProgressAsync(Task task)
        {
            InitializeProgressbar("Waiting ...");
            while (true)
            {
                var content = await _stableDiffusionAPIVariable.Get().GetProgressAsync();
                if (content.Response.IsError) break;
                    
                SDOutProgress progress = content.Data;
                if (task.IsCompleted) return;
                
                UpdateProgressBar(progress);

                await Task.Delay(TaskDelay);
            }
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
        
        private void FinishProgressBar()
        {
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
        }

        private void UpdateSlider(float percent, string message)
        {
            _sliderText.text = message;
            _slider.value = percent;
        }
    }
}
