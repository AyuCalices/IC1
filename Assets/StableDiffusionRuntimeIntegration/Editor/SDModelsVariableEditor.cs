using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEditor;
using UnityEngine;

namespace StableDiffusionRuntimeIntegration.Editor
{
    [CustomEditor(typeof(SDModelsVariable))]
    public class SDModelsVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SDModelsVariable samplersVariable = (SDModelsVariable)target;

            // Draw the drop-down list for the Samplers list
            if (samplersVariable.Models != null && samplersVariable.Models.Length != 0)
            {
                string[] modelNames = new string[samplersVariable.Models.Length];
                for (var index = 0; index < samplersVariable.Models.Length; index++)
                {
                    var samplersVariableModel = samplersVariable.Models[index];
                    modelNames[index] = samplersVariableModel.model_name;
                }

                samplersVariable.CurrentModel = EditorGUILayout.Popup("Model", samplersVariable.CurrentModel, modelNames);
            }
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("Get All Models"))
            {
                // Code to execute when the button is clicked
                samplersVariable.SetupAllModelsAsync();
            }
            
            if (GUILayout.Button("Get Current Model"))
            {
                // Code to execute when the button is clicked
                samplersVariable.GetCurrentModelAsync();
            }
            
            if (GUILayout.Button("Set Current Model"))
            {
                // Code to execute when the button is clicked
                samplersVariable.SetCurrentModelAsync();
            }

            // Apply the changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
