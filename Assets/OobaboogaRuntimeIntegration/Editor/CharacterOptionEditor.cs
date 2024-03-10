using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEditor;

namespace OobaboogaRuntimeIntegration.Editor
{
    [CustomEditor(typeof(CharacterOption))]
    public class CharacterOptionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Cast the target to PresetOption
            CharacterOption presetOption = (CharacterOption)target;

            presetOption.UseCustomCharacter = EditorGUILayout.Toggle("Use Custom Character", presetOption.UseCustomCharacter);

            if (presetOption.UseCustomCharacter)
            {
                presetOption.CustomCharacter = EditorGUILayout.ObjectField("Custom Character",
                    presetOption.CustomCharacter, typeof(CharacterParameters), false) as CharacterParameters;
            }
            else
            {
                presetOption.Character = EditorGUILayout.TextField("Preset", presetOption.Character);
            }
        }
    }
}
