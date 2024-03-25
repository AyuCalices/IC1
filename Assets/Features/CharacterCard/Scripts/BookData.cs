using System;
using System.Collections.Generic;
using OobaboogaRuntimeIntegration;
using UnityEngine;

namespace Features.CharacterCard.Scripts
{
    [Serializable]
    public class BookData : ICharacterParameters, IMessageWrapper
    {
        public BookData(string imagePath, string userName, string userBio, string assistantName, string context, string greeting)
        {
            ImagePath = imagePath;
            Messages = new List<Message>();
            Name1 = userName;
            User_Bio = userBio;
            Name2 = assistantName;
            Context = context;
            Greeting = greeting;
        }
        
        [field: SerializeField] public string ImagePath { get; set; }
        [field: SerializeField] public List<Message> Messages { get; set; }
        [field: SerializeField] public string Name1 { get; set; }
        [field: SerializeField] public string User_Bio { get; set; }
        [field: SerializeField] public string Name2 { get; set; }
        [field: SerializeField] public string Context { get; set; }
        [field: SerializeField] public string Greeting { get; set; }
    }
}
