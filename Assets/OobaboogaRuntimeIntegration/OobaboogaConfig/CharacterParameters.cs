using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaCharacterParameters", menuName = "Oobabooga/Character/Parameters")]
    public class CharacterParameters : ScriptableObject, ICharacterParameters
    {
        [SerializeField] private string _userName = string.Empty;
        public string Name1 
        { 
            get => _userName;
            set => _userName = value;
        }
        
        [SerializeField] private string _userBio = string.Empty;
        public string User_Bio 
        { 
            get => _userBio; 
            set => _userBio = value; 
        }
        
        [SerializeField] private string _botName = string.Empty;
        public string Name2 
        { 
            get => _botName;
            set => _botName = value;
        }
        
        [field: SerializeField, TextArea(3, 50)] public string Context { get; set; } = string.Empty;
        
        [field: SerializeField, TextArea(3, 50)] public string Greeting { get; set; } = string.Empty;
    }
}
