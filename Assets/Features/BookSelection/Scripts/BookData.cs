using System;
using System.Collections.Generic;
using Features._Core.API.Oobabooga.Scripts;
using UnityEngine;

namespace Features.BookSelection.Scripts
{
    [Serializable]
    public class BookData : ICharacterParameters, IMessageWrapper
    {
        public event Action OnValueChanged;
        
        public BookData(string userName, string imagePathUser, string userBio, string assistantName, string imagePathAssistant, string context, string greeting)
        {
            Messages = new List<Message>();
            Name1 = userName;
            ImagePathUser = imagePathUser;
            User_Bio = userBio;
            Name2 = assistantName;
            ImagePathAssistant = imagePathAssistant;
            Context = context;
            Greeting = greeting;
        }

        [SerializeField] private List<Message> _messages;
        public List<Message> Messages 
        {
            get => _messages;
            set
            {
                _messages = value;
                OnValueChanged?.Invoke();
            }
        }

        [SerializeField] private string _name1;
        public string Name1 
        {
            get => _name1;
            set
            {
                _name1 = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _imagePathUser;
        public string ImagePathUser
        {
            get => _imagePathUser;
            set
            {
                _imagePathUser = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _userBio;
        public string User_Bio 
        {
            get => _userBio;
            set
            {
                _userBio = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _name2;
        public string Name2 
        {
            get => _name2;
            set
            {
                _name2 = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _imagePathAssistant;
        public string ImagePathAssistant
        {
            get => _imagePathAssistant;
            set
            {
                _imagePathAssistant = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _context;
        public string Context 
        {
            get => _context;
            set
            {
                _context = value;
                OnValueChanged?.Invoke();
            }
        }
        
        [SerializeField] private string _greeting;
        public string Greeting 
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnValueChanged?.Invoke();
            }
        }
    }
}
