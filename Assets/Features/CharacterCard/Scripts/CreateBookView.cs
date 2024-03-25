using System;
using Features.Connection.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CharacterCard.Scripts
{
    public class CreateBookView : MonoBehaviour
    {
        [SerializeField] private BookDataVariable _bookDataToLoad;
        [SerializeField] private ButtonToggleGroupManager _buttonToggleGroupManager;
        
        [Header("Cancel")]
        [SerializeField] private Button _cancelButton;
        [SerializeField] private UnityEvent _onCancel;
        
        [Header("Create")]
        [SerializeField] private Button _createButton;
        [SerializeField] private UnityEvent<BookData> _onCreate;
        
        [Header("Create And Play")]
        [SerializeField] private Button _createAndStartButton;
        [SerializeField] private UnityEvent<BookData> _onCreateAndPlay;
        
        [Header("References")]
        [SerializeField] private TMP_InputField _userName;
        [SerializeField] private TMP_InputField _userBio;
        [SerializeField] private TMP_InputField _characterName;
        [SerializeField] private TMP_InputField _context;
        [SerializeField] private TMP_InputField _greeting;

        private void Awake()
        {
            _cancelButton.onClick.AddListener(InvokeOnCancel);
            _createButton.onClick.AddListener(InvokeOnCreate);
            _createAndStartButton.onClick.AddListener(InvokeOnCreateAndStart);
        }
        
        private void OnDestroy()
        {
            _cancelButton.onClick.RemoveListener(InvokeOnCancel);
            _createButton.onClick.RemoveListener(InvokeOnCreate);
            _createAndStartButton.onClick.RemoveListener(InvokeOnCreateAndStart);
        }

        private void OnEnable()
        {
            if (_bookDataToLoad == null || _bookDataToLoad.Get() == null)
            {
                _userName.text = string.Empty;
                _userBio.text = string.Empty;
                _characterName.text = string.Empty;
                _context.text = string.Empty;
                _greeting.text = string.Empty;
            }
            else
            {
                _userName.text = _bookDataToLoad.Get().Name1;
                _userBio.text = _bookDataToLoad.Get().User_Bio;
                _characterName.text = _bookDataToLoad.Get().Name2;
                _context.text = _bookDataToLoad.Get().Context;
                _greeting.text = _bookDataToLoad.Get().Context;
            }
        }
        
        private void InvokeOnCancel()
        {
            _onCancel.Invoke();
        }

        private void InvokeOnCreate()
        {
            string characterName = _buttonToggleGroupManager.IsToggleActive ? _characterName.text : "Narrator";
            BookData newBook = new BookData(string.Empty, _userName.text, _userBio.text, 
                characterName, _context.text, _greeting.text);
            _onCreate.Invoke(newBook);
        }
        
        private void InvokeOnCreateAndStart()
        {
            string characterName = _buttonToggleGroupManager.IsToggleActive ? _characterName.text : "Narrator";
            BookData newBook = new BookData(string.Empty, _userName.text, _userBio.text, 
                characterName, _context.text, _greeting.text);
            _onCreateAndPlay.Invoke(newBook);
        }
    }
}
