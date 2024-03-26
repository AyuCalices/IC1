using System;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaPresetParameters", menuName = "Oobabooga/Preset/Parameters")]
    public class PresetParameters : ScriptableObject, IPresetParameters
    {
        [field: SerializeField] public float Temperature { get; set; } = 1;
        
        [field: SerializeField] public float Top_P { get; set; } = 1;
        
        [field: SerializeField] public float Min_P { get; set; } = 0;
        
        [field: SerializeField] public int Top_K { get; set; } = 0;
        
        [field: SerializeField] public float Repetition_Penalty { get; set; } = 1;
        
        [field: SerializeField] public int Presence_Penalty { get; set; } = 0;
        
        [field: SerializeField] public int Frequency_Penalty { get; set; } = 0;
        
        [field: SerializeField] public int Repetition_Penalty_Range { get; set; } = 1024;
        
        [field: SerializeField] public float Typical_P { get; set; } = 1;
        
        [field: SerializeField] public float Tfs { get; set; } = 1;
        
        [field: SerializeField] public int Top_A { get; set; } = 0;
        
        [field: SerializeField] public float Epsilon_Cutoff { get; set; } = 0;
        
        [field: SerializeField] public float Eta_Cutoff { get; set; } = 0;
        
        [field: SerializeField] public float Guidance_Scale { get; set; } = 1;
        
        [field: SerializeField] public float Penalty_Alpha { get; set; } = 0;
        
        [field: SerializeField] public int Mirostat_Mode { get; set; } = 0;
        
        [field: SerializeField] public float Mirostat_Tau { get; set; } = 5;
        
        [field: SerializeField] public float Mirostat_Eta { get; set; } = 0.1f;
        
        [field: SerializeField] public float Smoothing_Factor { get; set; } = 0;
        
        [field: SerializeField] public float Smoothing_Curve { get; set; } = 1f;
        
        [field: SerializeField] public bool Dynamic_Temperature { get; set; } = false;
        
        [field: SerializeField] public float Dynatemp_Low { get; set; } = 1;
        
        [field: SerializeField] public float Dynatemp_High { get; set; } = 1;
        
        [field: SerializeField] public float DynaTemp_Exponent { get; set; } = 1;
        
        [field: SerializeField] public bool Temperature_Last { get; set; } = false;
        
        [field: SerializeField] public bool Do_Sample { get; set; } = true;
        
        [field: SerializeField] public float Encoder_Repetition_Penalty { get; set; } = 1;
        
        [field: SerializeField] public int No_Repeat_Ngram_Size { get; set; } = 0;
        
        [field: SerializeField] public int Min_Length { get; set; } = 1;
        
        [field: SerializeField] public int Num_Beams { get; set; } = 1;
        
        [field: SerializeField] public float Length_Penalty { get; set; } = 1;
        
        [field: SerializeField] public bool Early_Stopping { get; set; } = false;
        
        [field: SerializeField] public string[] Sampler_Priority { get; set; } = {
            "temperature", 
            "dynamic_temperature",
            "quadratic_sampling",
            "top_k",
            "top_p",
            "typical_p",
            "epsilon_cutoff",
            "eta_cutoff",
            "tfs",
            "top_a",
            "min_p",
            "mirostat"
        };
    }
}
