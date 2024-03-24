using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.Example
{
    public class CharacterSetupView : CharacterData
    {
        [Header("Load Scriptable Object")] 
        [SerializeField] private CharacterParameters _characterParameters;
    
        [Header("References")]
        [SerializeField] private TMP_InputField _ownName;
        [SerializeField] private TMP_InputField _characterName;
        [SerializeField] private TMP_InputField _context;
        [SerializeField] private TMP_InputField _greeting;
        
        public override string Name1 
        { 
            get => _ownName.text;
            set => _ownName.text = value;
        }

        public override string User_Bio { get; set; } = string.Empty;

        public override string Name2 
        { 
            get => _characterName.text;
            set => _characterName.text = value;
        }
        
        public override string Context 
        { 
            get => _context.text;
            set => _context.text = value;
        }
        
        public override string Greeting 
        { 
            get => _greeting.text;
            set => _greeting.text = value;
        }
    
        private void Awake()
        {
            if (_characterParameters != null)
            {
                _ownName.text = _characterParameters.Name1;
                _characterName.text = _characterParameters.Name2;
                _context.text = _characterParameters.Context;
                _greeting.text = _characterParameters.Greeting;
            }
        }
    }
}
