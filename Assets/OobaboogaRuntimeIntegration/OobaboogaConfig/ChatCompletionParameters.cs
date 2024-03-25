using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    public enum ChatCompletionMode
    {
        [Description("chat"), UsedImplicitly]
        Chat,
        [Description("chat-instruct"), UsedImplicitly]
        ChatInstruct,
        [Description("instruct"), UsedImplicitly]
        Instruct
    }
    
    [CreateAssetMenu(fileName = "OobaboogaChatCompletionParameters", menuName = "Oobabooga/ChatCompletionParameters")]
    public class ChatCompletionParameters : ScriptableObject, IChatCompletionParameters
    {
        #region Other Settings
        
        [SerializeField, Header("Other Settings")] private ChatCompletionMode _chatCompletionMode = ChatCompletionMode.Chat;

        /// <summary>
        /// Valid options: instruct, chat, chat-instruct.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        public string Mode
        {
            get
            {
                var field = _chatCompletionMode.GetType().GetField(_chatCompletionMode.ToString());
                var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                return attribute != null ? attribute.Description : _chatCompletionMode.ToString();
            }
            set => _chatCompletionMode = (ChatCompletionMode)Enum.Parse(typeof(ChatCompletionMode), value, ignoreCase: true);
        }

        #endregion
        
        #region TemplateString
        
        /// <summary>
        /// An instruction template defined under text-generation-webui/instruction-templates.
        /// If not set, the correct template will be automatically obtained from the model metadata.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, Header("Template Strings")] public string Instruction_Template { get; set; } = string.Empty;

        /// <summary>
        /// A Jinja2 instruction template. If set, will take precedence over everything else.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Instruction_Template_Str { get; set; } = string.Empty;

        /// <summary>
        /// Jinja2 template for chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Chat_Template_Str { get; set; } = string.Empty;
        
        #endregion
        
        #region Unused Parameter
        
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public string Function_Call { get; set; } = string.Empty;

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public List<object> Functions { get; set; } = new();
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public int N { get; set; } = 1;
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public string User { get; set; } = string.Empty;
        
        #endregion

        #region Not Yet supported

        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [field: SerializeField, HideInInspector] public Dictionary<string, object> Logit_Bias { get; set; } = new();

        #endregion
        
        #region TODO
        
        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        [field: SerializeField, HideInInspector] public string[] Stop { get; set; } = Array.Empty<string>();

        //TODO: no clue
        [field: SerializeField, HideInInspector] public string Chat_Instruct_Command { get; set; } = string.Empty;
        
        public bool Continue_ { get; set; }

        #endregion
    }
}
