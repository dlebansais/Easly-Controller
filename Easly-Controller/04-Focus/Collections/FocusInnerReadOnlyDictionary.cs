namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusInnerReadOnlyDictionary<TKey> : FrameInnerReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, IFocusInner>>, IEnumerable<KeyValuePair<TKey, IFocusInner>>, IDictionary<TKey, IFocusInner>, IReadOnlyCollection<KeyValuePair<TKey, IFocusInner>>, IReadOnlyDictionary<TKey, IFocusInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusInnerReadOnlyDictionary(FocusInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }

        #region TKey, IFocusInner
        void ICollection<KeyValuePair<TKey, IFocusInner>>.Add(KeyValuePair<TKey, IFocusInner> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, IFocusInner>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.Contains(KeyValuePair<TKey, IFocusInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IFocusInner>>.CopyTo(KeyValuePair<TKey, IFocusInner>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.Remove(KeyValuePair<TKey, IFocusInner> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFocusInner>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, IFocusInner>> IEnumerable<KeyValuePair<TKey, IFocusInner>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IFocusInner>((TKey)iterator.Current.Key, (IFocusInner)iterator.Current.Value); } }

        IFocusInner IDictionary<TKey, IFocusInner>.this[TKey key] { get { return (IFocusInner)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, IFocusInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusInner> IDictionary<TKey, IFocusInner>.Values { get { List<IFocusInner> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IFocusInner>.Add(TKey key, IFocusInner value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFocusInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IFocusInner>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFocusInner>.TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }

        IFocusInner IReadOnlyDictionary<TKey, IFocusInner>.this[TKey key] { get { return (IFocusInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFocusInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusInner> IReadOnlyDictionary<TKey, IFocusInner>.Values { get { List<IFocusInner> Result = new(); foreach (KeyValuePair<TKey, IFocusInner> Entry in (ICollection<KeyValuePair<TKey, IFocusInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IFocusInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IFocusInner>.TryGetValue(TKey key, out IFocusInner value) { bool Result = TryGetValue(key, out IFrameInner Value); value = (IFocusInner)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusInnerReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IFocusInner Value = (IFocusInner)this[Key];

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
