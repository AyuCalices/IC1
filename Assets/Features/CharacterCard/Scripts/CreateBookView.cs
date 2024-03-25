using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CharacterCard.Scripts
{
    public class CreateBookView : MonoBehaviour
    {
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
            _createButton.onClick.AddListener(InvokeOnCreate);
            _createAndStartButton.onClick.AddListener(InvokeOnCreateAndStart);
        }

        private void OnDestroy()
        {
            _createButton.onClick.RemoveListener(InvokeOnCreate);
            _createAndStartButton.onClick.RemoveListener(InvokeOnCreateAndStart);
        }

        private void InvokeOnCreate()
        {
            BookData newBook = new BookData(string.Empty, _userName.text, _userBio.text, 
                _characterName.text, _context.text, _greeting.text);
            _onCreate.Invoke(newBook);
        }
        
        private void InvokeOnCreateAndStart()
        {
            BookData newBook = new BookData(string.Empty, _userName.text, _userBio.text, 
                _characterName.text, _context.text, _greeting.text);
            _onCreateAndPlay.Invoke(newBook);
        }
    }
}
