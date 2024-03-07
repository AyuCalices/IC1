using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using Utils;

namespace OobaboogaRuntimeIntegration
{
    public class TextGeneration : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;

        private CancellationTokenSource token = new();
        
        private void OnDestroy()
        {
            token.Cancel();
        }
        
        [ContextMenu("Generate Completion")]
        public void GenerateCompletion()
        {
            CompletionRequest chatCompletionRequest = new CompletionRequest();
            OobaboogaAPI.CreateCompletionStream(chatCompletionRequest, token, HandleResponse);
        }
        
        [ContextMenu("Generate Chat Completion")]
        public void GenerateChatCompletion()
        {
            ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest();
            OobaboogaAPI.CreateChatCompletionStream(chatCompletionRequest, token, HandleResponse);
        }
        
        private void HandleResponse(APIResponse<List<CompletionResponse>> responses)
        {
            _tmpText.text = string.Join("", responses.Data.Select(r => r.Choices[0].Text));
        }
        
        private void HandleResponse(APIResponse<List<ChatCompletionResponse>> responses)
        {
            _tmpText.text = string.Join("", responses.Data.Select(r => r.Choices[0].Delta.Content));
        }
    }
}
