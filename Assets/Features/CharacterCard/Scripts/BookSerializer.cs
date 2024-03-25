using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Features.CharacterCard.Scripts
{
    public class BookSerializer
    {
        public HashSet<BookData> BookDataList { get; private set; } = new ();

        public void AddBook(BookData bookData)
        {
            BookDataList.Add(bookData);
        }

        public void RemoveBook(BookData bookData)
        {
            BookDataList.Remove(bookData);
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
            HashSet<BookData> result = JsonConvert.DeserializeObject<HashSet<BookData>>(json);
            if (result != null)
            {
                BookDataList = result;
            }
        }
    }
}
