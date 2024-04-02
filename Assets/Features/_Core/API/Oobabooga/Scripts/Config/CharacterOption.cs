using UnityEngine;

namespace Features._Core.API.Oobabooga.Scripts.Config
{
    [CreateAssetMenu(fileName = "OobaboogaCharacterOption", menuName = "Oobabooga/Character/Option")]
    public class CharacterOption : ScriptableObject, ICharacterName
    {
        [field: SerializeField] public bool UseCustomCharacter { get; set; } = false;
        [field: SerializeField] public string Character { get; set; } = string.Empty;
        [field: SerializeField] public CharacterParameters CustomCharacter { get; set; }
    }
}
