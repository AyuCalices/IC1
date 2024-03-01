using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace OobaboogaRuntimeIntegration
{
    public class TextGeneration : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        
        [ContextMenu("GenerateText")]
        public async void GenerateText()
        {
            await InternalGenerateText();
        }

        private async Task InternalGenerateText()
        {
            GeneratedSettings generatedSettings = new GeneratedSettings();

            await OobaboogaAPI.DispatchRequest(generatedSettings, 
                HandleResponse, 
                x => Debug.Log(x));
        }
        
        private void HandleResponse(List<CompletionResponse> responses)
        {
            _tmpText.text = string.Join("", responses.Select(r => r.choices[0].Text));
        }
    }
}
