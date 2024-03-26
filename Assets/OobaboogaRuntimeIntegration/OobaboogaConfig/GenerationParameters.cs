using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaGenerationParameters", menuName = "Oobabooga/GenerationParameters")]
    public class GenerationParameters : ScriptableObject, IGenerationParameters
    {
        //TODO:
        [field: SerializeField, Header("General")] public PresetOption Preset_Option { get; set; }
        
        [field: SerializeField, Space] public int Seed { get; set; } = -1;
        
        [field: SerializeField] public int Max_Tokens { get; set; } = 512;
        
        [field: SerializeField] public bool Auto_Max_New_Tokens { get; set; } = false;
        
        [field: SerializeField] public int Truncation_Length { get; set; } = 2048;
        
        [field: SerializeField] public int Prompt_Lookup_Num_Tokens { get; set; } = 0;
        
        [field: SerializeField, Header("Sequence")] public bool Ban_Eos_Token { get; set; } = false;
        
        [field: SerializeField] public bool Add_Bos_Token { get; set; } = true;
        
        [field: SerializeField] public bool Skip_Special_Tokens { get; set; } = true;
        
        [field: SerializeField, Header("Utility")] public int Max_Tokens_Second { get; set; } = 0;
        
        [field: SerializeField, Header("Formatting"), TextArea(3, 50)] public string Grammar_String { get; set; } = string.Empty;
        
        [field: SerializeField, Header("Conditional"), TextArea(3, 50)] public string Negative_Prompt { get; set; } = string.Empty;
        
        [field: SerializeField, TextArea(3, 50)] public string Custom_Token_Bans { get; set; } = string.Empty;
    }
}
