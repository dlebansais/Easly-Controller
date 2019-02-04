#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System;
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
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }
        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }
        public void Add(TKey key, IReadOnlyInner value) { base.Add(key, (IFrameInner)value); }

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

        public bool TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IReadOnlyInner> item) { base.Add(item.Key, (IFrameInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameInner>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFrameInner)value; } }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { return Keys; } }
        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { return new List<IWriteableInner>(Values); } }
        public void Add(TKey key, IWriteableInner value) { base.Add(key, (IFrameInner)value); }

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

        public bool TryGetValue(TKey key, out IWriteableInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IWriteableInner> item) { base.Add(item.Key, (IFrameInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IWriteableInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameInner>>)this).IsReadOnly; } }
        #endregion
    }
}
