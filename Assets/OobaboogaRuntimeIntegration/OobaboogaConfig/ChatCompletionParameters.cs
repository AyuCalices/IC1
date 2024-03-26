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
        
        [field: SerializeField, Header("Template Strings")] public string Instruction_Template { get; set; } = string.Empty;
        
        [field: SerializeField, TextArea(3, 50)] public string Instruction_Template_Str { get; set; } = string.Empty;
        
        [field: SerializeField, TextArea(3, 50)] public string Chat_Template_Str { get; set; } = string.Empty;
        
        #endregion
        
        #region Unused Parameter
        
        [field: SerializeField, HideInInspector] public string Model { get; set; } = string.Empty;
        
        [field: SerializeField, HideInInspector] public string Function_Call { get; set; } = string.Empty;
        
        [field: SerializeField, HideInInspector] public List<object> Functions { get; set; } = new();
        
        [field: SerializeField, HideInInspector] public int N { get; set; } = 1;
        
        [field: SerializeField, HideInInspector] public string User { get; set; } = string.Empty;
        
        #endregion

        #region Not Yet supported
        
        [field: SerializeField, HideInInspector] public Dictionary<string, object> Logit_Bias { get; set; } = new();

        #endregion
        
        #region TODO
        
        [field: SerializeField, HideInInspector] public string[] Stop { get; set; } = Array.Empty<string>();
        
        [field: SerializeField, HideInInspector] public string Chat_Instruct_Command { get; set; } = string.Empty;
        
        public bool Continue_ { get; set; }

        #endregion
    }
}
