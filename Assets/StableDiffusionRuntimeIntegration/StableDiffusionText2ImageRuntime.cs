using System;
using System.IO;
using System.Threading.Tasks;
using Core.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionRuntimeIntegration
{
    public class StableDiffusionText2ImageRuntime : MonoBehaviour
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private ImageSlider _imageSlider;

        [Header("Generation")] 
        [SerializeField] private TMP_InputField _characterNamePromptInputField;
        [SerializeField] private TMP_InputField _contextPromptInputField;
        //[SerializeField] private string _prompt = "";
        [SerializeField] private string _negativePrompt = "";
        [SerializeField, Range(1, 150)] private int _steps = 25;
        [SerializeField, Range(1, 30)] private int _cfgScale = 7;
        [SerializeField] private int _width = 512;
        [SerializeField] private int _height = 512;
        [SerializeField] private int _seed = -1;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;

        private const int TaskDelay = 1000;
        
        [ContextMenu("Generate Txt2Img")]
        public async void GetTxt2Img()
        {
            Task task = InternalGetTxt2Img();
            
            task.GetAwaiter().OnCompleted(FinishProgressBar);
        
            await CheckProgressAsync(task);
        }
    
        private async Task CheckProgressAsync(Task task)
        {
            UpdateProgressString("Waiting ...");
            while (true)
            {
                SDOutProgress progress = await Automatic1111API.GetProgressAsync();
                if (task.IsCompleted) return;
                
                UpdateProgressBar(progress);

                await Task.Delay(TaskDelay);
            }
        }
        
        private void UpdateProgressString(string message)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayProgressBar("Generation in progress", message, 0);
            }
#endif
            if (Application.isPlaying)
            {
                _imageSlider.UpdateSlider(0, message);
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
                _imageSlider.UpdateSlider(progress.progress, $"{info} ETA: {progress.eta_relative:F0}s");
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
                _imageSlider.UpdateSlider(1, "Done");
            }
        }

        private async Task InternalGetTxt2Img()
        {
            SDInTxt2Img inTxt2Img = new SDInTxt2Img
            {
                prompt = _characterNamePromptInputField.text + ":1.4, " + _contextPromptInputField.text,
                negative_prompt = _negativePrompt,
                steps = _steps,
                cfg_scale = _cfgScale,
                width = _width,
                height = _height,
                seed = _seed,
                sampler_name = _sdSamplersVariable.GetCurrent
            };

            SDOutTxt2Img outTxt2Img = await Automatic1111API.PostTextToImage(inTxt2Img);
            bool hasImage = outTxt2Img.images != null && outTxt2Img.images.Count != 0;
            
            if (hasImage)
            {
                foreach (string image in outTxt2Img.images)
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(image);
                        string filePath = Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
                        Debug.Log("Trying to save to: " + filePath);
                    
                        using (FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                        {
                            await sourceStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                        }
                    
                        if (File.Exists(filePath))
                        {
                            Texture2D texture = new Texture2D(2, 2);
                            byte[] fileImage = await File.ReadAllBytesAsync(filePath);
                            texture.LoadImage(fileImage);
                            texture.Apply();
                            
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                            _targetImage.sprite = sprite;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}




