using StableDiffusionRuntimeIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OobaboogaRuntimeIntegration.Example
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
