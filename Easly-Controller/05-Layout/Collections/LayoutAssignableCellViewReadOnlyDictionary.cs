namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutAssignableCellViewReadOnlyDictionary<TKey> : FocusAssignableCellViewReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>, IDictionary<TKey, ILayoutAssignableCellView>, IReadOnlyCollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IReadOnlyDictionary<TKey, ILayoutAssignableCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutAssignableCellViewReadOnlyDictionary(LayoutAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFocusAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }

        #region TKey, ILayoutAssignableCellView
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Add(KeyValuePair<TKey, ILayoutAssignableCellView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Contains(KeyValuePair<TKey, ILayoutAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.CopyTo(KeyValuePair<TKey, ILayoutAssignableCellView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Remove(KeyValuePair<TKey, ILayoutAssignableCellView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, ILayoutAssignableCellView>((TKey)iterator.Current.Key, (ILayoutAssignableCellView)iterator.Current.Value); } }

        ILayoutAssignableCellView IDictionary<TKey, ILayoutAssignableCellView>.this[TKey key] { get { return (ILayoutAssignableCellView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, ILayoutAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutAssignableCellView> IDictionary<TKey, ILayoutAssignableCellView>.Values { get { List<ILayoutAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, ILayoutAssignableCellView>.Add(TKey key, ILayoutAssignableCellView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFocusAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }

        ILayoutAssignableCellView IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.this[TKey key] { get { return (ILayoutAssignableCellView)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutAssignableCellView> IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.Values { get { List<ILayoutAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFocusAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutAssignableCellViewReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                ILayoutAssignableCellView Value = (ILayoutAssignableCellView)this[Key];

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
