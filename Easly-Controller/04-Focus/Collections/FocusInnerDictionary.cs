namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusInnerDictionary<TKey> : FrameInnerDictionary<TKey>, ICollection<KeyValuePair<TKey, IFocusInner>>, IEnumerable<KeyValuePair<TKey, IFocusInner>>, IDictionary<TKey, IFocusInner>, IReadOnlyCollection<KeyValuePair<TKey, IFocusInner>>, IReadOnlyDictionary<TKey, IFocusInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusInnerDictionary() : base() { }
        /// <inheritdoc/>
        public FocusInnerDictionary(IDictionary<TKey, IFocusInner> dictionary) : base() { foreach (KeyValuePair<TKey, IFocusInner> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusInnerDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }

        #region TKey, IFocusInner
        void ICollection<KeyValuePair<TKey, IFocusInner>>.Add(KeyValuePair<TKey, IFocusInner> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.Contains(KeyValuePair<TKey, IFocusInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IFocusInner>>.CopyTo(KeyValuePair<TKey, IFocusInner>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this) array[i++] = new KeyValuePair<TKey, IFocusInner>(Entry.Key, (IFocusInner)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.Remove(KeyValuePair<TKey, IFocusInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameInner>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<TKey, IFocusInner>> IEnumerable<KeyValuePair<TKey, IFocusInner>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IFocusInner>((TKey)iterator.Current.Key, (IFocusInner)iterator.Current.Value); } }

        IFocusInner IDictionary<TKey, IFocusInner>.this[TKey key] { get { return (IFocusInner)this[key]; } set { this[key] = value; } }
        ICollection<TKey> IDictionary<TKey, IFocusInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusInner> IDictionary<TKey, IFocusInner>.Values { get { List<IFocusInner> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IFocusInner>.Add(TKey key, IFocusInner value) { Add(key, value); }
        bool IDictionary<TKey, IFocusInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IFocusInner>.Remove(TKey key) { return Remove(key); }
        bool IDictionary<TKey, IFocusInner>.TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }

        IFocusInner IReadOnlyDictionary<TKey, IFocusInner>.this[TKey key] { get { return (IFocusInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFocusInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusInner> IReadOnlyDictionary<TKey, IFocusInner>.Values { get { List<IFocusInner> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IFocusInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IFocusInner>.TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new FocusInnerReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusInnerDictionary<TKey> AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IFocusInner Value = (IFocusInner)this[Key];

                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
