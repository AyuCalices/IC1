using System;
using System.Collections.Generic;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaCompletionParameters", menuName = "Oobabooga/CompletionParameters")]
    public class CompletionParameters : ScriptableObject, ICompletionParameters
    {
        [field: SerializeField, TextArea(3, 50)] public string Prompt { get; set; } = string.Empty;
        
        #region Not Yet Supported
        
        [field: SerializeField, HideInInspector] public Dictionary<string, object> Logit_Bias { get; set; } = new();
        
        [field: SerializeField, HideInInspector] public int Logprobs { get; set; } = 0;
        
        #endregion

        #region Unused Parameter
        
        [field: SerializeField, HideInInspector] public string Model { get; set; } = string.Empty;
        
        [field: SerializeField, HideInInspector] public int Best_Of { get; set; } = 1;
        
        [field: SerializeField, HideInInspector] public int N { get; set; } = 1;
        
        [field: SerializeField, HideInInspector] public string User { get; set; } = string.Empty;
        
        #endregion
        
        #region TODO
        
        [field: SerializeField, HideInInspector] public bool Echo { get; set; } = false;
        
        [field: SerializeField, HideInInspector] public string[] Stop { get; set; } = Array.Empty<string>();
        
        [field: SerializeField, HideInInspector] public string Suffix { get; set; } = string.Empty;
        
        #endregion
    }
}
