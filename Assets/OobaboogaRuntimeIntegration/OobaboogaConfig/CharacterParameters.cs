using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaCharacterParameters", menuName = "Oobabooga/Character/Parameters")]
    public class CharacterParameters : ScriptableObject, ICharacterParameters
    {
        /// <summary>
        /// Your name (the user). By default, it's \"You\"." (User_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _userName = string.Empty;
        public string Name1 
        { 
            get => _userName;
            set => _userName = value;
        }

        /// <summary>
        /// The user description/personality.
        /// </summary>
        [SerializeField] private string _userBio = string.Empty;
        public string User_Bio 
        { 
            get => _userBio; 
            set => _userBio = value; 
        }

        /// <summary>
        /// Overwrites the value set by character field. (Bot_Name)
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string _botName = string.Empty;
        public string Name2 
        { 
            get => _botName;
            set => _botName = value;
        }

        /// <summary>
        /// Overwrites the value set by character field.
        /// A string that is always at the top of the prompt. It never gets truncated.
        /// It usually defines the bot's personality and some key elements of the conversation.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Context { get; set; } = string.Empty;

        /// <summary>
        /// Overwrites the value set by character field.
        /// An opening message for the bot. When set, it appears whenever you start a new chat.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Greeting { get; set; } = string.Empty;
    }
}
