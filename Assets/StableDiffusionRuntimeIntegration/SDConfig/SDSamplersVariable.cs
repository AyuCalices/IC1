using UnityEditor;
using UnityEngine;

namespace StableDiffusionRuntimeIntegration
{
    [CreateAssetMenu]
    public class SDSamplersVariable : ScriptableObject
    {
        [SerializeReference] private string[] _samplers;
        public string[] Samplers => _samplers;
        
        [SerializeField] private int _currentSampler;
        public int CurrentSampler 
        {
            get => _currentSampler;
            set => _currentSampler = value;
        }
        
        public string GetCurrent => _samplers[CurrentSampler];

        [ContextMenu("Setup Sampler")]
        public async void SetupSampler()
        {
            SDOutSampler[] fetchedSamplers = await StableDiffusionRequests.GetSamplersAsync();

            _samplers = new string[fetchedSamplers.Length];
            for (var i = 0; i < fetchedSamplers.Length; i++)
            {
                _samplers[i] = fetchedSamplers[i].name;
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
