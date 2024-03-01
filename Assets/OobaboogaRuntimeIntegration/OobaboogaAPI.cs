using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI;
using StableDiffusionRuntimeIntegration;
using UnityEngine;
using UnityEngine.Networking;

namespace OobaboogaRuntimeIntegration
{
    public class OobaboogaAPI : MonoBehaviour
    {
        private const string StableDiffusionServerURL = "http://127.0.0.1:5000";
        
        private const string CurrentModelAPI = "/v1/internal/model/info";
        private const string AllModelAPI = "/v1/internal/model/list";
        private const string LoadModelAPI = "/v1/internal/model/load";
        private const string UnloadModelAPI = "/v1/internal/model/unload";
        
        private const string CompletionsAPI = "/v1/completions";
    
        public static async Task<ModelInfoResponse> GetCurrentModelAsync()
        {
            const string url = StableDiffusionServerURL + CurrentModelAPI;

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
                    
                        return JsonConvert.DeserializeObject<ModelInfoResponse>(request.downloadHandler.text);
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
    
        public static async Task<ModelListResponse> GetAllModelAsync()
        {
            const string url = StableDiffusionServerURL + AllModelAPI;

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
                    
                        return JsonConvert.DeserializeObject<ModelListResponse>(request.downloadHandler.text);
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
    
        public static async Task LoadModelAsync(string modelName)
        {
            const string url = StableDiffusionServerURL + LoadModelAPI;

            try
            {
                // Create request object
                using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                {
                    // Serialize data and set request properties
                    string jsonData = JsonConvert.SerializeObject(new LoadModelRequest { model_name = modelName });
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
        
        public static async Task<bool> UnloadModelAsync()
        {
            const string url = StableDiffusionServerURL + UnloadModelAPI;

            try
            {
                // Create request object
                using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    // Perform the request asynchronously
                    await request.SendWebRequest();

                    // Check for errors
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Request Error: " + request.error);
                        return false;
                    }
                    else
                    {
                        // Get the response from the server
                        string result = request.downloadHandler.text;
                        Debug.LogWarning($"Finished request to {request.url} with result {request.responseCode} {result}");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Request Error: " + ex.Message);
                throw;
            }
        }
        
        public static async Task<CompletionResponse> OpenaiCompletions(GeneratedSettings generatedSetting)
        {
            const string url = StableDiffusionServerURL + CompletionsAPI;

            try
            {
                using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                {
                    string jsonData = JsonConvert.SerializeObject(generatedSetting);
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
                        
                        return JsonConvert.DeserializeObject<CompletionResponse>(request.downloadHandler.text);
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
        
        public static async Task<CompletionResponse> StreamCompletion(GeneratedSettings generatedSetting)
        {
            const string url = StableDiffusionServerURL + CompletionsAPI;

            using (HttpClient httpClient = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(generatedSetting);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await httpClient.PostAsync(url, content))
                {
                    response.EnsureSuccessStatusCode();
                    
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = await reader.ReadLineAsync();
                            Debug.Log(line);
                        
                            // Process the line (chunk) from the delta field
                            // Example: Extract relevant information and print it
                            //return JsonConvert.DeserializeObject<CompletionResponse>(line);
                        }
                    }
                }
            }

            return null;
        }
        
        public static async Task DispatchRequest(GeneratedSettings generatedSetting, Action<List<CompletionResponse>> onResponse, Action<List<CompletionResponse>> onComplete)
        {
            const string url = StableDiffusionServerURL + CompletionsAPI;
            
            string jsonData = JsonConvert.SerializeObject(generatedSetting);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            using (var request = UnityWebRequest.Put(url, bodyRaw))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.SetRequestHeader("Content-Type", "application/json");
                
                var asyncOperation = request.SendWebRequest();

                string previousResponse = String.Empty;
                
                List<CompletionResponse> completionResponses = new List<CompletionResponse>();

                while (!asyncOperation.isDone)
                {
                    if (request.downloadHandler.text.Length != previousResponse.Length)
                    {
                        completionResponses.Clear();
                        previousResponse = request.downloadHandler.text;
                        
                        string[] lines = request.downloadHandler.text.Split('\n')
                            .Select(line => line.Trim())
                            .Where(line => !string.IsNullOrEmpty(line))
                            .ToArray();
                        
                        foreach (string line in lines)
                        {
                            var value = line.Replace("data: ", "");
                            var data = JsonConvert.DeserializeObject<CompletionResponse>(value);
                            completionResponses.Add(data);
                        }
                        
                        onResponse.Invoke(completionResponses);
                        Debug.Log("OnResponse");
                    }
                    
                    await Task.Yield();
                }
                
                onComplete.Invoke(completionResponses);
                Debug.Log("OnComplete");
            }
        }
    }
    
    [Serializable]
    public class ModelInfoResponse
    {
        public string model_name;
        public List<string> lora_names;
    }
    
    [Serializable]
    public class ModelListResponse
    {
        public List<string> model_names;
    }
    
    [Serializable]
    public class LoadModelRequest
    {
        public string model_name;
        public Dictionary<string, object> args;
        public Dictionary<string, object> settings;
    }
    
    [Serializable]
    public class GeneratedSettings
    {
        public string model = string.Empty;
        public string prompt = string.Empty;
        public int best_of = 1;
        public bool echo;
        public int frequency_penalty;
        public Dictionary<string, object> logit_bias = new();
        public int logprobs;
        public int max_tokens = 512;
        public int n = 1;
        public int presence_penalty;
        public string[] stop = Array.Empty<string>();
        public bool stream = true;
        public string suffix = string.Empty;
        public float temperature = 1;
        public float top_p = 1;
        public string user = string.Empty;
        public string preset = string.Empty;
        public float min_p;
        public bool dynamic_temperature;
        public float dynatemp_low = 1;
        public float dynatemp_high = 1;
        public float dynatemp_exponent = 1;
        public float smoothing_factor;
        public int top_k;
        public float repetition_penalty = 1;
        public int repetition_penalty_range = 1024;
        public float typical_p = 1;
        public float tfs = 1;
        public int top_a;
        public float epsilon_cutoff;
        public float eta_cutoff;
        public float guidance_scale = 1;
        public string negative_prompt = string.Empty;
        public float penalty_alpha;
        public int mirostat_mode;
        public float mirostat_tau = 5;
        public float mirostat_eta = 0.1f;
        public bool temperature_last;
        public bool do_sample = true;
        public int seed = -1;
        public float encoder_repetition_penalty = 1;
        public int no_repeat_ngram_size;
        public int min_length;
        public int num_beams = 1;
        public float length_penalty = 1;
        public bool early_stopping;
        public int truncation_length;
        public int max_tokens_second;
        public int prompt_lookup_num_tokens;
        public string custom_token_bans = string.Empty;
        public string[] sampler_priority = Array.Empty<string>();
        public bool auto_max_new_tokens;
        public bool ban_eos_token;
        public bool add_bos_token = true;
        public bool skip_special_tokens = true;
        public string grammar_string = string.Empty;
    }
    
    public class CompletionResponse
    {
        public string id = string.Empty;
        public List<Choice> choices = new();
        public long created = 1709214771;
        public string model = string.Empty;
        public string @object = "text_completion";
        public Dictionary<string, object> usage = new();
    }

    public struct Choice
    {
        public long Index { get; set; }
        public object Finish_Reason { get; set; }
        public string Text { get; set; }
        public JObject Logprobs { get; set; }
    }
    
    public class ChatCompletionSchema
    {
        public List<object> messages = new();
        public string model = "string";
        public int frequency_penalty = 0;
        public string function_call = "string";
        public List<object> functions = new List<object>();
        public Dictionary<string, object> logit_bias = new Dictionary<string, object>();
        public int max_tokens = 0;
        public int n = 1;
        public int presence_penalty = 0;
        public List<string> stop = new List<string>();
        public bool stream = false;
        public int temperature = 1;
        public int top_p = 1;
        public string user = "string";
        public string mode = "instruct";
        public string instruction_template = "string";
        public string instruction_template_str = "string";
        public string character = "string";
        public string name1 = "string";
        public string name2 = "string";
        public string context = "string";
        public string greeting = "string";
        public string chat_template_str = "string";
        public string chat_instruct_command = "string";
        public bool continue_ = false;
        public string preset = "string";
        public int min_p = 0;
        public bool dynamic_temperature = false;
        public int dynatemp_low = 1;
        public int dynatemp_high = 1;
        public int dynatemp_exponent = 1;
        public int smoothing_factor = 0;
        public int top_k = 0;
        public int repetition_penalty = 1;
        public int repetition_penalty_range = 1024;
        public int typical_p = 1;
        public int tfs = 1;
        public int top_a = 0;
        public int epsilon_cutoff = 0;
        public int eta_cutoff = 0;
        public int guidance_scale = 1;
        public string negative_prompt = "";
        public int penalty_alpha = 0;
        public int mirostat_mode = 0;
        public int mirostat_tau = 5;
        public double mirostat_eta = 0.1;
        public bool temperature_last = false;
        public bool do_sample = true;
        public int seed = -1;
        public int encoder_repetition_penalty = 1;
        public int no_repeat_ngram_size = 0;
        public int min_length = 0;
        public int num_beams = 1;
        public int length_penalty = 1;
        public bool early_stopping = false;
        public int truncation_length = 0;
        public int max_tokens_second = 0;
        public int prompt_lookup_num_tokens = 0;
        public string custom_token_bans = "";
        public List<string> sampler_priority = new List<string>();
        public bool auto_max_new_tokens = false;
        public bool ban_eos_token = false;
        public bool add_bos_token = true;
        public bool skip_special_tokens = true;
        public string grammar_string = "";
    }
}
