using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace OobaboogaRuntimeIntegration.Example
{
    public class ChatGeneration : MonoBehaviour, IMessageWrapper
    {
        [Header("Parameters")] 
        [SerializeField] private CharacterData _character;
        [SerializeField] private ChatCompletionParameters _chatCompletionParameters;
        [SerializeField] private GenerationParameters _generationParameters;
        
        [Header("Text Instantiation")]
        [SerializeField] private ChatMessageView _chatMessageView;
        [SerializeField] private Transform _instantiationParent;

        [Header("User")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _regenerateButton;
        
        private readonly List<ChatMessageView> _chatMessageViews = new();
        private readonly CancellationTokenSource _token = new();

        //TODO: messages change with different character
        public List<Message> Messages { get; set; } = new();
        //TODO: implement input method for continue
        public bool Continue_ { get; set; } = false;

        private void Awake()
        {
            _inputField.onSubmit.AddListener(CommitMessage);
            _regenerateButton.onClick.AddListener(RegenerateMessage);
        }

        private void Start()
        {
            ChatMessageView instantiatedMessage = Instantiate(_chatMessageView, _instantiationParent);
            instantiatedMessage.Role = _character.Name2;
            
            string starFormatted = FormatTextQuotation(_character.Greeting, '*', "<i>", "</i>");
            string quotationFormatted = FormatTextQuotation(starFormatted, '\"', "\"<i>", "</i>\"");
            instantiatedMessage.Content = quotationFormatted;
        }

        private void CommitMessage(string arg0)
        {
            Messages.Add(new Message{Role = "user", Content = _inputField.text});
            ChatMessageView instantiatedMessage = Instantiate(_chatMessageView, _instantiationParent);
            instantiatedMessage.Role = _character.Name1;
            instantiatedMessage.Content = _inputField.text;
            _inputField.text = "";
            GenerateChatCompletion();
        }

        private void RegenerateMessage()
        {
            if (Messages[^1].Role == "assistant")
            {
                Messages.RemoveAt(Messages.Count - 1);
                
                Destroy(_chatMessageViews[^1].gameObject);
                _chatMessageViews.RemoveAt(_chatMessageViews.Count - 1);
                
                GenerateChatCompletion();
            }
        }
        
        private void OnDestroy()
        {
            _token.Cancel();
            _inputField.onSubmit.RemoveAllListeners();
        }
        
        [ContextMenu("Generate Chat Completion")]
        public void GenerateChatCompletion()
        {
            ChatMessageView chatMessageView = Instantiate(_chatMessageView, _instantiationParent);
            _chatMessageViews.Add(chatMessageView);
            chatMessageView.Role = _character.Name2;
            
            ChatCompletionRequestContainer chatCompletionRequestContainer = new ChatCompletionRequestContainer(this, _character)
            {
                ChatCompletionParameters = _chatCompletionParameters,
                GenerationParameters = _generationParameters,
                PresetName = _generationParameters.Preset_Option.UseCustomPreset ? null : _generationParameters.Preset_Option,
                PresetParameters = _generationParameters.Preset_Option.UseCustomPreset ? _generationParameters.Preset_Option.CustomPreset : null
            };
            
            OobaboogaAPI.CreateChatCompletionStream(chatCompletionRequestContainer, _token, HandleResponse, OnComplete);
        }
        
        private void HandleResponse(APIResponse<List<ChatCompletionResponse>> responses)
        {
            _chatMessageViews[^1].Content = string.Join("", responses.Data.Select(r => r.Choices[0].Delta.Content));
            
            string starFormatted = FormatTextQuotation(_chatMessageViews[^1].Content, '*', "<i>", "</i>");
            string quotationFormatted = FormatTextQuotation(starFormatted, '\"', "\"<i>", "</i>\"");
            _chatMessageViews[^1].Content = quotationFormatted;
        }

        private void OnComplete()
        {
            Messages.Add(new Message{Role = "assistant", Content = _chatMessageViews[^1].Content});
        }
        
        private string FormatTextQuotation(string originalText, char separator, string prefix, string suffix)
        {
            string[] parts = originalText.Split(separator);
            string formattedText = "";

            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i])) continue;
                
                if (i % 2 == 0)
                {
                    formattedText += "" + parts[i].Trim() + "\n\n";
                }
                else
                {
                    formattedText += prefix + parts[i].Trim() + suffix + "\n\n";
                }
            }

            return formattedText;
        }
    }
}
