using UnityEngine;

namespace OobaboogaRuntimeIntegration.Example
{
    public abstract class CharacterData : MonoBehaviour, ICharacterParameters
    {
        public abstract string Name1 { get; set; }
        public abstract string User_Bio { get; set; }
        public abstract string Name2 { get; set; }
        public abstract string Context { get; set; }
        public abstract string Greeting { get; set; }
    }
}
