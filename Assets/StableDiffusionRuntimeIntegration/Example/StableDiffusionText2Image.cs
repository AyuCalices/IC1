using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataStructures.Variables;
using StableDiffusionRuntimeIntegration.SDConfig;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionText2Image : MonoBehaviour
    {
        [SerializeField] private List<Text2ImageTaskCallback> _baseTaskCallbacks;
        
        [Header("API")]
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;

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

        [ContextMenu("Generate Txt2Img")]
        public async void GetTxt2Img()
        {
            Task task = InternalGetTxt2Img();

            foreach (Text2ImageTaskCallback baseTaskCallback in _baseTaskCallbacks)
            {
                baseTaskCallback.OnPerformTaskCallback(task);
            }
        }

        //TODO: show text on error
        private async Task InternalGetTxt2Img()
        {
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

            var content = await _stableDiffusionAPIVariable.Get().PostTextToImage(inTxt2Img);
            if (content.Response.IsError) return;

            SDOutTxt2Img outTxt2Img = content.Data;
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

                        await using (FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                        {
                            await sourceStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                        }
                        
                        foreach (Text2ImageTaskCallback text2ImageTaskCallback in _baseTaskCallbacks)
                        {
                            text2ImageTaskCallback.OnTaskCompletedCallback(filePath);
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




