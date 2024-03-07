using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OobaboogaRuntimeIntegration
{
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
        
        /// <summary>
        /// When unchecked, the full response is outputted at once, without streaming the words one at a time. I recommend
        /// unchecking this parameter on high latency networks like running the webui on Google Colab or using --share.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Stream { get; set; } = false;   //is set internally -> not inside interface
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
        public List<ChatMessage> Messages { get; set; } = new();
        public string Model { get; set; } = string.Empty;
        public int Frequency_Penalty { get; set; } = 0;
        public string Function_Call { get; set; } = string.Empty;
        public List<object> Functions { get; set; } = new();
        public Dictionary<string, object> Logit_Bias { get; set; } = new();
        public int Max_Tokens { get; set; } = 512;
        public int N { get; set; } = 1;
        public int Presence_Penalty { get; set; } = 0;
        public string[] Stop { get; set; } = Array.Empty<string>();
        public bool Stream { get; set; } = false;   //is set internally -> not inside interface
        public float Temperature { get; set; } = 1;
        public float Top_P { get; set; } = 1;
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
        public string Suffix { get; set; }
        public float Temperature { get; set; }
        public float Top_P { get; set; }
        public string User { get; set; }
    }

    public interface IChatCompletionParams
    {
        public List<ChatMessage> Messages { get; set; }
        public string Model { get; set; }
        public int Frequency_Penalty { get; set; }
        public string Function_Call { get; set; }
        public List<object> Functions { get; set; }
        public Dictionary<string, object> Logit_Bias { get; set; }
        public int Max_Tokens { get; set; }
        public int N { get; set; }
        public int Presence_Penalty { get; set; }
        public string[] Stop { get; set; }
        public float Temperature { get; set; }
        public float Top_P { get; set; }
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
}
