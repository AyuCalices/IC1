using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.BookSelection.Scripts
{
    public class BookSelectElement : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private Image _bookCoverImage;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _context;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _deleteButton;
        
        public BookData ContainedBook { get; private set; }

        public void Initialize(BookData bookData, UnityEvent<BookData> onPressPlayButton, UnityEvent<BookData> onPressEditButton, Action<BookData> onRemoveAction)
        {
            ContainedBook = bookData;
            ContainedBook.OnValueChanged += UpdateData;
            _playButton.onClick.AddListener(() => onPressPlayButton.Invoke(bookData));
            _editButton.onClick.AddListener(() => onPressEditButton.Invoke(bookData));
            _deleteButton.onClick.AddListener(() => onRemoveAction.Invoke(bookData));

            UpdateData();
        }
        

        private void OnDestroy()
        {
            ContainedBook.OnValueChanged -= UpdateData;
            _playButton.onClick.RemoveAllListeners();
            _editButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
        }
        
        //TODO: duplicate code
        private async void UpdateData()
        {
            if (File.Exists(ContainedBook.ImagePathAssistant))
            {
                Texture2D texture = new Texture2D(2, 2);
                byte[] fileImage = await File.ReadAllBytesAsync(ContainedBook.ImagePathAssistant);
                texture.LoadImage(fileImage);
                texture.Apply();
                            
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                _bookCoverImage.sprite = sprite;
            }
            
            _title.text = ContainedBook.Name2;
            _context.text = ContainedBook.Context;
        }
    }
}
