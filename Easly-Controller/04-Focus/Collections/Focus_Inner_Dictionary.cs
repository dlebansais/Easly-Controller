#pragma warning disable 1591

namespace EaslyController.Focus
{
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
        void IDictionary<TKey, IReadOnlyInner>.Add(TKey key, IReadOnlyInner value) { Add((TKey)key, (IFocusInner)value); }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IReadOnlyInner>.TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFocusInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Add(KeyValuePair<TKey, IReadOnlyInner> item) { Add(item.Key, (IFocusInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }

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
        #endregion

        #region Writeable
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

        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner)value; } }
        void IDictionary<TKey, IWriteableInner>.Add(TKey key, IWriteableInner value) { Add((TKey)key, (IFocusInner)value); }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { return new List<IWriteableInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IWriteableInner>>.CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFocusInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Add(KeyValuePair<TKey, IWriteableInner> item) { Add(item.Key, (IFocusInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Remove(KeyValuePair<TKey, IWriteableInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }

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
        #endregion

        #region Frame
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

        IFrameInner IDictionary<TKey, IFrameInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusInner)value; } }
        void IDictionary<TKey, IFrameInner>.Add(TKey key, IFrameInner value) { Add((TKey)key, (IFocusInner)value); }
        ICollection<TKey> IDictionary<TKey, IFrameInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IFrameInner>.TryGetValue(TKey key, out IFrameInner value)
        {
            bool Result = TryGetValue(key, out IFocusInner Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameInner> IDictionary<TKey, IFrameInner>.Values { get { return new List<IFrameInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IFrameInner>>.CopyTo(KeyValuePair<TKey, IFrameInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFocusInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IFrameInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IFrameInner>>.Add(KeyValuePair<TKey, IFrameInner> item) { Add(item.Key, (IFocusInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.Contains(KeyValuePair<TKey, IFrameInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.Remove(KeyValuePair<TKey, IFrameInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusInner>>)this).IsReadOnly; } }

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
        #endregion
    }
}
