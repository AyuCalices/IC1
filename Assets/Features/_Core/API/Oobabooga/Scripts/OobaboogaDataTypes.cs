using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Features._Core.API.Oobabooga.Scripts
{
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
            APICore.CopyProperties(completionRequestContainer.CompletionParameters, this);
            APICore.CopyProperties(completionRequestContainer.GenerationParameters, this);
            APICore.CopyProperties(completionRequestContainer.PresetName, this);
            APICore.CopyProperties(completionRequestContainer.PresetParameters, this);
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
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Model { get; set; }
        
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public string Prompt { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/extensions/openai/typing.py#L55
        /// </summary>
        public int Best_Of { get; set; }
        
        //TODO: probably debugging for oobabooga internal?
        public bool Echo { get; set; }
        
        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        public Dictionary<string, object> Logit_Bias { get; set; }
        
        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        public int Logprobs { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public int N { get; set; }
        
        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        public string[] Stop { get; set; }
        
        //TODO: no clue
        public string Suffix { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
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
    
    public class ChatCompletionRequestContainer : IMessageWrapper, IContinueOption
    {
        public ChatCompletionRequestContainer(List<Message> messages, bool continue_)
        {
            Messages = messages;
            Continue_ = continue_;
        }
        
        public List<Message> Messages { get; set; }
        public bool Continue_ { get; set; }
        
        public IChatCompletionParameters ChatCompletionParameters { get; set; }
        public ICharacterName CharacterName { get; set; }
        public ICharacterParameters CharacterParameters { get; set; }
        public IGenerationParameters GenerationParameters { get; set; }
        public IPresetName PresetName { get; set; }
        public IPresetParameters PresetParameters { get; set; }
    }
    
    public class ChatCompletionRequest : IChatCompletionParameters, IMessageWrapper, IContinueOption, ICharacterName, ICharacterParameters, IGenerationParameters, IPresetName, IPresetParameters
    {
        public ChatCompletionRequest(bool stream, ChatCompletionRequestContainer chatCompletionRequestContainer)
        {
            Messages = chatCompletionRequestContainer.Messages;
            Continue_ = chatCompletionRequestContainer.Continue_;
            Stream = stream;
            
            APICore.CopyProperties(chatCompletionRequestContainer.ChatCompletionParameters, this);
            APICore.CopyProperties(chatCompletionRequestContainer.CharacterName, this);
            APICore.CopyProperties(chatCompletionRequestContainer.CharacterParameters, this);
            APICore.CopyProperties(chatCompletionRequestContainer.GenerationParameters, this);
            APICore.CopyProperties(chatCompletionRequestContainer.PresetName, this);
            APICore.CopyProperties(chatCompletionRequestContainer.PresetParameters, this);
            
            if (Messages.Count == 0 && Continue_)
            {
                Debug.LogWarning($"The AI can't continue the last message, if the provided {typeof(List<Message>)} is empty! " +
                                 $"This request will be done with {nameof(Continue_)} set to false");
                Continue_ = false;
            }
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
        public bool Stream { get; set; } = false;   //currently set internally -> not inside interface
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

    public interface IContinueOption
    {
        /// <summary>
        /// Makes the last bot message in the history be continued instead of starting a new message.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public bool Continue_ { get; set; }
    }
    
    public interface IChatCompletionParameters
    {
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Model { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Function_Call { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public List<object> Functions { get; set; }
        
        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        public Dictionary<string, object> Logit_Bias { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public int N { get; set; }
        
        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        public string[] Stop { get; set; }
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string User { get; set; }
        
        /// <summary>
        /// Valid options: instruct, chat, chat-instruct.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Mode { get; set; }
        
        /// <summary>
        /// An instruction template defined under text-generation-webui/instruction-templates.
        /// If not set, the correct template will be automatically obtained from the model metadata.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Instruction_Template { get; set; }
        
        /// <summary>
        /// A Jinja2 instruction template. If set, will take precedence over everything else.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Instruction_Template_Str { get; set; }
        
        /// <summary>
        /// Jinja2 template for chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Chat_Template_Str { get; set; }
        
        //TODO: no clue
        public string Chat_Instruct_Command { get; set; }
    }

    public interface ICharacterName
    {
        /// <summary>
        /// A character defined under text-generation-webui/characters. If not set, the default \"Assistant\" character will be used.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Character { get; set; }
    }
    
    //TODO: Overwrites the value set by character field.
    /// <summary>
    /// Overwrites the value set by character field.
    /// </summary>
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
        /// <summary>
        /// Only used when guidance_scale != 1. It is most useful for instruct models and custom system messages.
        /// You place your full prompt in this field with the system message replaced with the default one
        /// for the model (like "You are Llama, a helpful assistant...") to make the model pay more attention to your custom system message.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public string Negative_Prompt { get; set; }
        
        /// <summary>
        /// Set the Pytorch seed to this number. Note that some loaders do not use Pytorch (notably llama.cpp),
        /// and others are not deterministic (notably ExLlama v1 and v2). For these loaders, the seed has no effect.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Seed { get; set; }
        
        /// <summary>
        /// Maximum number of tokens to generate. Don't set it higher than necessary: it is used in the truncation
        /// calculation through the formula (prompt_length) = min(truncation_length - max_new_tokens, prompt_length),
        /// so your prompt will get truncated if you set it too high.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Max_Tokens { get; set; }
        
        /// <summary>
        /// Used to prevent the prompt from getting bigger than the model's context length. In the case of the transformers
        /// loader, which allocates memory dynamically, this parameter can also be used to set a VRAM ceiling and prevent
        /// out-of-memory errors. This parameter is automatically updated with the model's context length (from "n_ctx" or
        /// "max_seq_len" for loaders that use these parameters, and from the model metadata directly for loaders that do not)
        /// when you load a model.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Truncation_Length { get; set; }
        
        /// <summary>
        /// to make text readable in real-time in case the model is generating too fast.
        /// Good if you want to flex and tell everyone how good your GPU is.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Max_Tokens_Second { get; set; }
        
        /// <summary>
        /// Activates Prompt Lookup Decoding.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/modules/ui_parameters.py#L77
        /// </summary>
        public int Prompt_Lookup_Num_Tokens { get; set; }
        
        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to ban the model from generating certain tokens altogether. You need to find the token IDs under
        /// "Default" &gt; "Tokens" or "Notebook" &gt; "Tokens", or by looking at the tokenizer.json for the model directly.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public string Custom_Token_Bans { get; set; }
        
        /// <summary>
        /// When checked, the max_new_tokens parameter is expanded in the backend to the available context length. The maximum
        /// length is given by the "truncation_length" parameter. This is useful for getting long replies in the Chat tab without
        /// having to click on "Continue" many times.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Auto_Max_New_Tokens { get; set; }
        
        /// <summary>
        /// One of the possible tokens that a model can generate is the EOS (End of Sequence) token. When it is generated,
        /// the generation stops prematurely. When this parameter is checked, that token is banned from being generated,
        /// and the generation will always generate "max_new_tokens" tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Ban_Eos_Token { get; set; }
        
        /// <summary>
        /// By default, the tokenizer will add a BOS (Beginning of Sequence) token to your prompt. During training, BOS
        /// tokens are used to separate different documents. If unchecked, no BOS token will be added, and the model will
        /// interpret your prompt as being in the middle of a document instead of at the start of one. This significantly
        /// changes the output and can make it more creative.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Add_Bos_Token { get; set; }
        
        /// <summary>
        /// When decoding the generated tokens, skip special tokens from being converted to their text representation.
        /// Otherwise, BOS appears as <s>, EOS as </s>, etc.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Skip_Special_Tokens { get; set; }
        
        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to constrain the model output to a particular format. For instance, you can make the model generate lists,
        /// JSON, specific words, etc. Grammar is extremely powerful and I highly recommend it. The syntax looks a bit daunting
        /// at first sight, but it gets very easy once you understand it.
        /// See the GBNF Guide for details: https://github.com/ggerganov/llama.cpp/blob/master/grammars/README.md
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public string Grammar_String { get; set; }
    }

    public interface IPresetName
    {
        /// <summary>
        /// The name of a file under text-generation-webui/presets (without the .yaml extension).
        /// The sampling parameters that get overwritten by this option are the keys in the default_preset() function in modules/presets.py.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/modules/presets.py
        /// </summary>
        public string Preset { get; set; }
    }
    
    public interface IPresetParameters
    {
        /// <summary>
        /// Primary factor to control the randomness of outputs. 0 = deterministic (only the most likely token is used). Higher value = more randomness.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Temperature { get; set; }
        
        /// <summary>
        /// Makes temperature the last sampler instead of the first. With this, you can remove low probability tokens
        /// with a sampler like min_p and then use a high temperature to make the model creative without losing coherency.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Temperature_Last { get; set; }
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Dynamic_Temperature { get; set; }
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Dynatemp_Low { get; set; }
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Dynatemp_High { get; set; }
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float DynaTemp_Exponent { get; set; }
        
        /// <summary>
        /// Activates Quadratic Sampling. When `0 &lt; smoothing_factor &lt; 1`, the logits distribution becomes flatter. When `smoothing_factor &gt; 1`, it becomes more peaked.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/docs/03%20-%20Parameters%20Tab.md?plain=1#L58
        /// </summary>
        public float Smoothing_Factor { get; set; }
        
        /// <summary>
        /// Adjusts the dropoff curve of Quadratic Sampling.
        /// </summary>
        public float Smoothing_Curve { get; set; }
        
        /// <summary>
        /// If not set to 1, select tokens with probabilities adding up to less than this number. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Top_P { get; set; }
        
        /// <summary>
        /// Tokens with probability smaller than (min_p) * (probability of the most likely token) are discarded. This is the same as top_a but without squaring the probability.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Min_P { get; set; }
        
        /// <summary>
        /// Similar to top_p, but select instead only the top_k most likely tokens. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Top_K { get; set; }
        
        /// <summary>
        /// Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Repetition_Penalty { get; set; }
        
        /// <summary>
        /// repetition_penalty: Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Similar to repetition_penalty, but with an additive offset on the raw token scores instead of a multiplicative factor.
        /// It may generate better results. 0 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Previously called "additive_repetition_penalty".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Presence_Penalty { get; set; }
        
        /// <summary>
        /// Repetition penalty that scales based on how many times the token has appeared in the context.
        /// Be careful with this; there's no limit to how much a token can be penalized.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Frequency_Penalty { get; set; }
        
        /// <summary>
        /// The number of most recent tokens to consider for repetition penalty. 0 makes all tokens be used.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Repetition_Penalty_Range { get; set; }
        
        /// <summary>
        /// If not set to 1, select only tokens that are at least this much more likely to appear than random tokens, given the prior text.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Typical_P { get; set; }
        
        /// <summary>
        /// Tries to detect a tail of low-probability tokens in the distribution and removes those tokens.
        /// See this blog post for details: https://www.trentonbricken.com/Tail-Free-Sampling/
        /// The closer to 0, the more discarded tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Tfs { get; set; }
        
        /// <summary>
        /// Tokens with probability smaller than (top_a) * (probability of the most likely token)^2 are discarded.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Top_A { get; set; }
        
        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. This sets a probability floor below which tokens are excluded from being sampled.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Epsilon_Cutoff { get; set; }
        
        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. The main parameter of the special Eta Sampling technique.
        /// See this paper for a description: https://arxiv.org/pdf/2210.15191.pdf
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Eta_Cutoff { get; set; }
        
        /// <summary>
        /// The main parameter for Classifier-Free Guidance (CFG).
        /// The paper suggests that 1.5 is a good value: https://arxiv.org/pdf/2306.17806.pdf
        /// It can be used in conjunction with a negative prompt or not.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Guidance_Scale { get; set; }
        
        /// <summary>
        /// Contrastive Search is enabled by setting this to greater than zero and unchecking "do_sample".
        /// It should be used with a low value of top_k, for instance, top_k = 4.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Penalty_Alpha { get; set; }
        
        /// <summary>
        /// Activates the Mirostat sampling technique. It aims to control perplexity during sampling.
        /// See the paper: https://arxiv.org/abs/2007.14966
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Mirostat_Mode { get; set; }
        
        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 8 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Mirostat_Tau { get; set; }
        
        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 0.1 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Mirostat_Eta { get; set; }
        
        /// <summary>
        /// When unchecked, sampling is entirely disabled, and greedy decoding is used instead (the most likely token is always picked).
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Do_Sample { get; set; }
        
        /// <summary>
        /// Also known as the "Hallucinations filter". Used to penalize tokens that are not in the prior text.
        /// Higher value = more likely to stay in context, lower value = more likely to diverge.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Encoder_Repetition_Penalty { get; set; }
        
        /// <summary>
        /// If not set to 0, specifies the length of token sets that are completely blocked from repeating at all.
        /// Higher values = blocks larger phrases, lower values = blocks words or letters from repeating.
        /// Only 0 or high values are a good idea in most cases.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int No_Repeat_Ngram_Size { get; set; }
        
        /// <summary>
        /// Minimum generation length in tokens. This is a built-in parameter in the transformers library that has never
        /// been very useful. Typically you want to check "Ban the eos_token" instead.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Min_Length { get; set; }
        
        /// <summary>
        /// Number of beams for beam search. 1 means no beam search.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public int Num_Beams { get; set; }
        
        /// <summary>
        /// Used by beam search only. length_penalty &gt; 0.0 promotes longer sequences,
        /// while length_penalty &lt; 0.0 encourages shorter sequences.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public float Length_Penalty { get; set; }
        
        /// <summary>
        /// Used by beam search only. When checked, the generation stops as soon as there are "num_beams"
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        public bool Early_Stopping { get; set; }
        
        /// <summary>
        /// List of samplers where the first items will appear first in the stack. Example: [\"top_k\", \"temperature\", \"top_p\"].
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string[] Sampler_Priority { get; set; }
    }

    #endregion
    
}
