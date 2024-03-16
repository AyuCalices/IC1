using OobaboogaRuntimeIntegration.Example;
using UnityEditor;

namespace OobaboogaRuntimeIntegration.Editor
{
    [CustomEditor(typeof(SetMinTextHeight))]
    public class SetMinTextHeightEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SetMinTextHeight setMinTextHeight = (SetMinTextHeight)target;

            setMinTextHeight.UpdateHeight();
        }
    }
}
