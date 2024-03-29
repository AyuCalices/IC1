using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEditor;
using UnityEngine;

namespace StableDiffusionRuntimeIntegration.Editor
{
    [CustomEditor(typeof(SDSamplersVariable))]
    public class SDSamplersVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SDSamplersVariable samplersVariable = (SDSamplersVariable)target;

            // Draw the drop-down list for the Samplers list
            if (samplersVariable.Samplers != null)
            {
                samplersVariable.CurrentSampler = EditorGUILayout.Popup("Sampler", samplersVariable.CurrentSampler, samplersVariable.Samplers);
            }
            
            if (GUILayout.Button("Setup Sampler"))
            {
                // Code to execute when the button is clicked
                samplersVariable.SetupSampler();
            }

            // Apply the changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
