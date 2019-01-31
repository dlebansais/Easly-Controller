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
    public interface IFocusInnerDictionary<TKey> : IFrameInnerDictionary<TKey>, IDictionary<TKey, IFocusInner<IFocusBrowsingChildIndex>>
    {
        new int Count { get; }
        new Dictionary<TKey, IFocusInner<IFocusBrowsingChildIndex>>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FocusInnerDictionary<TKey> : Dictionary<TKey, IFocusInner<IFocusBrowsingChildIndex>>, IFocusInnerDictionary<TKey>
    {
        #region ReadOnly
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner<IFocusBrowsingChildIndex>)value; } }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Keys { get { return Keys; } }
        ICollection<IReadOnlyInner<IReadOnlyBrowsingChildIndex>> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Values { get { return new List<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Values); } }
        public void Add(TKey key, IReadOnlyInner<IReadOnlyBrowsingChildIndex> value) { base.Add(key, (IFocusInner<IFocusBrowsingChildIndex>)value); }

        public new IEnumerator<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IReadOnlyInner<IReadOnlyBrowsingChildIndex> value)
        {
            bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { base.Add(item.Key, (IFocusInner<IFocusBrowsingChildIndex>)item.Value); }
        public bool Contains(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        IWriteableInner<IWriteableBrowsingChildIndex> IDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner<IFocusBrowsingChildIndex>)value; } }
        ICollection<TKey> IDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.Keys { get { return Keys; } }
        ICollection<IWriteableInner<IWriteableBrowsingChildIndex>> IDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.Values { get { return new List<IWriteableInner<IWriteableBrowsingChildIndex>>(Values); } }
        public void Add(TKey key, IWriteableInner<IWriteableBrowsingChildIndex> value) { base.Add(key, (IFocusInner<IFocusBrowsingChildIndex>)value); }

        IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> IEnumerable<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.Enumerator IWriteableInnerDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> NewDictionary = new Dictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IWriteableInner<IWriteableBrowsingChildIndex> value)
        {
            bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> item) { base.Add(item.Key, (IFocusInner<IFocusBrowsingChildIndex>)item.Value); }
        public bool Contains(KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion

        #region Frame
        IFrameInner<IFrameBrowsingChildIndex> IDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner<IFocusBrowsingChildIndex>)value; } }
        ICollection<TKey> IDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.Keys { get { return Keys; } }
        ICollection<IFrameInner<IFrameBrowsingChildIndex>> IDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.Values { get { return new List<IFrameInner<IFrameBrowsingChildIndex>>(Values); } }
        public void Add(TKey key, IFrameInner<IFrameBrowsingChildIndex> value) { base.Add(key, (IFocusInner<IFocusBrowsingChildIndex>)value); }

        IEnumerator<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> IEnumerable<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.Enumerator IFrameInnerDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>> NewDictionary = new Dictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>();
            IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IFrameInner<IFrameBrowsingChildIndex> value)
        {
            bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>> item) { base.Add(item.Key, (IFocusInner<IFocusBrowsingChildIndex>)item.Value); }
        public bool Contains(KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }
        #endregion
    }
}
