using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionRuntimeIntegration
{
    public class StableDiffusionImageReference : Text2ImageTaskCallback
    {
        [SerializeField] private Image _image;
        [SerializeField] private bool _clearOnEnable;

        public string CurrentPath { get; private set; }

        private void OnEnable()
        {
            if (_clearOnEnable)
            {
                UnloadImage();
            }
        }

        public override void OnPerformTaskCallback(Task task) { }

        public override async void OnTaskCompletedCallback(string imagePath)
        {
            LoadImageFromPath(imagePath);
        }

        public void LoadImageFromPath(string imagePath)
        {
            TryLoadSpriteFromPathAsync(imagePath, sprite =>
            {
                CurrentPath = imagePath;
                _image.sprite = sprite;
            });
        }

        public void UnloadImage()
        {
            _image.sprite = null;
            CurrentPath = string.Empty;
        }

        private async void TryLoadSpriteFromPathAsync(string path, Action<Sprite> onSuccessful, Action onFailed = null)
        {
            if (File.Exists(path))
            {
                Texture2D texture = new Texture2D(2, 2);
                byte[] fileImage = await File.ReadAllBytesAsync(path);
                texture.LoadImage(fileImage);
                texture.Apply();
                
                onSuccessful.Invoke(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
            }

            onFailed?.Invoke();
        }
    }
}
