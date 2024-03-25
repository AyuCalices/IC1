using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.CharacterCard.Scripts
{
    public class BookLoader : MonoBehaviour
    {
        [SerializeField] private BookSelectElement _bookPrefab;
        [SerializeField] private Transform _instantiationParent;
        [SerializeField] private Transform _lastElement;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<BookData> _onPressPlayBook;
        [SerializeField] private UnityEvent<BookData> _onPressEditBook;
        
        private BookSerializer _bookSerializer;
        private readonly List<BookSelectElement> _instantiatedElements = new();
        
        private void Start()
        {
            _bookSerializer = new BookSerializer();
            _bookSerializer.Load();
            foreach (BookData bookData in _bookSerializer.BookDataList)
            {
                BookSelectElement instantiatedElement = Instantiate(_bookPrefab, _instantiationParent);
                instantiatedElement.Initialize(bookData, _onPressPlayBook, _onPressEditBook, RemoveBook);
                _instantiatedElements.Add(instantiatedElement);
                _lastElement.SetAsLastSibling();
            }
        }

        private void OnDestroy()
        {
            _bookSerializer.Save();
        }

        public void AddNewBook(BookData bookData)
        {
            _bookSerializer.AddBook(bookData);
            BookSelectElement instantiatedElement = Instantiate(_bookPrefab, _instantiationParent);
            instantiatedElement.Initialize(bookData, _onPressPlayBook, _onPressEditBook, _bookSerializer.RemoveBook);
            _lastElement.SetAsLastSibling();
        }

        public void RemoveBook(BookData bookData)
        {
            _bookSerializer.RemoveBook(bookData);

            BookSelectElement bookSelectElement = _instantiatedElements.Find(x => x.ContainedBook == bookData);
            if (bookSelectElement != null)
            {
                _instantiatedElements.Remove(bookSelectElement);
            }
            Destroy(bookSelectElement.gameObject);
        }
    }
}
