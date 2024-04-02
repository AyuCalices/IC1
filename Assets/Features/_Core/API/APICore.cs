using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace Features._Core.API
{
    public static class APICore
    {
        #region Public Methods
        
        public static async void DispatchRequest<T>(string url, string webMethod, byte[] body, 
            Func<string, List<T>> responseParseMethod, CancellationTokenSource token, Action<(APIResponse, List<T>)> onResponse = null, Action<APIResponse> onComplete = null)
        {
            using var request = UnityWebRequest.Put(url, body);
            request.method = webMethod;
            request.SetRequestHeader("Content-Type", "application/json");

            var asyncOperation = request.SendWebRequest();
            APIResponse response = new APIResponse();
            
            string previousResponse = string.Empty;
            while (!asyncOperation.isDone && !token.IsCancellationRequested)
            {
                string text = request.downloadHandler.text;
                if (!text.Equals(previousResponse))
                {
                    previousResponse = text;

                    response.ResponseCode = request.responseCode;
                    response.Result = request.result;

                    try
                    {
                        List<T> parsedContent = responseParseMethod(text);
                        onResponse?.Invoke((response, parsedContent));
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Couldn't parse the Response text. Error: " + e);
                    }
                }

                await Task.Yield();
            }

            response.ResponseCode = request.responseCode;
            response.Result = request.result;
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                response.Error = request.error;
                Debug.LogWarning($"Request Error: {request.responseCode} | {request.error}");
            }
            else
            {
                Debug.Log($"Finished request to {request.url} with result {request.responseCode} {request.result}");
            }
            
            onComplete?.Invoke(response);
        }
        
        public static async Task<(APIResponse Response, T Data)> DispatchRequest<T>(string url, string webMethod, byte[] body = null, Func<string, T> responseParseMethod = null)
        {
            using var request = UnityWebRequest.Put(url, body);
            request.method = webMethod;
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            APIResponse response = new APIResponse()
            {
                ResponseCode = request.responseCode,
                Result = request.result
            };
            T data = default;
            
            try
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    response.Error = request.error;
                    Debug.LogWarning($"Request Error to {request.url}: {request.responseCode} | {request.error} | {request.result}");
                }
                else
                {
                    Debug.Log($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                    if (responseParseMethod == null)
                    {
                        data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    }
                    else
                    {
                        data = responseParseMethod.Invoke(request.downloadHandler.text);
                    }
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = 500;
                response.Result = UnityWebRequest.Result.DataProcessingError;
                response.Error = UnityWebRequest.Result.DataProcessingError.ToString();
                Debug.LogWarning(e);
            }
            
            return (response, data);
        }
        
        public static byte[] CreateBody<T>(T @object)
        {
            string jsonData = JsonConvert.SerializeObject(@object, JsonSerializerSettings);
            return Encoding.UTF8.GetBytes(jsonData);
        }
        
        public static void CopyProperties(object source, object destination)
        {
            if (source == null) return;
            
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] destinationProperties = destinationType.GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = destinationProperties.FirstOrDefault(
                    p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    object value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
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
        public string Error { get; set; }
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
