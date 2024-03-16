using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    /// <summary>
    /// A character defined under text-generation-webui/characters. If not set, the default \"Assistant\" character will be used.
    /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
    /// </summary>
    [CreateAssetMenu(fileName = "OobaboogaCharacterOption", menuName = "Oobabooga/Character/Option")]
    public class CharacterOption : ScriptableObject, ICharacterName
    {
        [field: SerializeField] public bool UseCustomCharacter { get; set; } = false;
        [field: SerializeField] public string Character { get; set; } = string.Empty;
        [field: SerializeField] public CharacterParameters CustomCharacter { get; set; }
    }
}
