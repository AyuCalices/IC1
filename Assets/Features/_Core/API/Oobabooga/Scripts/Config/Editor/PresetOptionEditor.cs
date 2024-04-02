using Features._Core.API.Oobabooga.Scripts.Config;
using UnityEditor;

namespace OobaboogaRuntimeIntegration.Editor
{
    [CustomEditor(typeof(PresetOption))]
    public class PresetOptionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Cast the target to PresetOption
            PresetOption presetOption = (PresetOption)target;

            presetOption.UseCustomPreset = EditorGUILayout.Toggle("Use Custom Preset", presetOption.UseCustomPreset);

            if (presetOption.UseCustomPreset)
            {
                presetOption.CustomPreset = EditorGUILayout.ObjectField("Custom Character",
                    presetOption.CustomPreset, typeof(PresetParameters), false) as PresetParameters;
            }
            else
            {
                presetOption.Preset = EditorGUILayout.TextField("Preset", presetOption.Preset);
            }
        }
    }
}
