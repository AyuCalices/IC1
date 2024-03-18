using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Utils;

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
        public async Task<(APIResponse Response, SDOutSampler[] Data)> SetupSampler()
        {
            var content = await Automatic1111API.GetSamplersAsync();
            if (content.Response.IsError) return content;
            
            SDOutSampler[] fetchedSamplers = content.Data;

            _samplers = new string[fetchedSamplers.Length];
            for (var i = 0; i < fetchedSamplers.Length; i++)
            {
                _samplers[i] = fetchedSamplers[i].name;
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

            return content;
        }
    }
}
