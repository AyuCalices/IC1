using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CharacterCard.Scripts
{
    public class BookSelectElement : MonoBehaviour
    {
        [Header("Visual")]
        //TODO: cover image
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
            
            _playButton.onClick.AddListener(() => onPressPlayButton.Invoke(bookData));
            _editButton.onClick.AddListener(() => onPressEditButton.Invoke(bookData));
            _deleteButton.onClick.AddListener(() => onRemoveAction.Invoke(bookData));
            
            //_bookCoverImage.sprite = 
            _title.text = bookData.Name2;
            _context.text = bookData.Context;
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _editButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
        }
    }
}
