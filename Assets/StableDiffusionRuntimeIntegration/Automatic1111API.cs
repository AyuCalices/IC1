using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace StableDiffusionRuntimeIntegration
{
    public static class Automatic1111API
    {
        private const string StableDiffusionServerURL = "http://127.0.0.1:7860";
        
        private const string ModelsAPI = "/sdapi/v1/sd-models";
        private const string SamplersAPI = "/sdapi/v1/samplers";
        private const string OptionAPI = "/sdapi/v1/options";
        private const string ProgressAPI = "/sdapi/v1/progress";
        private const string TextToImageAPI = "/sdapi/v1/txt2img";
        
        public static async Task<SDOutSampler[]> GetSamplersAsync()
        {
            const string url = StableDiffusionServerURL + SamplersAPI;

            try
            {
                using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
            
                    await request.SendWebRequest();
                
                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Request Error: {request.responseCode} | {request.error}");
                    }
                    else
                    {
                        // Get the response from the server
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                    
                        return JsonConvert.DeserializeObject<SDOutSampler[]>(request.downloadHandler.text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Request Error: " + ex.Message);
                throw;
            }
            
            return null;
        }
        
        public static async Task<SDOutModel[]> GetModelsAsync()
        {
            const string url = StableDiffusionServerURL + ModelsAPI;

            try
            {
                using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
            
                    await request.SendWebRequest();
                
                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Request Error: {request.responseCode} | {request.error}");
                    }
                    else
                    {
                        // Get the response from the server
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                    
                        return JsonConvert.DeserializeObject<SDOutModel[]>(request.downloadHandler.text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Request Error: " + ex.Message);
                throw;
            }
            
            return null;
        }
        
        public static async Task PostOptionsModelCheckpointAsync(string modelName)
        {
            const string url = StableDiffusionServerURL + OptionAPI;

            try
            {
                // Create request object
                using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                {
                    // Serialize data and set request properties
                    string jsonData = JsonConvert.SerializeObject(new SDInOptionsModelCheckpoint { sd_model_checkpoint = modelName });
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    // Perform the request asynchronously
                    await request.SendWebRequest();

                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Request Error: " + request.error);
                    }
                    else
                    {
                        // Get the response from the server
                        string result = request.downloadHandler.text;
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Request Error: " + ex.Message);
                throw;
            }
        }
        
        public static async Task<string> GetSDCheckpointSha256Async()
        {
            const string url = StableDiffusionServerURL + OptionAPI;

            try
            {
                // Create request object
                using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    // Perform the request asynchronously
                    await request.SendWebRequest();

                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Request Error: {request.responseCode} | {request.error}");
                    }
                    else
                    {
                        // Get the response from the server
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                    
                        JObject obj = JObject.Parse(request.downloadHandler.text);
                        JToken currentModelSha256 = obj["sd_checkpoint_hash"];
                        if (currentModelSha256 != null)
                        {
                            return currentModelSha256.Value<string>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Request Error: " + ex.Message);
                throw;
            }

            return null;
        }
        
        public static async Task<SDOutProgress> GetProgressAsync()
        {
            const string url = StableDiffusionServerURL + ProgressAPI;

            try
            {
                using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
                    
                    // Send the request and wait for a response
                    await request.SendWebRequest();
                    
                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Request Error: {request.responseCode} | {request.error}");
                    }
                    else
                    {
                        SDOutProgress sdp = JsonConvert.DeserializeObject<SDOutProgress>(request.downloadHandler.text);
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result} " +
                                         $"Generation in progress {sdp.progress * 100:F1}% ({sdp.progress} - {sdp.eta_relative})");

                        return sdp;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Request Error: {ex.Message}");
                throw;
            }

            return null;
        }

        public static async Task<SDOutTxt2Img> PostTextToImage(SDInTxt2Img inTxt2Img)
        {
            const string url = StableDiffusionServerURL + TextToImageAPI;

            try
            {
                using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                {
                    string jsonData = JsonConvert.SerializeObject(inTxt2Img);
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
                    
                    // Send the request and wait for a response
                    await request.SendWebRequest();
                    
                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Request Error: {request.responseCode} | {request.error}");
                    }
                    else
                    {
                        // Get the response from the server
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {request.result}");
                        
                        return JsonConvert.DeserializeObject<SDOutTxt2Img>(request.downloadHandler.text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Request Error: {ex.Message}");
                throw;
            }

            return null;
        }
    }
    
    [Serializable]
    public class SDOutProgress
    {
        public float progress;
        public float eta_relative;
        public Dictionary<string, object> state;
        public string current_image;
        public string textinfo;
    }
    
    [Serializable]
    public class SDOutSampler
    {
        public string name;
        public List<string> aliases;
        public Dictionary<string, string> options;
    }
    
    [Serializable]
    public class SDOutModel
    {
        public string title;
        public string model_name;
        public string hash;
        public string sha256;
        public string filename;
        public string config;
    }
    
    [Serializable]
    public class SDOutTxt2Img
    {
        public List<string> images;
        public Dictionary<string, object> parameters;
        public string info;
    }
    
    [Serializable]
    public class SDInTxt2Img
    {
        [UsedImplicitly] public string prompt = string.Empty;
        [UsedImplicitly] public string negative_prompt = string.Empty;
        //public List<string> styles = new();
        [UsedImplicitly] public int seed = -1;
        //public int subseed = -1;
        //public int subseed_strength;
        //public int seed_resize_from_h = -1;
        //public int seed_resize_from_w = -1;
        [UsedImplicitly] public string sampler_name = string.Empty;
        //public int batch_size = 1;
        //public int n_iter = 1;
        [UsedImplicitly] public int steps = 50;
        [UsedImplicitly] public int cfg_scale = 7;
        [UsedImplicitly] public int width = 512;
        [UsedImplicitly] public int height = 512;
        //public bool restore_faces = true;
        //public bool tiling = true;
        //public bool do_not_save_samples;
        //public bool do_not_save_grid;
        //public double eta;
        //public double denoising_strength;
        //public double s_min_uncond;
        //public double s_churn;
        //public double s_tmax;
        //public double s_tmin;
        //public double s_noise;
        //public Dictionary<string, object> override_settings = new();
        //public bool override_settings_restore_afterwards = true;
        //public string refiner_checkpoint = string.Empty;
        //public int refiner_switch_at;
        //public bool disable_extra_networks;
        //public Dictionary<string, object> comments = new();
        //public bool enable_hr;
        //public int firstphase_width;
        //public int firstphase_height;
        //public int hr_scale = 2;
        //public string hr_upscaler = string.Empty;
        //public int hr_second_pass_steps;
        //public int hr_resize_x;
        //public int hr_resize_y;
        //public string hr_checkpoint_name = string.Empty;
        //public string hr_sampler_name = string.Empty;
        //public string hr_prompt = string.Empty;
        //public string hr_negative_prompt = string.Empty;
        //public string sampler_index = "Euler";
        //public string script_name = string.Empty;
        //public List<object> script_args = new();
        //public bool send_images = true;
        //public bool save_images;
        //public Dictionary<string, object> alwayson_scripts = new();
    }
    
    [Serializable]
    public class SDInOptionsModelCheckpoint
    {
        [UsedImplicitly] public string sd_model_checkpoint = "";
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
