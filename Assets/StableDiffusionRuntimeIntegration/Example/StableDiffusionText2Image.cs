using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStructures.Variables;
using StableDiffusionRuntimeIntegration.SDConfig;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionText2Image : MonoBehaviour
    {
        [Header("API")]
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;
        [SerializeField] private TMP_Text _errorMessage;

        [Header("Generation")] 
        [SerializeField] private TMP_InputField _characterNamePromptInputField;
        [SerializeField] private TMP_InputField _contextPromptInputField;
        [SerializeField] private string _additionalPrompt = "";
        [SerializeField] private string _negativePrompt = "";
        [SerializeField, Range(1, 150)] private int _steps = 25;
        [SerializeField, Range(1, 30)] private int _cfgScale = 7;
        [SerializeField] private int _width = 512;
        [SerializeField] private int _height = 512;
        [SerializeField] private int _seed = -1;

        [Header("Events")] 
        [SerializeField] private UnityEvent<Task> _onStartGenerating;
        [SerializeField] private UnityEvent<string> _onGeneratingSuccessful;
        [SerializeField] private UnityEvent<string> _onGeneratingFailed;

        private static bool _currentlyGenerating;
        private CancellationTokenSource _cancellationToken;

        private void Awake()
        { 
            _errorMessage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_currentlyGenerating)
            {
                _cancellationToken.Cancel();
            }
        }

        [ContextMenu("Generate Txt2Img")]
        public async void GetTxt2Img()
        {
            Task task = InternalGetTxt2Img();
            
            _onStartGenerating?.Invoke(task);
        }

        private async Task InternalGetTxt2Img()
        {
            _currentlyGenerating = true;
            _cancellationToken = new CancellationTokenSource();
            
            _errorMessage.gameObject.SetActive(false);
            SDInTxt2Img inTxt2Img = new SDInTxt2Img
            {
                prompt = _characterNamePromptInputField.text + ":1.4, " + _contextPromptInputField.text + _additionalPrompt,
                negative_prompt = _negativePrompt,
                steps = _steps,
                cfg_scale = _cfgScale,
                width = _width,
                height = _height,
                seed = _seed,
                sampler_name = _sdSamplersVariable.GetCurrent
            };

            (APIResponse Response, SDOutTxt2Img Data) content = await _stableDiffusionAPIVariable.Get().PostTextToImage(inTxt2Img);
            if (content.Response.IsError)
            {
                _errorMessage.gameObject.SetActive(true);
                string errorMessage = $"An error occured while generating the chat! Error: {content.Response.ResponseCode} {content.Response.Error}";
                _errorMessage.text = errorMessage;
                _onGeneratingFailed?.Invoke(errorMessage);
                return;
            }

            SDOutTxt2Img outTxt2Img = content.Data;
            bool hasImage = outTxt2Img.images != null && outTxt2Img.images.Count != 0;
            
            if (hasImage)
            {
                foreach (string image in outTxt2Img.images)
                {
                    try
                    {
                        if (_cancellationToken.IsCancellationRequested) return;
                        
                        byte[] imageBytes = Convert.FromBase64String(image);
                        string filePath = Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
                        Debug.Log("Trying to save to: " + filePath);

                        await using (FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                        {
                            await sourceStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                        }
                        
                        _onGeneratingSuccessful?.Invoke(filePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.Message);
                        _onGeneratingFailed?.Invoke("ex.Message");
                        throw;
                    }
                }
            }
            else
            {
                _onGeneratingFailed?.Invoke("The response didn't return an image.");
            }
            
            _currentlyGenerating = false;
        }
    }
}




