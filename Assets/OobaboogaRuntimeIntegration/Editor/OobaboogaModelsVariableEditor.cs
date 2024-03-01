using OobaboogaRuntimeIntegration.OobaboogaConfig;
using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEditor;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.Editor
{
    [CustomEditor(typeof(OobaboogaModelsVariable))]
    public class OobaboogaModelsVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            OobaboogaModelsVariable samplersVariable = (OobaboogaModelsVariable)target;

            // Draw the drop-down list for the Samplers list
            if (samplersVariable.ModelList != null && samplersVariable.ModelList.model_names.Count != 0)
            {
                samplersVariable.CurrentModelIndex = EditorGUILayout.Popup(
                    "Model", samplersVariable.CurrentModelIndex, samplersVariable.ModelList.model_names.ToArray());
            }
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("Get All Models"))
            {
                // Code to execute when the button is clicked
                samplersVariable.SetupAllModels();
            }
            
            if (GUILayout.Button("Get Current Model"))
            {
                // Code to execute when the button is clicked
                samplersVariable.GetCurrentModel();
            }
            
            if (GUILayout.Button("Load Current Model"))
            {
                // Code to execute when the button is clicked
                samplersVariable.LoadModel();
            }
            
            if (GUILayout.Button("Unload Current Model"))
            {
                // Code to execute when the button is clicked
                samplersVariable.UnloadModel();
            }

            // Apply the changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
