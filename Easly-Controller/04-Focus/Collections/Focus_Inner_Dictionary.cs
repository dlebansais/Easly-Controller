#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFocusInnerDictionary<TKey> : IFrameInnerDictionary<TKey>, IDictionary<TKey, IFocusInner>
    {
        new int Count { get; }
        new Dictionary<TKey, IFocusInner>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FocusInnerDictionary<TKey> : Dictionary<TKey, IFocusInner>, IFocusInnerDictionary<TKey>
    {
        #region ReadOnly
        IReadOnlyInner IDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner)value; } }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }
        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }
        public void Add(TKey key, IReadOnlyInner value) { base.Add(key, (IFocusInner)value); }

        IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> IEnumerable<KeyValuePair<TKey, IReadOnlyInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IReadOnlyInner> item) { base.Add(item.Key, (IFocusInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner)value; } }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { return Keys; } }
        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { return new List<IWriteableInner>(Values); } }
        public void Add(TKey key, IWriteableInner value) { base.Add(key, (IFocusInner)value); }

        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner>> NewList = new List<KeyValuePair<TKey, IWriteableInner>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<TKey, IWriteableInner>.Enumerator IWriteableInnerDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IWriteableInner> NewDictionary = new Dictionary<TKey, IWriteableInner>();
            IEnumerator<KeyValuePair<TKey, IFocusInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IWriteableInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IWriteableInner> item) { base.Add(item.Key, (IFocusInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IWriteableInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion

        #region Frame
        IFrameInner IDictionary<TKey, IFrameInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner)value; } }
        ICollection<TKey> IDictionary<TKey, IFrameInner>.Keys { get { return Keys; } }
        ICollection<IFrameInner> IDictionary<TKey, IFrameInner>.Values { get { return new List<IFrameInner>(Values); } }
        public void Add(TKey key, IFrameInner value) { base.Add(key, (IFocusInner)value); }

        IEnumerator<KeyValuePair<TKey, IFrameInner>> IEnumerable<KeyValuePair<TKey, IFrameInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameInner>> NewList = new List<KeyValuePair<TKey, IFrameInner>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<TKey, IFrameInner>.Enumerator IFrameInnerDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IFrameInner> NewDictionary = new Dictionary<TKey, IFrameInner>();
            IEnumerator<KeyValuePair<TKey, IFocusInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IFrameInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IFrameInner> item) { base.Add(item.Key, (IFocusInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IFrameInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IFrameInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IFrameInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion
    }
}
