using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace OobaboogaRuntimeIntegration
{
    
    #region Helper
    
    public static class IntegrationHelper
    {
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
    }
    
    #endregion
    
    #region Model Info
    
    [Serializable]
    public class ModelInfoResponse
    {
        public string model_name;
        public List<string> lora_names;
    }
    
    #endregion
    
    #region Model List
    
    [Serializable]
    public class ModelListResponse
    {
        public List<string> model_names;
    }
    
    #endregion
    
    #region Load Model
    
    [Serializable]
    public class LoadModelRequest
    {
        public string model_name;
        public Dictionary<string, object> args;
        public Dictionary<string, object> settings;
    }
    
    #endregion
    
    #region Completion
    
    public class CompletionRequestContainer
    {
        public ICompletionParameters CompletionParameters { get; set; }
        public IGenerationParameters GenerationParameters { get; set; }
        public IPresetName PresetName { get; set; }
        public IPresetParameters PresetParameters { get; set; }
    }

    [Serializable]
    public class CompletionRequest : ICompletionParameters, IGenerationParameters, IPresetName, IPresetParameters
    {
        public CompletionRequest(bool stream, CompletionRequestContainer completionRequestContainer)
        {
            Stream = stream;
            IntegrationHelper.CopyProperties(completionRequestContainer.CompletionParameters, this);
            IntegrationHelper.CopyProperties(completionRequestContainer.GenerationParameters, this);
            IntegrationHelper.CopyProperties(completionRequestContainer.PresetName, this);
            IntegrationHelper.CopyProperties(completionRequestContainer.PresetParameters, this);
        }

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
        public bool Stream { get; set; } = false;   //is set internally -> not inside interface
        public string Suffix { get; set; } = string.Empty;
        public float Temperature { get; set; } = 1f;
        public float Top_P { get; set; } = 1f;
        public string User { get; set; } = string.Empty;
        public string Preset { get; set; } = string.Empty;
        public float Min_P { get; set; } = 0f;
        public bool Dynamic_Temperature { get; set; } = false;
        public float Dynatemp_Low { get; set; } = 1f;
        public float Dynatemp_High { get; set; } = 1f;
        public float DynaTemp_Exponent { get; set; } = 1f;
        public float Smoothing_Factor { get; set; } = 0f;
        public float Smoothing_Curve { get; set; } = 1f;
        public int Top_K { get; set; } = 0;
        public float Repetition_Penalty { get; set; } = 1f;
        public int Repetition_Penalty_Range { get; set; } = 1024;
        public float Typical_P { get; set; } = 1f;
        public float Tfs { get; set; } = 1f;
        public int Top_A { get; set; } = 0;
        public float Epsilon_Cutoff { get; set; } = 0f;
        public float Eta_Cutoff { get; set; } = 0f;
        public float Guidance_Scale { get; set; } = 1f;
        public string Negative_Prompt { get; set; } = string.Empty;
        public float Penalty_Alpha { get; set; } = 0f;
        public int Mirostat_Mode { get; set; } = 0;
        public float Mirostat_Tau { get; set; } = 5f;
        public float Mirostat_Eta { get; set; } = 0.1f;
        public bool Temperature_Last { get; set; } = false;
        public bool Do_Sample { get; set; } = true;
        public int Seed { get; set; } = -1;
        public float Encoder_Repetition_Penalty { get; set; } = 1f;
        public int No_Repeat_Ngram_Size { get; set; } = 0;
        public int Min_Length { get; set; } = 0;
        public int Num_Beams { get; set; } = 1;
        public float Length_Penalty { get; set; } = 1f;
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
    
    public interface ICompletionParameters
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public int Best_Of { get; set; }
        public bool Echo { get; set; }
        public Dictionary<string, object> Logit_Bias { get; set; }
        public int Logprobs { get; set; }
        public int N { get; set; }
        public string[] Stop { get; set; }
        public string Suffix { get; set; }
        public string User { get; set; }
    }
    
    public struct ChatCompletionResponse
    {
        public string ID { get; set; }
        public List<ChatCompletionChoice> Choices { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
        public Dictionary<string, object> Usage { get; set; }
    }
    
    public struct ChatCompletionChoice
    {
        public long Index { get; set; }
        public object Finish_Reason { get; set; }
        public Message Delta { get; set; }
    }
    
    #endregion
    
    #region ChatCompletion
    
    public class ChatCompletionRequestContainer
    {
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public IMessageWrapper MessageWrapper { get; }
        public ICharacterName CharacterName { get; }
        public ICharacterParameters CharacterParameters { get; }
        
        public ChatCompletionRequestContainer(IMessageWrapper messageWrapper, ICharacterName characterName)
        {
            MessageWrapper = messageWrapper;
            CharacterName = characterName;
        }
        
        public ChatCompletionRequestContainer(IMessageWrapper messageWrapper, ICharacterParameters characterParameters)
        {
            MessageWrapper = messageWrapper;
            CharacterParameters = characterParameters;
        }
        
        public IChatCompletionParameters ChatCompletionParameters { get; set; }
        public IGenerationParameters GenerationParameters { get; set; }
        public IPresetName PresetName { get; set; }
        public IPresetParameters PresetParameters { get; set; }
        
    }
    
    public class ChatCompletionRequest : IChatCompletionParameters, IMessageWrapper, ICharacterName, ICharacterParameters, IGenerationParameters, IPresetName, IPresetParameters
    {
        public ChatCompletionRequest(bool stream, ChatCompletionRequestContainer chatCompletionRequestContainer)
        {
            Messages = chatCompletionRequestContainer.MessageWrapper.Messages;

            if (Messages.Count == 0 && Continue_)
            {
                Debug.LogWarning($"The AI can't continue the last message, if the provided {typeof(List<Message>)} is empty! " +
                                 $"This request will be done with {nameof(Continue_)} set to false");
                Continue_ = false;
            }
            
            Stream = stream;
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.ChatCompletionParameters, this);
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.CharacterName, this);
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.CharacterParameters, this);
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.GenerationParameters, this);
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.PresetName, this);
            IntegrationHelper.CopyProperties(chatCompletionRequestContainer.PresetParameters, this);
        }
        
        public List<Message> Messages { get; set; } = new();
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
        public float Temperature { get; set; } = 1f;
        public float Top_P { get; set; } = 1f;
        public string User { get; set; } = string.Empty;
        public string Mode { get; set; } = "instruct";
        public string Instruction_Template { get; set; } = string.Empty;
        public string Instruction_Template_Str { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string Name1 { get; set; } = string.Empty;
        public string User_Bio { get; set; } = string.Empty;
        public string Name2 { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public string Greeting { get; set; } = string.Empty;
        public string Chat_Template_Str { get; set; } = string.Empty;
        public string Chat_Instruct_Command { get; set; } = string.Empty;
        public bool Continue_ { get; set; } = false;
        public string Preset { get; set; } = string.Empty;
        public float Min_P { get; set; } = 0f;
        public bool Dynamic_Temperature { get; set; } = false;
        public float Dynatemp_Low { get; set; } = 1f;
        public float Dynatemp_High { get; set; } = 1f;
        public float DynaTemp_Exponent { get; set; } = 1f;
        public float Smoothing_Factor { get; set; } = 0f;
        public float Smoothing_Curve { get; set; } = 1f;
        public int Top_K { get; set; } = 0;
        public float Repetition_Penalty { get; set; } = 1f;
        public int Repetition_Penalty_Range { get; set; } = 1024;
        public float Typical_P { get; set; } = 1f;
        public float Tfs { get; set; } = 1f;
        public int Top_A { get; set; } = 0;
        public float Epsilon_Cutoff { get; set; } = 0f;
        public float Eta_Cutoff { get; set; } = 0f;
        public float Guidance_Scale { get; set; } = 1f;
        public string Negative_Prompt { get; set; } = string.Empty;
        public float Penalty_Alpha { get; set; } = 0f;
        public int Mirostat_Mode { get; set; } = 0;
        public float Mirostat_Tau { get; set; } = 5f;
        public float Mirostat_Eta { get; set; } = 0.1f;
        public bool Temperature_Last { get; set; } = false;
        public bool Do_Sample { get; set; } = true;
        public int Seed { get; set; } = -1;
        public float Encoder_Repetition_Penalty { get; set; } = 1f;
        public int No_Repeat_Ngram_Size { get; set; } = 0;
        public int Min_Length { get; set; } = 0;
        public int Num_Beams { get; set; } = 1;
        public float Length_Penalty { get; set; } = 1f;
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

    public interface IMessageWrapper
    {
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public List<Message> Messages { get; set; }
    }
    
    public interface IChatCompletionParameters
    {
        public string Model { get; set; }
        public string Function_Call { get; set; }
        public List<object> Functions { get; set; }
        public Dictionary<string, object> Logit_Bias { get; set; }
        public int N { get; set; }
        public string[] Stop { get; set; }
        public string User { get; set; }
        public string Mode { get; set; }
        public string Instruction_Template { get; set; }
        public string Instruction_Template_Str { get; set; }
        public string Chat_Template_Str { get; set; }
        public string Chat_Instruct_Command { get; set; }
        
        /// <summary>
        /// Makes the last bot message in the history be continued instead of starting a new message.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public bool Continue_ { get; set; }
    }

    public interface ICharacterName
    {
        public string Character { get; set; }
    }
    
    public interface ICharacterParameters
    {
        /// <summary>
        /// Your name (the user). By default, it's \"You\"." (User_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Name1 { get; set; }
        
        /// <summary>
        /// The user description/personality.
        /// </summary>
        public string User_Bio { get; set; }
        
        /// <summary>
        /// Overwrites the value set by character field. (Bot_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Name2 { get; set; }
        
        /// <summary>
        /// Overwrites the value set by character field.
        /// A string that is always at the top of the prompt. It never gets truncated.
        /// It usually defines the bot's personality and some key elements of the conversation.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Context { get; set; }
        
        /// <summary>
        /// Overwrites the value set by character field.
        /// An opening message for the bot. When set, it appears whenever you start a new chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Greeting { get; set; }
    }
    
    public struct CompletionResponse
    {
        public string ID { get; set; }
        public List<CompletionChoice> Choices { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
        public Dictionary<string, object> Usage { get; set; }
    }

    public struct CompletionChoice
    {
        public long Index { get; set; }
        public object Finish_Reason { get; set; }
        public string Text { get; set; }
        public JObject Logprobs { get; set; }
    }
    
    [Serializable]
    public struct Message
    {
        [field: SerializeField] public string Role { get; set; }
        [field: SerializeField] public string Content { get; set; }
    }
    
    #endregion

    #region GenerationParameters

    public interface IGenerationParameters
    {
        public string Negative_Prompt { get; set; }
        public int Seed { get; set; }
        public int Max_Tokens { get; set; }
        public int Truncation_Length { get; set; }
        public int Max_Tokens_Second { get; set; }
        public int Prompt_Lookup_Num_Tokens { get; set; }
        public string Custom_Token_Bans { get; set; }
        public bool Auto_Max_New_Tokens { get; set; }
        public bool Ban_Eos_Token { get; set; }
        public bool Add_Bos_Token { get; set; }
        public bool Skip_Special_Tokens { get; set; }
        public string Grammar_String { get; set; }
    }

    public interface IPresetName
    {
        public string Preset { get; set; }
    }
    
    public interface IPresetParameters
    {
        public float Temperature { get; set; }
        public bool Temperature_Last { get; set; }
        public bool Dynamic_Temperature { get; set; }
        public float Dynatemp_Low { get; set; }
        public float Dynatemp_High { get; set; }
        public float DynaTemp_Exponent { get; set; }
        public float Smoothing_Factor { get; set; }
        public float Smoothing_Curve { get; set; }
        public float Top_P { get; set; }
        public float Min_P { get; set; }
        public int Top_K { get; set; }
        public float Repetition_Penalty { get; set; }
        public int Presence_Penalty { get; set; }
        public int Frequency_Penalty { get; set; }
        public int Repetition_Penalty_Range { get; set; }
        public float Typical_P { get; set; }
        public float Tfs { get; set; }
        public int Top_A { get; set; }
        public float Epsilon_Cutoff { get; set; }
        public float Eta_Cutoff { get; set; }
        public float Guidance_Scale { get; set; }
        public float Penalty_Alpha { get; set; }
        public int Mirostat_Mode { get; set; }
        public float Mirostat_Tau { get; set; }
        public float Mirostat_Eta { get; set; }
        public bool Do_Sample { get; set; }
        public float Encoder_Repetition_Penalty { get; set; }
        public int No_Repeat_Ngram_Size { get; set; }
        public int Min_Length { get; set; }
        public int Num_Beams { get; set; }
        public float Length_Penalty { get; set; }
        public bool Early_Stopping { get; set; }
        public string[] Sampler_Priority { get; set; }
    }

    #endregion
    
}
