using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OobaboogaRuntimeIntegration.Example
{
    public class ChatMessageView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _role;
        [SerializeField] private TMP_Text _content;

        public Sprite Image
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }
        
        public string Role
        {
            get => _role.text;
            set => _role.text = value;
        }
        
        public string Content
        {
            get => _content.text;
            set => _content.text = value;
        }
    }
}
