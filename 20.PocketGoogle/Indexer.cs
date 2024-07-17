using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private Dictionary<string, WordData> wordDictionary = new();
        private char[] _delimeters = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        public void Add(int id, string documentText)
        {
            var count = 0;
            var text = documentText.Split(_delimeters);
            foreach (var word in text)
            {
                if (string.IsNullOrEmpty(word))
                {
                }
                else if (wordDictionary.TryGetValue(word, out WordData val))
                {
                    val.AddPositionAndId(id, count);
                }
                else
                {
                    wordDictionary.Add(word, new WordData(id, count));
                }
                count += word.Length;
                count++;
            }
        }

        public List<int> GetIds(string word)
        {
            if (wordDictionary.TryGetValue(word, out var wordData))
            {
                return wordData.GetIds();
            }
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (wordDictionary.TryGetValue(word, out var wordData))
            {
                return wordData.GetPositions(id);
            }
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var item in wordDictionary)
            {
                if (item.Value.HatId(id))
                {
                    item.Value.Delete(id);

                }
                if (item.Value.GetIds().Count == 0)
                {
                    wordDictionary.Remove(item.Key);
                }
            }
        }
    }

    public class WordData
    {
        private HashSet<int> _id = new();
        //private List<int> _positions = new();
        private Dictionary<int, List<int>> _idAndPositions = new();
        public WordData(int id, int position)
        {
            _id.Add(id);
           // _positions.Add(position);
            _idAndPositions.Add(id, new List<int>() { position });
        }

        public void AddPositionAndId(int id, int position)
        {
            _id.Add(id);
            //_positions.Add(position);
            if (_idAndPositions.ContainsKey(id))
            {
                _idAndPositions[id].Add(position);
            }
            else
            {
                _idAndPositions.Add(id, new List<int>() { position });
            }
        }

        public List<int> GetIds()
        {
            return _id.ToList();
        }

        public List<int> GetPositions(int id)
        {
            return _idAndPositions[id].ToList();
        }

        public bool HatId(int id)
        {
            return _id.Contains(id);
        }

        public void Delete(int id)
        {
            _id.Remove(id);
            _idAndPositions.Remove(id);
        }
    }
}