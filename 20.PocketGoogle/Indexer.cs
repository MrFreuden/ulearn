using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private Dictionary<string, WordData> _wordDictionary = new();
        private char[] _delimeters = { ' ', '.', ',', '!', '?', ':', '-', '–', '\r', '\n' };
        public void Add(int id, string documentText)
        {
            var count = 0;
            var text = documentText.Split(_delimeters);

            foreach (var word in text)
            {
                if (string.IsNullOrEmpty(word))
                {
                    count++;
                    continue;
                }
                if (_wordDictionary.TryGetValue(word, out WordData val))
                {
                    val.AddPositionAndId(id, count);
                }
                else
                {
                    _wordDictionary.Add(word, new WordData(id, count));
                }
                count += word.Length + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            if (_wordDictionary.TryGetValue(word, out var wordData))
            {
                return wordData.GetIds();
            }

            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (_wordDictionary.TryGetValue(word, out var wordData))
            {
                return wordData.GetPositions(id);
            }

            return new List<int>();
        }

        public void Remove(int id)
        {
            var keysToRemove = new List<string>();

            foreach (var item in _wordDictionary)
            {
                item.Value.RemoveId(id);
                if (item.Value.GetIds().Count == 0)
                {
                    keysToRemove.Add(item.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _wordDictionary.Remove(key);
            }
        }
    }

    public class WordData
    {
        private Dictionary<int, List<int>> _idAndPositions = new();
        public WordData(int id, int position)
        {
            AddPositionAndId(id, position);
        }

        public void AddPositionAndId(int id, int position)
        {
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
            return _idAndPositions.Keys.ToList();
        }

        public List<int> GetPositions(int id)
        {
            if (HasId(id))
            {
                return _idAndPositions[id].ToList();
            }
            return new List<int>();
        }

        public bool HasId(int id)
        {
            return _idAndPositions.ContainsKey(id);
        }

        public void RemoveId(int id)
        {
            if (HasId(id))
            {
                _idAndPositions.Remove(id);
            }
        }
    }
}