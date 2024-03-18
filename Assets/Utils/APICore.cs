using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public static class APICore
    {
        #region Public Methods
        
        public static async void DispatchRequest<T>(string url, string webMethod, byte[] body, 
            Func<string, List<T>> responseParseMethod, CancellationTokenSource token, Action<(APIResponse, List<T>)> onResponse = null, Action onComplete = null)
        {
            using var request = UnityWebRequest.Put(url, body);
            request.method = webMethod;
            request.SetRequestHeader("Content-Type", "application/json"); //TODO: remove magic variable

            var asyncOperation = request.SendWebRequest();
            
            string previousResponse = string.Empty;
            while (!asyncOperation.isDone && !token.IsCancellationRequested)
            {
                string text = request.downloadHandler.text;
                if (!text.Equals(previousResponse))
                {
                    previousResponse = text;
                    
                    APIResponse response = new APIResponse()
                    {
                        ResponseCode = request.responseCode,
                        Result = request.result
                    };
                    
                    onResponse?.Invoke((response, responseParseMethod(text)));
                }

                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Request Error: {request.responseCode} | {request.error}");
            }
            else
            {
                Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
            }
            
            onComplete?.Invoke();
        }
        
        public static async Task<(APIResponse Response, T Data)> DispatchRequest<T>(string url, string webMethod, byte[] body = null, Func<string, T> responseParseMethod = null)
        {
            using var request = UnityWebRequest.Put(url, body);
            request.method = webMethod;
            request.SetRequestHeader("Content-Type", "application/json"); //TODO: remove magic variable

            await request.SendWebRequest();

            APIResponse response = new APIResponse()
            {
                ResponseCode = request.responseCode,
                Result = request.result
            };

            T data = default;
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Request Error: {request.responseCode} | {request.error} | {request.result}");
            }
            else
            {
                Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                if (responseParseMethod == null)
                {
                    data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                }
                else
                {
                    data = responseParseMethod.Invoke(request.downloadHandler.text);
                }
            }
            
            return (response, data);
        }
        
        public static byte[] CreateBody<T>(T @object)
        {
            string jsonData = JsonConvert.SerializeObject(@object, JsonSerializerSettings);
            return Encoding.UTF8.GetBytes(jsonData);
        }
        
        #endregion
        
        #region Json Settings
        
        private class CustomNamingStrategy : NamingStrategy
        {
            protected override string ResolvePropertyName(string name)
            {
                var result = Regex.Replace(name, "([A-Z])", m => m.Value[0].ToString().ToLowerInvariant());
                return result;
            }
        }
        
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CustomNamingStrategy()
            },
            Culture = CultureInfo.InvariantCulture
        };
        
        #endregion
    }
    
    public class APIResponse
    {
        public bool IsError => Result is UnityWebRequest.Result.ConnectionError 
            or UnityWebRequest.Result.ProtocolError
            or UnityWebRequest.Result.DataProcessingError;
        public bool IsInProgress => Result is UnityWebRequest.Result.InProgress;
        public bool IsValid => Result is UnityWebRequest.Result.Success or UnityWebRequest.Result.InProgress;
        public long ResponseCode { get; set; }
        public UnityWebRequest.Result Result { get; set; }
    }
    
    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
        
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            return new AsyncOperationAwaiter(asyncOp);
        }
    }
    
    //https://gist.github.com/krzys-h/9062552e33dd7bd7fe4a6c12db109a1a
    [DebuggerNonUserCode]
    public readonly struct AsyncOperationAwaiter : INotifyCompletion
    {
        private readonly AsyncOperation _asyncOperation;
        public bool IsCompleted => _asyncOperation.isDone;

        public AsyncOperationAwaiter( AsyncOperation asyncOperation ) => _asyncOperation = asyncOperation;

        public void OnCompleted( Action continuation ) => _asyncOperation.completed += _ => continuation();

        public void GetResult() { }
    }

    [DebuggerNonUserCode]
    public readonly struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public UnityWebRequestAwaiter( UnityWebRequestAsyncOperation asyncOperation ) => _asyncOperation = asyncOperation;

        public void OnCompleted( Action continuation ) => _asyncOperation.completed += _ => continuation();

        public UnityWebRequest GetResult() => _asyncOperation.webRequest;
    }
}