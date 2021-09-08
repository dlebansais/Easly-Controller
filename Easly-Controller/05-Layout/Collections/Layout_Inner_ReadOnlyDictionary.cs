namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class LayoutInnerReadOnlyDictionary<TKey> : FocusInnerReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, ILayoutInner>>, IEnumerable<KeyValuePair<TKey, ILayoutInner>>, IDictionary<TKey, ILayoutInner>, IReadOnlyCollection<KeyValuePair<TKey, ILayoutInner>>, IReadOnlyDictionary<TKey, ILayoutInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutInnerReadOnlyDictionary(LayoutInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out ILayoutInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (ILayoutInner)Value; return Result; }

        #region TKey, ILayoutInner
        void ICollection<KeyValuePair<TKey, ILayoutInner>>.Add(KeyValuePair<TKey, ILayoutInner> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, ILayoutInner>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, ILayoutInner>>.Contains(KeyValuePair<TKey, ILayoutInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, ILayoutInner>>.CopyTo(KeyValuePair<TKey, ILayoutInner>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<TKey, ILayoutInner>>.Remove(KeyValuePair<TKey, ILayoutInner> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, ILayoutInner>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, ILayoutInner>> IEnumerable<KeyValuePair<TKey, ILayoutInner>>.GetEnumerator() { return ((IList<KeyValuePair<TKey, ILayoutInner>>)this).GetEnumerator(); }

        ILayoutInner IDictionary<TKey, ILayoutInner>.this[TKey key] { get { return (ILayoutInner)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, ILayoutInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutInner> Entry in (ICollection<KeyValuePair<TKey, ILayoutInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutInner> IDictionary<TKey, ILayoutInner>.Values { get { List<ILayoutInner> Result = new(); foreach (KeyValuePair<TKey, ILayoutInner> Entry in (ICollection<KeyValuePair<TKey, ILayoutInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, ILayoutInner>.Add(TKey key, ILayoutInner value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, ILayoutInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, ILayoutInner>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, ILayoutInner>.TryGetValue(TKey key, out ILayoutInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (ILayoutInner)Value; return Result; }

        ILayoutInner IReadOnlyDictionary<TKey, ILayoutInner>.this[TKey key] { get { return (ILayoutInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, ILayoutInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutInner> Entry in (ICollection<KeyValuePair<TKey, ILayoutInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutInner> IReadOnlyDictionary<TKey, ILayoutInner>.Values { get { List<ILayoutInner> Result = new(); foreach (KeyValuePair<TKey, ILayoutInner> Entry in (ICollection<KeyValuePair<TKey, ILayoutInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, ILayoutInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, ILayoutInner>.TryGetValue(TKey key, out ILayoutInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (ILayoutInner)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                ILayoutInner Value = (ILayoutInner)this[Key];

                if (!comparer.IsTrue(AsInnerReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsInnerReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
