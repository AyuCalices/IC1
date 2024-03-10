using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaPresetOption", menuName = "Oobabooga/Preset/Option")]
    public class PresetOption : ScriptableObject, IPresetName
    {
        [field: SerializeField] public bool UseCustomPreset { get; set; } = false;
        [field: SerializeField] public string Preset { get; set; } = string.Empty;
        [field: SerializeField] public PresetParameters CustomPreset { get; set; }
    }
}
