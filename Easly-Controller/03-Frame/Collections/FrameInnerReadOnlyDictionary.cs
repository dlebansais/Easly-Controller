namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameInnerReadOnlyDictionary<TKey> : WriteableInnerReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, IFrameInner>>, IEnumerable<KeyValuePair<TKey, IFrameInner>>, IDictionary<TKey, IFrameInner>, IReadOnlyCollection<KeyValuePair<TKey, IFrameInner>>, IReadOnlyDictionary<TKey, IFrameInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameInnerReadOnlyDictionary(FrameInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out IFrameInner value) { bool Result = TryGetValue(key, out IWriteableInner Value); value = (IFrameInner)Value; return Result; }

        #region TKey, IFrameInner
        void ICollection<KeyValuePair<TKey, IFrameInner>>.Add(KeyValuePair<TKey, IFrameInner> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, IFrameInner>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.Contains(KeyValuePair<TKey, IFrameInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IFrameInner>>.CopyTo(KeyValuePair<TKey, IFrameInner>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this) array[i++] = new KeyValuePair<TKey, IFrameInner>(Entry.Key, (IFrameInner)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.Remove(KeyValuePair<TKey, IFrameInner> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFrameInner>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, IFrameInner>> IEnumerable<KeyValuePair<TKey, IFrameInner>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IFrameInner>((TKey)iterator.Current.Key, (IFrameInner)iterator.Current.Value); } }

        IFrameInner IDictionary<TKey, IFrameInner>.this[TKey key] { get { return (IFrameInner)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, IFrameInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFrameInner> Entry in (ICollection<KeyValuePair<TKey, IFrameInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFrameInner> IDictionary<TKey, IFrameInner>.Values { get { List<IFrameInner> Result = new(); foreach (KeyValuePair<TKey, IFrameInner> Entry in (ICollection<KeyValuePair<TKey, IFrameInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IFrameInner>.Add(TKey key, IFrameInner value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFrameInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IFrameInner>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFrameInner>.TryGetValue(TKey key, out IFrameInner value) { bool Result = TryGetValue(key, out IWriteableInner Value); value = (IFrameInner)Value; return Result; }

        IFrameInner IReadOnlyDictionary<TKey, IFrameInner>.this[TKey key] { get { return (IFrameInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFrameInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFrameInner> Entry in (ICollection<KeyValuePair<TKey, IFrameInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFrameInner> IReadOnlyDictionary<TKey, IFrameInner>.Values { get { List<IFrameInner> Result = new(); foreach (KeyValuePair<TKey, IFrameInner> Entry in (ICollection<KeyValuePair<TKey, IFrameInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IFrameInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IFrameInner>.TryGetValue(TKey key, out IFrameInner value) { bool Result = TryGetValue(key, out IWriteableInner Value); value = (IFrameInner)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameInnerReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IFrameInner Value = (IFrameInner)this[Key];

                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
