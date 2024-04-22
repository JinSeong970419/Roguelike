using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, ISerializable, IDeserializationCallback, ISerializationCallbackReceiver
    {
        [ArrayElementTitle("Key")]
        [SerializeField] private List<SerializalePair<TKey, TValue>> pairs = new List<SerializalePair<TKey, TValue>>();
        [HideInInspector][SerializeField] private List<TKey> keys = new List<TKey>();
        [HideInInspector][SerializeField] private List<TValue> values = new List<TValue>();

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;

        public int Count => pairs.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public bool IsFixedSize => false;

        ICollection IDictionary.Keys => dictionary.Keys;

        ICollection IDictionary.Values => dictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => dictionary.Values;

        public object this[object key] { get => dictionary[(TKey)key]; set => dictionary[(TKey)key] = (TValue)value; }
        public TValue this[TKey key] { get => dictionary[key]; set => dictionary[key] = value; }


        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            //pairs.Clear();
            //foreach (KeyValuePair<TKey, TValue> pair in this)
            //{
            //    pairs.Add(new SerializalePair<TKey, TValue>() { Key = pair.Key, Value = pair.Value });
            //}
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            //keys.Clear();
            //values.Clear();

            //for (int i = 0; i < pairs.Count; i++)
            //{
            //    keys.Add(pairs[i].Key);
            //    values.Add(pairs[i].Value);
            //}

            dictionary.Clear();
            for (int i = 0; i < pairs.Count; i++)
            {
                dictionary.Add(pairs[i].Key, pairs[i].Value);
            }

        }

        public void Add(TKey key, TValue value)
        {
            if(dictionary.TryAdd(key, value))
            {
                pairs.Add(new SerializalePair<TKey, TValue> { Key = key, Value = value });
            }
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
            pairs.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {

        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnDeserialization(object sender)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            TKey k = (TKey)key;
            TValue v = (TValue)value;
            if(k == null)
            {
                Debug.LogWarning("Invalid cast");
                return;
            }
            if(v == null)
            {
                Debug.LogWarning("Invalid cast");
                return;
            }
            Add((TKey)key, (TValue)value);
        }

        public bool Contains(object key)
        {
            return dictionary.TryGetValue((TKey)key, out var value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public void Remove(object key)
        {
            dictionary.Remove((TKey)key);
        }
    }

    [System.Serializable]
    public class SerializalePair<TKey, TValue>
    {
        [SerializeField] public TKey Key;
        [SerializeField] public TValue Value;
    }
}
