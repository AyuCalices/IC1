using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;
using Utils;

namespace OobaboogaRuntimeIntegration
{
    public class TextGeneration : MonoBehaviour
    {
        [SerializeField] private ChatCompletionParameters _chatCompletionParameters;
        [SerializeField] private CompletionParameters _completionParameters;
        [SerializeField] private GenerationParameters _generationParameters;
        [SerializeField] private TMP_Text _tmpText;

        private CancellationTokenSource token = new();
        
        private void OnDestroy()
        {
            token.Cancel();
        }
        
        [ContextMenu("Generate Completion")]
        public void GenerateCompletion()
        {
            CompletionRequestContainer chatCompletionRequestContainer = new CompletionRequestContainer()
            {
                CompletionParameters = _completionParameters,
                GenerationParameters = _generationParameters,
                PresetName = _generationParameters.Preset_Option.UseCustomPreset ? null : _generationParameters.Preset_Option,
                PresetParameters = _generationParameters.Preset_Option.UseCustomPreset ? _generationParameters.Preset_Option.CustomPreset : null
            };
            OobaboogaAPI.CreateCompletionStream(chatCompletionRequestContainer, token, HandleResponse);
        }
        
        [ContextMenu("Generate Chat Completion")]
        public void GenerateChatCompletion()
        {
            ChatCompletionRequestContainer chatCompletionRequestContainer = new ChatCompletionRequestContainer()
            {
                ChatCompletionParameters = _chatCompletionParameters,
                CharacterName = _chatCompletionParameters.Character.UseCustomCharacter ? null : _chatCompletionParameters.Character,
                CharacterParameters = _chatCompletionParameters.Character.UseCustomCharacter ? _chatCompletionParameters.Character.CustomCharacter : null,
                GenerationParameters = _generationParameters,
                PresetName = _generationParameters.Preset_Option.UseCustomPreset ? null : _generationParameters.Preset_Option,
                PresetParameters = _generationParameters.Preset_Option.UseCustomPreset ? _generationParameters.Preset_Option.CustomPreset : null
            };
            OobaboogaAPI.CreateChatCompletionStream(chatCompletionRequestContainer, token, HandleResponse);
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
