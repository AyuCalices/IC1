using System;
using System.Collections.Generic;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaCompletionParameters", menuName = "Oobabooga/CompletionParameters")]
    public class CompletionParameters : ScriptableObject, ICompletionParameters
    {
        //TODO: done by user input
        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Prompt { get; set; } = string.Empty;
        
        #region Not Yet Supported
        
        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [field: SerializeField, HideInInspector] public Dictionary<string, object> Logit_Bias { get; set; } = new();

        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [field: SerializeField, HideInInspector] public int Logprobs { get; set; } = 0;
        
        #endregion

        #region Unused Parameter
        
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, HideInInspector] public string Model { get; set; } = string.Empty;
        
        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/extensions/openai/typing.py#L55
        /// </summary>
        [field: SerializeField, HideInInspector] public int Best_Of { get; set; } = 1;
        
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
        
        #region TODO
        
        //TODO: probably debugging for oobabooga internal?
        [field: SerializeField, HideInInspector] public bool Echo { get; set; } = false;
        
        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        [field: SerializeField, HideInInspector] public string[] Stop { get; set; } = Array.Empty<string>();

        //TODO: no clue
        [field: SerializeField, HideInInspector] public string Suffix { get; set; } = string.Empty;
        
        #endregion
    }
}
