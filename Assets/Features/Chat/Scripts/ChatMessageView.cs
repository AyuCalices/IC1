using Features.BookCreation.Scripts.ImageGeneration;
using TMPro;
using UnityEngine;

namespace Features.Chat.Scripts
{
    public class ChatMessageView : MonoBehaviour
    {
        [SerializeField] private StableDiffusionImageReference _image;
        [SerializeField] private TMP_Text _role;
        [SerializeField] private TMP_Text _content;

        public void SetImageByPath(string path)
        {
            _image.LoadImageFromPath(path);
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
