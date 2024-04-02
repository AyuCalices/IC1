using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.BookSelection.Scripts
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
                InstantiateBook(bookData);
            }
        }
        
        private void OnDestroy()
        {
            _bookSerializer.Save();
        }

        public void AddNewBook(BookData bookData)
        {
            if (_bookSerializer.AddBook(bookData))
            {
                InstantiateBook(bookData);
            }
        }

        private void InstantiateBook(BookData bookData)
        {
            BookSelectElement instantiatedElement = Instantiate(_bookPrefab, _instantiationParent);
            instantiatedElement.Initialize(bookData, _onPressPlayBook, _onPressEditBook, RemoveBook);
            _instantiatedElements.Add(instantiatedElement);
            _lastElement.SetAsLastSibling();
        }

        private void RemoveBook(BookData bookData)
        {
            BookSelectElement bookSelectElement = _instantiatedElements.Find(x => x.ContainedBook == bookData);
            if (bookSelectElement != null && _bookSerializer.RemoveBook(bookData))
            {
                _instantiatedElements.Remove(bookSelectElement);
                Destroy(bookSelectElement.gameObject);
            }
        }
    }
}
