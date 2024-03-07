using System;
using System.Collections.Generic;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OoobaboogaChatCompletionParameters", menuName = "Ooobabooga/ChatCompletionParameters")]
    public class ChatCompletionParameters : ScriptableObject, IChatCompletionParams
    {
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private List<ChatMessage> _messages;
        public List<ChatMessage> Messages 
        {
            get => _messages;
            set => _messages = value;
        }
        
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private string _model = string.Empty;
        public string Model
        {
            get => _model;
            set => _model = value;
        }

        /// <summary>
        /// Repetition penalty that scales based on how many times the token has appeared in the context.
        /// Be careful with this; there's no limit to how much a token can be penalized.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _frequencyPenalty = 0;
        public int Frequency_Penalty 
        {
            get => _frequencyPenalty;
            set => _frequencyPenalty = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private string _functionCall = string.Empty;
        public string Function_Call 
        {
            get => _functionCall;
            set => _functionCall = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private List<object> _funtions = new();
        public List<object> Functions 
        {
            get => _funtions;
            set => _funtions = value;
        }

        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [SerializeField, HideInInspector] private Dictionary<string, object> _logitBias = new();
        public Dictionary<string, object> Logit_Bias 
        {
            get => _logitBias;
            set => _logitBias = value;
        }

        /// <summary>
        /// Maximum number of tokens to generate. Don't set it higher than necessary: it is used in the truncation
        /// calculation through the formula (prompt_length) = min(truncation_length - max_new_tokens, prompt_length),
        /// so your prompt will get truncated if you set it too high.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _maxTokens = 0;
        public int Max_Tokens 
        {
            get => _maxTokens;
            set => _maxTokens = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private int _n = 1;
        public int N 
        {
            get => _n;
            set => _n = value;
        }

        /// <summary>
        /// repetition_penalty: Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Similar to repetition_penalty, but with an additive offset on the raw token scores instead of a multiplicative factor.
        /// It may generate better results. 0 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Previously called "additive_repetition_penalty".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _presencePenalty = 0;
        public int Presence_Penalty 
        {
            get => _presencePenalty;
            set => _presencePenalty = value;
        }

        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        [SerializeField, HideInInspector] private string[] _stop = Array.Empty<string>();
        public string[] Stop 
        {
            get => _stop;
            set => _stop = value;
        }

        /// <summary>
        /// Primary factor to control the randomness of outputs. 0 = deterministic (only the most likely token is used). Higher value = more randomness.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _temperature = 1;
        public float Temperature 
        {
            get => _temperature;
            set => _temperature = value;
        }

        /// <summary>
        /// If not set to 1, select tokens with probabilities adding up to less than this number. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _topP = 1;
        public float Top_P 
        {
            get => _topP;
            set => _topP = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private string _user = string.Empty;
        public string User 
        {
            get => _user;
            set => _user = value;
        }

        //TODO: enum?
        /// <summary>
        /// Valid options: instruct, chat, chat-instruct.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _mode = "instruct";
        public string Mode 
        {
            get => _mode;
            set => _mode = value;
        }
        
        /// <summary>
        /// An instruction template defined under text-generation-webui/instruction-templates.
        /// If not set, the correct template will be automatically obtained from the model metadata.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _instructionTemplate = string.Empty;
        public string Instruction_Template 
        {
            get => _instructionTemplate;
            set => _instructionTemplate = value;
        }

        /// <summary>
        /// A Jinja2 instruction template. If set, will take precedence over everything else.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _instructionTemplateStr = string.Empty;
        public string Instruction_Template_Str 
        {
            get => _instructionTemplateStr;
            set => _instructionTemplateStr = value;
        }

        /// <summary>
        /// A character defined under text-generation-webui/characters. If not set, the default \"Assistant\" character will be used.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _character = string.Empty;
        public string Character 
        {
            get => _character;
            set => _character = value;
        }

        /// <summary>
        /// Your name (the user). By default, it's \"You\"." (User_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _name1 = string.Empty;
        public string Name1 
        {
            get => _name1;
            set => _name1 = value;
        }
        
        /// <summary>
        /// Overwrites the value set by character field. (Bot_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _name2 = string.Empty;
        public string Name2 
        {
            get => _name2;
            set => _name2 = value;
        }

        /// <summary>
        /// Overwrites the value set by character field.
        /// A string that is always at the top of the prompt. It never gets truncated.
        /// It usually defines the bot's personality and some key elements of the conversation.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _context = string.Empty;
        public string Context 
        {
            get => _context;
            set => _context = value;
        }

        /// <summary>
        /// Overwrites the value set by character field.
        /// An opening message for the bot. When set, it appears whenever you start a new chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _greeting = string.Empty;
        public string Greeting 
        {
            get => _greeting;
            set => _greeting = value;
        }

        /// <summary>
        /// Jinja2 template for chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _chatTemplateStr = string.Empty;
        public string Chat_Template_Str 
        {
            get => _chatTemplateStr;
            set => _chatTemplateStr = value;
        }

        //TODO: no clue
        [SerializeField, HideInInspector] private string _chatInstructCommand = string.Empty;
        public string Chat_Instruct_Command 
        {
            get => _chatInstructCommand;
            set => _chatInstructCommand = value;
        }

        /// <summary>
        /// Makes the last bot message in the history be continued instead of starting a new message.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private bool _continue = false;
        public bool Continue_ 
        {
            get => _continue;
            set => _continue = value;
        }
    }
}
