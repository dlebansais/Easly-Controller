#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IWriteableInnerDictionary<TKey> : IReadOnlyInnerDictionary<TKey>, IDictionary<TKey, IWriteableInner>
    {
        new int Count { get; }
        new Dictionary<TKey, IWriteableInner>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class WriteableInnerDictionary<TKey> : Dictionary<TKey, IWriteableInner>, IWriteableInnerDictionary<TKey>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new WriteableInnerReadOnlyDictionary<TKey>(this);
        }

        #region ReadOnly
        IReadOnlyInner IDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IWriteableInner)value; } }
        void IDictionary<TKey, IReadOnlyInner>.Add(TKey key, IReadOnlyInner value) { Add(key, (IWriteableInner)value); }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IReadOnlyInner>.TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IWriteableInner Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IWriteableInner> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Add(KeyValuePair<TKey, IReadOnlyInner> item) { Add(item.Key, (IWriteableInner)item.Value); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IWriteableInner>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> IEnumerable<KeyValuePair<TKey, IReadOnlyInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner>>();
            IEnumerator<KeyValuePair<TKey, IWriteableInner>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IWriteableInner> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
