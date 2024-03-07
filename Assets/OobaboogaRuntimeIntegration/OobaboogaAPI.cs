using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace OobaboogaRuntimeIntegration
{
    public class OobaboogaAPI : MonoBehaviour
    {
        private const string ServerURL = "http://127.0.0.1:5000";
    
        public static async Task<ModelInfoResponse> GetCurrentModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/info";
            return (await APICore.DispatchRequest<ModelInfoResponse>(url, UnityWebRequest.kHttpVerbGET)).Data;
        }
    
        public static async Task<ModelListResponse> GetAllModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/list";
            return (await APICore.DispatchRequest<ModelListResponse>(url, UnityWebRequest.kHttpVerbGET)).Data;
        }
    
        public static async Task LoadModelAsync(string modelName)
        {
            string url = $"{ServerURL}/v1/internal/model/load";
            LoadModelRequest loadModelRequest = new LoadModelRequest()
            {
                model_name = modelName
            };
            await APICore.DispatchRequest<string>(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(loadModelRequest));
        }
        
        public static async Task<UnityWebRequest.Result> UnloadModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/unload";
            return (await APICore.DispatchRequest<string>(url, UnityWebRequest.kHttpVerbPOST)).Result;
        }
        
        /// <summary>
        /// The full response is outputted at once, without streaming the words one at a time. Recommended
        /// on high latency networks like running the webui on Google Colab or using --share.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static async Task<APIResponse<List<CompletionResponse>>> CreateCompletion(CompletionRequest completionRequest)
        {
            completionRequest.Stream = false;
            
            string url = $"{ServerURL}/v1/completions";
            return (await APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(completionRequest), 
                ParseResponse<CompletionResponse>));
        }
        
        /// <summary>
        /// Returns stream, where the words return one at a time.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static void CreateCompletionStream(CompletionRequest completionRequest, CancellationTokenSource token, 
            Action<APIResponse<List<CompletionResponse>>> onResponse = null, Action onComplete = null)
        {
            completionRequest.Stream = true;
            
            string url = $"{ServerURL}/v1/completions";
            APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(completionRequest), 
                ParseResponse<CompletionResponse>, token, onResponse, onComplete);
        }
        
        /// <summary>
        /// The full response is outputted at once, without streaming the words one at a time. Recommended
        /// on high latency networks like running the webui on Google Colab or using --share.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static async Task<APIResponse<List<ChatCompletionResponse>>> CreateCompletion(ChatCompletionRequest completionRequest)
        {
            completionRequest.Stream = false;
            
            string url = $"{ServerURL}/v1/chat/completions";
            return (await APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(completionRequest), 
                ParseResponse<ChatCompletionResponse>));
        }
        
        /// <summary>
        /// Returns stream, where the words return one at a time.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static void CreateChatCompletionStream(ChatCompletionRequest completionRequest, CancellationTokenSource token, 
            Action<APIResponse<List<ChatCompletionResponse>>> onResponse = null, Action onComplete = null)
        {
            completionRequest.Stream = true;
            
            string url = $"{ServerURL}/v1/chat/completions";
            APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(completionRequest), 
                ParseResponse<ChatCompletionResponse>, token, onResponse, onComplete);
        }
        
        private static List<T> ParseResponse<T>(string responseText)
        {
            return responseText.Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Replace("data: ", ""))
                .Select(JsonConvert.DeserializeObject<T>)
                .ToList();
        }
    }
}
