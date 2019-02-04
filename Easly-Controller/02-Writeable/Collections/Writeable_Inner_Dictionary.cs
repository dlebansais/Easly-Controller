#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System;
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
        #region ReadOnly
        IReadOnlyInner IDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } set { this[key] = (IWriteableInner)value; } }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }
        ICollection<IReadOnlyInner> IDictionary<TKey, IReadOnlyInner>.Values { get { return new List<IReadOnlyInner>(Values); } }
        public void Add(TKey key, IReadOnlyInner value) { base.Add(key, (IWriteableInner)value); }

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

        public bool TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IWriteableInner Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<TKey, IReadOnlyInner> item) { base.Add(item.Key, (IWriteableInner)item.Value); }
        public bool Contains(KeyValuePair<TKey, IReadOnlyInner> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IReadOnlyInner>[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, IReadOnlyInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IWriteableInner>>)this).IsReadOnly; } }
        #endregion
    }
}
