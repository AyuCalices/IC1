using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace OobaboogaRuntimeIntegration
{
    public class TextGeneration : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        
        [ContextMenu("Generate Completion")]
        public void GenerateCompletion()
        {
            CompletionRequest chatCompletionRequest = new CompletionRequest();
            OobaboogaAPI.CreateCompletionStream(chatCompletionRequest, HandleResponse);
        }
        
        [ContextMenu("Generate Chat Completion")]
        public void GenerateChatCompletion()
        {
            ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest();
            OobaboogaAPI.CreateChatCompletionStream(chatCompletionRequest, HandleResponse);
        }
        
        private void HandleResponse(List<CompletionResponse> responses)
        {
            _tmpText.text = string.Join("", responses.Select(r => r.Choices[0].Text));
        }
        
        private void HandleResponse(List<ChatCompletionResponse> responses)
        {
            _tmpText.text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
        }
    }
}
