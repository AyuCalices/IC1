using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataStructures.Variables;
using Features.CharacterCard.Scripts;
using Features.Connection.UI;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Button = UnityEngine.UI.Button;

namespace OobaboogaRuntimeIntegration.Example
{
    public class ChatGeneration : MonoBehaviour, IContinueOption
    {
        [SerializeField] private BookDataVariable _currentBook;
        
        [Header("Parameters")] 
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private ChatCompletionParameters _chatCompletionParameters;
        [SerializeField] private GenerationParameters _generationParameters;

        [Header("Text Instantiation")] 
        [SerializeField] private ScrollRect _scrollView;
        [SerializeField] private ChatMessageView _chatMessageView;
        [SerializeField] private Transform _instantiationParent;

        [Header("User")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _regenerateButton;
        [SerializeField] private Button _deleteChatButton;
        [SerializeField] private Button _continueChatButton;

        private BookData _currentBookData;
        private ChatMessageView _initialMessage;
        private readonly List<ChatMessageView> _chatMessageViews = new();
        private readonly CancellationTokenSource _token = new();
        
        public bool Continue_ { get; set; }

        private const string UserRole = "user";
        private const string AssistantRole = "assistant";

        private void Awake()
        {
            _submitButton.onClick.AddListener(CommitUserMessage);
            _regenerateButton.onClick.AddListener(RegenerateLastAssistantMessage);
            _deleteChatButton.onClick.AddListener(DeleteChat);
            _continueChatButton.onClick.AddListener(BotContinuesChat);
        }

        private void Update()
        {
            _submitButton.interactable = _inputField.text != string.Empty;
        }

        private void OnDestroy()
        {
            _token.Cancel();
            
            _submitButton.onClick.RemoveListener(CommitUserMessage);
            _regenerateButton.onClick.RemoveListener(RegenerateLastAssistantMessage);
            _deleteChatButton.onClick.RemoveListener(DeleteChat);
            _continueChatButton.onClick.RemoveListener(BotContinuesChat);
        }
        
        private void OnEnable()
        {
            if (_currentBookData != null)
            {
                UnsetPreviousBook();
            }
            
            SetCurrentBook();
        }

        private void UnsetPreviousBook()
        {
            Destroy(_initialMessage.gameObject);
            _initialMessage = null;
            DestroyAllChatMessages();
            _currentBookData = null;
        }
        
        private void SetCurrentBook()
        {
            _initialMessage = InstantiateMessage(_currentBook.Get().ImagePath, _currentBook.Get().Name2, _currentBook.Get().Greeting);
            
            foreach (Message message in _currentBook.Get().Messages)
            {
                string mappedMassage = message.Role == AssistantRole ? _currentBook.Get().Name2 : _currentBook.Get().Name1;
                _chatMessageViews.Add(InstantiateMessage(_currentBook.Get().ImagePath, mappedMassage, message.Content));
            }

            _currentBookData = _currentBook.Get();
        }

        private void CommitUserMessage()
        {
            Message userMessage = new Message { Role = UserRole, Content = _inputField.text };
            _chatMessageViews.Add(InstantiateMessage(_currentBook.Get().ImagePath, userMessage.Role, userMessage.Content));
            _currentBook.Get().Messages.Add(userMessage);
            _inputField.text = "";
            GenerateChatCompletion();
        }

        private void RegenerateLastAssistantMessage()
        {
            if (_currentBook.Get().Messages.Count > 0 && _currentBook.Get().Messages[^1].Role == AssistantRole)
            {
                RemoveBookMessage(_currentBook.Get().Messages.Count - 1);
                DestroyChatMessage(_chatMessageViews.Count - 1);
            
                GenerateChatCompletion();
            }
        }

        private void DeleteChat()
        {
            _currentBook.Get().Messages.Clear();
            DestroyAllChatMessages();
        }

        private void BotContinuesChat()
        {
            if (_currentBook.Get().Messages.Count == 0 || _currentBook.Get().Messages[^1].Role == AssistantRole)
            {
                GenerateChatCompletion(true);
            }
        }
        
        private void GenerateChatCompletion(bool continue_ = false)
        {
            SetInputActivity(false);
            
            _chatMessageViews.Add(InstantiateMessage(_currentBook.Get().ImagePath, _currentBook.Get().Name2, ""));
            ChatCompletionRequestContainer chatCompletionRequestContainer = new ChatCompletionRequestContainer(_currentBook.Get().Messages, continue_)
            {
                CharacterParameters = _currentBook.Get(),
                ChatCompletionParameters = _chatCompletionParameters,
                GenerationParameters = _generationParameters,
                PresetName = _generationParameters.Preset_Option.UseCustomPreset ? null : _generationParameters.Preset_Option,
                PresetParameters = _generationParameters.Preset_Option.UseCustomPreset ? _generationParameters.Preset_Option.CustomPreset : null
            };
            
            _oobaboogaAPIVariable.Get().CreateChatCompletionStream(chatCompletionRequestContainer, _token, HandleResponse, OnComplete);
        }

        private void HandleResponse((APIResponse Response, List<ChatCompletionResponse> Data) content)
        {
            if (content.Response.IsError) return;
            
            string newText = string.Join("", content.Data.Select(r => r.Choices[0].Delta.Content));
            UpdateMessageContent(_chatMessageViews[^1], newText);
        }

        private void OnComplete()
        {
            _currentBook.Get().Messages.Add(new Message{Role = AssistantRole, Content = _chatMessageViews[^1].Content});
            SetInputActivity(true);
        }
        
        private void SetInputActivity(bool value)
        {
            _inputField.interactable = value;
            _submitButton.interactable = value;
            _regenerateButton.interactable = value;
            _deleteChatButton.interactable = value;
            _continueChatButton.interactable = value;
        }
        
        private ChatMessageView InstantiateMessage(string imagePath, string role, string message)
        {
            ChatMessageView instantiatedMessage = Instantiate(_chatMessageView, _instantiationParent);
            instantiatedMessage.SetImageByPath(imagePath);
            instantiatedMessage.Role = role;
            return UpdateMessageContent(instantiatedMessage, message);
        }

        private ChatMessageView UpdateMessageContent(ChatMessageView instantiatedMessage, string message)
        {
            string starFormatted = TextFormattingHelper.FormatTextQuotation(message, '*', "<i>", "</i>");
            string quotationFormatted = TextFormattingHelper.FormatTextQuotation(starFormatted, '\"', "\"<i>", "</i>\"");
            instantiatedMessage.Content = quotationFormatted;
            _scrollView.normalizedPosition = new Vector2(0, 0);
            return instantiatedMessage;
        }
        
        private void RemoveBookMessage(int index)
        {
            _currentBook.Get().Messages.RemoveAt(index);
        }

        private void DestroyChatMessage(int index)
        {
            Destroy(_chatMessageViews[index].gameObject);
            _chatMessageViews.RemoveAt(index);
        }

        private void DestroyAllChatMessages()
        {
            for (var i = _chatMessageViews.Count - 1; i >= 0; i--)
            {
                DestroyChatMessage(i);
            }
        }
    }
}
