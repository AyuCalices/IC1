using System;
using System.Collections.Generic;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaChatCompletionParameters", menuName = "Oobabooga/ChatCompletionParameters")]
    public class ChatCompletionParameters : ScriptableObject, IChatCompletionParameters
    {
        #region User Input
        
        //TODO: done by user input
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Header("User Input")] public List<Message> Messages { get; set; } = new();
        
        //TODO: if set to true, but messages doesnt contain one -> error
        //TODO: done by user input
        /// <summary>
        /// Makes the last bot message in the history be continued instead of starting a new message.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField] public bool Continue_ { get; set; } = false;
        
        //TODO: done by user input
        /// <summary>
        /// A character defined under text-generation-webui/characters. If not set, the default \"Assistant\" character will be used.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField] public CharacterOption Character { get; set; }
        
        #endregion
        
        #region Other Settings
        
        //TODO: enum?
        /// <summary>
        /// Valid options: instruct, chat, chat-instruct.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, Header("Other Settings")] public string Mode { get; set; } = "instruct";
        
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
        
        #endregion
    }
}
