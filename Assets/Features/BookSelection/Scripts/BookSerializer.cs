using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Features.BookSelection.Scripts
{
    public class BookSerializer
    {
        public HashSet<BookData> BookDataList { get; private set; } = new ();

        public bool AddBook(BookData bookData)
        {
            return BookDataList.Add(bookData);
        }

        public bool RemoveBook(BookData bookData)
        {
            return BookDataList.Remove(bookData);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(BookDataList);
            PlayerPrefs.SetString(GetType().ToString(), json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString(GetType().ToString());
            if (string.IsNullOrEmpty(json)) return;
            
            HashSet<BookData> result = JsonConvert.DeserializeObject<HashSet<BookData>>(json);
            if (result != null)
            {
                BookDataList = result;
            }
        }
    }
}
