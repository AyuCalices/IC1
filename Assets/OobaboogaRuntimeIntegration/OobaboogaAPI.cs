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
        private static string ServerURL => _hasCustom ? _customServerUrl : DefaultServerURL;

        private const string DefaultServerURL = "http://127.0.0.1:5000";
        private static string _customServerUrl;
        private static bool _hasCustom;

        public static void SetCustomServerUrl(string customServerUrl)
        {
            _hasCustom = true;
            _customServerUrl = customServerUrl;
        }
    
        public static async Task<(APIResponse Response, ModelInfoResponse Data)> GetCurrentModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/info";
            return await APICore.DispatchRequest<ModelInfoResponse>(url, UnityWebRequest.kHttpVerbGET);
        }
    
        public static async Task<(APIResponse Response, ModelListResponse Data)> GetAllModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/list";
            return await APICore.DispatchRequest<ModelListResponse>(url, UnityWebRequest.kHttpVerbGET);
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
        
        public static async Task<APIResponse> UnloadModelAsync()
        {
            string url = $"{ServerURL}/v1/internal/model/unload";
            return (await APICore.DispatchRequest<string>(url, UnityWebRequest.kHttpVerbPOST)).Response;
        }
        
        /// <summary>
        /// The full response is outputted at once, without streaming the words one at a time. Recommended
        /// on high latency networks like running the webui on Google Colab or using --share.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static async Task<(APIResponse Response, List<CompletionResponse> Data)> CreateCompletion(CompletionRequestContainer completionRequestContainer)
        {
            CompletionRequest chatCompletionRequest = new CompletionRequest(false, completionRequestContainer);
            string url = $"{ServerURL}/v1/completions";
            
            return (await APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(chatCompletionRequest), 
                ParseResponse<CompletionResponse>));
        }
        
        /// <summary>
        /// Returns stream, where the words return one at a time.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static void CreateCompletionStream(CompletionRequestContainer completionRequestContainer, CancellationTokenSource token, 
            Action<(APIResponse Response, List<CompletionResponse> Data)> onResponseContent = null, Action onComplete = null)
        {
            CompletionRequest chatCompletionRequest = new CompletionRequest(true, completionRequestContainer);
            string url = $"{ServerURL}/v1/completions";
            
            APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(chatCompletionRequest), 
                ParseResponse<CompletionResponse>, token, onResponseContent, onComplete);
        }
        
        /// <summary>
        /// The full response is outputted at once, without streaming the words one at a time. Recommended
        /// on high latency networks like running the webui on Google Colab or using --share.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static async Task<(APIResponse Response, List<ChatCompletionResponse> Data)> CreateCompletion(ChatCompletionRequestContainer chatCompletionRequestContainer)
        {
            ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest(false, chatCompletionRequestContainer);
            string url = $"{ServerURL}/v1/chat/completions";
            
            return (await APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(chatCompletionRequest), 
                ParseResponse<ChatCompletionResponse>));
        }
        
        /// <summary>
        /// Returns stream, where the words return one at a time.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public static void CreateChatCompletionStream(ChatCompletionRequestContainer chatCompletionRequestContainer, CancellationTokenSource token, 
            Action<(APIResponse Response, List<ChatCompletionResponse> Data)> onResponseContent = null, Action onComplete = null)
        {
            ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest(true, chatCompletionRequestContainer);
            string url = $"{ServerURL}/v1/chat/completions";
            
            APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(chatCompletionRequest), 
                ParseResponse<ChatCompletionResponse>, token, onResponseContent, onComplete);
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
