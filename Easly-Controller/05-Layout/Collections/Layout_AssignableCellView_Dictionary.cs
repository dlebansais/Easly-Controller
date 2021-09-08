namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutAssignableCellViewDictionary<TKey> : FocusAssignableCellViewDictionary<TKey>, ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>, IDictionary<TKey, ILayoutAssignableCellView>, IReadOnlyCollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IReadOnlyDictionary<TKey, ILayoutAssignableCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }

        #region TKey, ILayoutAssignableCellView
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Add(KeyValuePair<TKey, ILayoutAssignableCellView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Contains(KeyValuePair<TKey, ILayoutAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.CopyTo(KeyValuePair<TKey, ILayoutAssignableCellView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Remove(KeyValuePair<TKey, ILayoutAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>.GetEnumerator() { return ((IList<KeyValuePair<TKey, ILayoutAssignableCellView>>)this).GetEnumerator(); }

        ILayoutAssignableCellView IDictionary<TKey, ILayoutAssignableCellView>.this[TKey key] { get { return (ILayoutAssignableCellView)this[key]; } set { this[key] = value; } }
        ICollection<TKey> IDictionary<TKey, ILayoutAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutAssignableCellView> IDictionary<TKey, ILayoutAssignableCellView>.Values { get { List<ILayoutAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, ILayoutAssignableCellView>.Add(TKey key, ILayoutAssignableCellView value) { Add(key, value); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.Remove(TKey key) { return Remove(key); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }

        ILayoutAssignableCellView IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.this[TKey key] { get { return (ILayoutAssignableCellView)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutAssignableCellView> IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.Values { get { List<ILayoutAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, ILayoutAssignableCellView>.TryGetValue(TKey key, out ILayoutAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (ILayoutAssignableCellView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutAssignableCellViewDictionary<TKey> AsAssignableCellViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsAssignableCellViewDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                ILayoutAssignableCellView Value = (ILayoutAssignableCellView)this[Key];

                if (!comparer.IsTrue(AsAssignableCellViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsAssignableCellViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override FrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new LayoutAssignableCellViewReadOnlyDictionary<TKey>(this);
        }
    }
}
