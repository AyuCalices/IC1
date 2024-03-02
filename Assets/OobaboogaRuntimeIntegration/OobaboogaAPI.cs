using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using StableDiffusionRuntimeIntegration;
using UnityEngine;
using UnityEngine.Networking;

namespace OobaboogaRuntimeIntegration
{
    public class CustomNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            var result = Regex.Replace(name, "([A-Z])", m => m.Value[0].ToString().ToLowerInvariant());
            return result;
        }
    }
    
    public class OobaboogaAPI : MonoBehaviour
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CustomNamingStrategy()
            },
            Culture = CultureInfo.InvariantCulture
        };
        
        private const string ServerURL = "http://127.0.0.1:5000";
        
        private const string CurrentModelAPI = "/v1/internal/model/info";
        private const string AllModelAPI = "/v1/internal/model/list";
        private const string LoadModelAPI = "/v1/internal/model/load";
        private const string UnloadModelAPI = "/v1/internal/model/unload";
    
        public static async Task<ModelInfoResponse> GetCurrentModelAsync()
        {
            const string url = ServerURL + CurrentModelAPI;

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
            const string url = ServerURL + AllModelAPI;

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
            const string url = ServerURL + LoadModelAPI;

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
            const string url = ServerURL + UnloadModelAPI;

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

        public static void CreateCompletionStream(CompletionRequest completionRequest, 
            Action<List<CompletionResponse>> onResponse = null, Action onComplete = null)
        {
            completionRequest.Stream = true;
            string url = $"{ServerURL}/v1/completions";

            DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, CreateBody(completionRequest), 
                ParseResponse<CompletionResponse>, onResponse, onComplete);
        }
        
        public static void CreateChatCompletionStream(ChatCompletionRequest completionRequest, 
            Action<List<ChatCompletionResponse>> onResponse = null, Action onComplete = null)
        {
            completionRequest.Stream = true;
            string url = $"{ServerURL}/v1/chat/completions";

            DispatchRequest(url, UnityWebRequest.kHttpVerbPOST, CreateBody(completionRequest), 
                ParseResponse<ChatCompletionResponse>, onResponse, onComplete);
        }

        private static byte[] CreateBody<T>(T @object)
        {
            string jsonData = JsonConvert.SerializeObject(@object, jsonSerializerSettings);
            return Encoding.UTF8.GetBytes(jsonData);
        }

        //TODO: cancelationToken
        private static async void DispatchRequest<T>(string url, string webMethod, byte[] body, 
            Func<string, List<T>> responseParseMethod, Action<List<T>> onResponse = null, Action onComplete = null)
        {
            using var request = UnityWebRequest.Put(url, body);
            request.method = webMethod;
            request.SetRequestHeader("Content-Type", "application/json"); //TODO: remove magic variable

            var asyncOperation = request.SendWebRequest();

            string previousResponse = string.Empty;
            while (!asyncOperation.isDone)
            {
                string text = request.downloadHandler.text;
                if (!text.Equals(previousResponse))
                {
                    previousResponse = text;
                    List<T> completionResponses = responseParseMethod(text);
                    onResponse?.Invoke(completionResponses);
                }

                await Task.Yield();
            }

            onComplete?.Invoke();
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

    public interface IGenerationParameters
    {
        public string Preset { get; set; }
        public float Min_P { get; set; }
        public bool Dynamic_Temperature { get; set; }
        public float Dynatemp_Low { get; set; }
        public float Dynatemp_High { get; set; }
        public float DynaTemp_Exponent { get; set; }
        public float Smoothing_Factor { get; set; }
        public int Top_K { get; set; }
        public float Repetition_Penalty { get; set; }
        public int Repetition_Penalty_Range { get; set; }
        public float Typical_P { get; set; }
        public float Tfs { get; set; }
        public int Top_A { get; set; }
        public float Epsilon_Cutoff { get; set; }
        public float Eta_Cutoff { get; set; }
        public float Guidance_Scale { get; set; }
        public string Negative_Prompt { get; set; }
        public float Penalty_Alpha { get; set; }
        public int Mirostat_Mode { get; set; }
        public float Mirostat_Tau { get; set; }
        public float Mirostat_Eta { get; set; }
        public bool Temperature_Last { get; set; }
        public bool Do_Sample { get; set; }
        public int Seed { get; set; }
        public float Encoder_Repetition_Penalty { get; set; }
        public int No_Repeat_Ngram_Size { get; set; }
        public int Min_Length { get; set; }
        public int Num_Beams { get; set; }
        public float Length_Penalty { get; set; }
        public bool Early_Stopping { get; set; }
        public int Truncation_Length { get; set; }
        public int Max_Tokens_Second { get; set; }
        public int Prompt_Lookup_Num_Tokens { get; set; }
        public string Custom_Token_Bans { get; set; }
        public string[] Sampler_Priority { get; set; }
        public bool Auto_Max_New_Tokens { get; set; }
        public bool Ban_Eos_Token { get; set; }
        public bool Add_Bos_Token { get; set; }
        public bool Skip_Special_Tokens { get; set; }
        public string Grammar_String { get; set; }
    }

    public interface ICompletionParams
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public int Best_Of { get; set; }
        public bool Echo { get; set; }
        public int Frequency_Penalty { get; set; }
        public Dictionary<string, object> Logit_Bias { get; set; }
        public int Logprobs { get; set; }
        public int Max_Tokens { get; set; }
        public int N { get; set; }
        public int Presence_Penalty { get; set; }
        public string[] Stop { get; set; }
        public bool Stream { get; set; }
        public string Suffix { get; set; }
        public float Temperature { get; set; }
        public float Top_P { get; set; }
        public string User { get; set; }
    }

    public interface IChatCompletionParams
    {
        public List<object> Messages { get; set; }
        public string Model { get; set; }
        public int Frequency_Penalty { get; set; }
        public string Function_Call { get; set; }
        public List<object> Functions { get; set; }
        public Dictionary<string, object> Logit_Bias { get; set; }
        public int Max_Tokens { get; set; }
        public int N { get; set; }
        public int Presence_Penalty { get; set; }
        public string[] Stop { get; set; }
        public bool Stream { get; set; }
        public int Temperature { get; set; }
        public int Top_P { get; set; }
        public string User { get; set; }
        public string Mode { get; set; }
        public string Instruction_Template { get; set; }
        public string Instruction_Template_Str { get; set; }
        public string Character { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Context { get; set; }
        public string Greeting { get; set; }
        public string Chat_Template_Str { get; set; }
        public string Chat_Instruct_Command { get; set; }
        public bool Continue_ { get; set; }
    }

    [Serializable]
    public class CompletionRequest : ICompletionParams, IGenerationParameters
    {
        public string Model { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public int Best_Of { get; set; } = 1;
        public bool Echo { get; set; } = false;
        public int Frequency_Penalty { get; set; } = 0;
        public Dictionary<string, object> Logit_Bias { get; set; } = new();
        public int Logprobs { get; set; } = 0;
        public int Max_Tokens { get; set; } = 512;
        public int N { get; set; } = 1;
        public int Presence_Penalty { get; set; } = 0;
        public string[] Stop { get; set; } = Array.Empty<string>();
        public bool Stream { get; set; } = false;
        public string Suffix { get; set; } = string.Empty;
        public float Temperature { get; set; } = 1;
        public float Top_P { get; set; } = 1;
        public string User { get; set; } = string.Empty;
        
        public string Preset { get; set; } = string.Empty;
        public float Min_P { get; set; } = 0;
        public bool Dynamic_Temperature { get; set; } = false;
        public float Dynatemp_Low { get; set; } = 1;
        public float Dynatemp_High { get; set; } = 1;
        public float DynaTemp_Exponent { get; set; } = 1;
        public float Smoothing_Factor { get; set; } = 0;
        public int Top_K { get; set; } = 0;
        public float Repetition_Penalty { get; set; } = 1;
        public int Repetition_Penalty_Range { get; set; } = 1024;
        public float Typical_P { get; set; } = 1;
        public float Tfs { get; set; } = 1;
        public int Top_A { get; set; } = 0;
        public float Epsilon_Cutoff { get; set; } = 0;
        public float Eta_Cutoff { get; set; } = 0;
        public float Guidance_Scale { get; set; } = 1;
        public string Negative_Prompt { get; set; } = string.Empty;
        public float Penalty_Alpha { get; set; } = 0;
        public int Mirostat_Mode { get; set; } = 0;
        public float Mirostat_Tau { get; set; } = 5;
        public float Mirostat_Eta { get; set; } = 0.1f;
        public bool Temperature_Last { get; set; } = false;
        public bool Do_Sample { get; set; } = true;
        public int Seed { get; set; } = -1;
        public float Encoder_Repetition_Penalty { get; set; } = 1;
        public int No_Repeat_Ngram_Size { get; set; } = 0;
        public int Min_Length { get; set; } = 0;
        public int Num_Beams { get; set; } = 1;
        public float Length_Penalty { get; set; } = 1;
        public bool Early_Stopping { get; set; } = false;
        public int Truncation_Length { get; set; } = 0;
        public int Max_Tokens_Second { get; set; } = 0;
        public int Prompt_Lookup_Num_Tokens { get; set; } = 0;
        public string Custom_Token_Bans { get; set; } = string.Empty;
        public string[] Sampler_Priority { get; set; } = Array.Empty<string>();
        public bool Auto_Max_New_Tokens { get; set; } = false;
        public bool Ban_Eos_Token { get; set; } = false;
        public bool Add_Bos_Token { get; set; } = true;
        public bool Skip_Special_Tokens { get; set; } = true;
        public string Grammar_String { get; set; } = string.Empty;
    }
    
    public class ChatCompletionRequest : IChatCompletionParams, IGenerationParameters
    {
        public List<object> Messages { get; set; } = new();
        public string Model { get; set; } = string.Empty;
        public int Frequency_Penalty { get; set; } = 0;
        public string Function_Call { get; set; } = string.Empty;
        public List<object> Functions { get; set; } = new();
        public Dictionary<string, object> Logit_Bias { get; set; } = new();
        public int Max_Tokens { get; set; } = 512;
        public int N { get; set; } = 1;
        public int Presence_Penalty { get; set; } = 0;
        public string[] Stop { get; set; } = Array.Empty<string>();
        public bool Stream { get; set; } = false;
        public int Temperature { get; set; } = 1;
        public int Top_P { get; set; } = 1;
        public string User { get; set; } = string.Empty;
        public string Mode { get; set; } = "instruct";
        public string Instruction_Template { get; set; } = string.Empty;
        public string Instruction_Template_Str { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string Name1 { get; set; } = string.Empty;
        public string Name2 { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public string Greeting { get; set; } = string.Empty;
        public string Chat_Template_Str { get; set; } = string.Empty;
        public string Chat_Instruct_Command { get; set; } = string.Empty;
        public bool Continue_ { get; set; } = false;
        
        public string Preset { get; set; } = string.Empty;
        public float Min_P { get; set; } = 0;
        public bool Dynamic_Temperature { get; set; } = false;
        public float Dynatemp_Low { get; set; } = 1;
        public float Dynatemp_High { get; set; } = 1;
        public float DynaTemp_Exponent { get; set; } = 1;
        public float Smoothing_Factor { get; set; } = 0;
        public int Top_K { get; set; } = 0;
        public float Repetition_Penalty { get; set; } = 1;
        public int Repetition_Penalty_Range { get; set; } = 1024;
        public float Typical_P { get; set; } = 1;
        public float Tfs { get; set; } = 1;
        public int Top_A { get; set; } = 0;
        public float Epsilon_Cutoff { get; set; } = 0;
        public float Eta_Cutoff { get; set; } = 0;
        public float Guidance_Scale { get; set; } = 1;
        public string Negative_Prompt { get; set; } = string.Empty;
        public float Penalty_Alpha { get; set; } = 0;
        public int Mirostat_Mode { get; set; } = 0;
        public float Mirostat_Tau { get; set; } = 5;
        public float Mirostat_Eta { get; set; } = 0.1f;
        public bool Temperature_Last { get; set; } = false;
        public bool Do_Sample { get; set; } = true;
        public int Seed { get; set; } = -1;
        public float Encoder_Repetition_Penalty { get; set; } = 1;
        public int No_Repeat_Ngram_Size { get; set; } = 0;
        public int Min_Length { get; set; } = 0;
        public int Num_Beams { get; set; } = 1;
        public float Length_Penalty { get; set; } = 1;
        public bool Early_Stopping { get; set; } = false;
        public int Truncation_Length { get; set; } = 0;
        public int Max_Tokens_Second { get; set; } = 0;
        public int Prompt_Lookup_Num_Tokens { get; set; } = 0;
        public string Custom_Token_Bans { get; set; } = string.Empty;
        public string[] Sampler_Priority { get; set; } = Array.Empty<string>();
        public bool Auto_Max_New_Tokens { get; set; } = false;
        public bool Ban_Eos_Token { get; set; } = false;
        public bool Add_Bos_Token { get; set; } = true;
        public bool Skip_Special_Tokens { get; set; } = true;
        public string Grammar_String { get; set; } = string.Empty;
    }
    
    public struct ChatCompletionResponse
    {
        public string ID { get; set; }
        public List<ChatMessageChoice> Choices { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
        public Dictionary<string, object> Usage { get; set; }
    }
    
    public struct ChatMessageChoice
    {
        public long Index { get; set; }
        public object Finish_Reason { get; set; }
        public ChatMessage Delta { get; set; }
    }

    public struct ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
    
    public struct CompletionResponse
    {
        public string ID { get; set; }
        public List<MessageChoice> Choices { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
        public Dictionary<string, object> Usage { get; set; }
    }

    public struct MessageChoice
    {
        public long Index { get; set; }
        public object Finish_Reason { get; set; }
        public string Text { get; set; }
        public JObject Logprobs { get; set; }
    }
}
