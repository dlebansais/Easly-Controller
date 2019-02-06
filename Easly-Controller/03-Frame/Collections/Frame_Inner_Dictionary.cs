#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameInnerDictionary<TKey> : IWriteableInnerDictionary<TKey>, IDictionary<TKey, IFrameInner>
    {
        new int Count { get; }
        new Dictionary<TKey, IFrameInner>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameInnerDictionary<TKey> : Dictionary<TKey, IFrameInner>, IFrameInnerDictionary<TKey>
    {
        #region ReadOnly
        IReadOnlyInner IDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFrameInner)value; } }
        void IDictionary<TKey, IReadOnlyInner>.Add(TKey key, IReadOnlyInner value) { Add(key, (IFrameInner)value); }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IReadOnlyInner>.TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFrameInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Add(KeyValuePair<TKey, IReadOnlyInner> item) { Add(item.Key, (IFrameInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameInner>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> IEnumerable<KeyValuePair<TKey, IReadOnlyInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner>>();
            IEnumerator<KeyValuePair<TKey, IFrameInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFrameInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        Dictionary<TKey, IWriteableInner>.Enumerator IWriteableInnerDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IWriteableInner> NewDictionary = new Dictionary<TKey, IWriteableInner>();
            IEnumerator<KeyValuePair<TKey, IFrameInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFrameInner> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFrameInner)value; } }
        void IDictionary<TKey, IWriteableInner>.Add(TKey key, IWriteableInner value) { Add(key, (IFrameInner)value); }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { return new List<IWriteableInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IWriteableInner>>.CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFrameInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Add(KeyValuePair<TKey, IWriteableInner> item) { Add(item.Key, (IFrameInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Remove(KeyValuePair<TKey, IWriteableInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameInner>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner>> NewList = new List<KeyValuePair<TKey, IWriteableInner>>();
            IEnumerator<KeyValuePair<TKey, IFrameInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFrameInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
