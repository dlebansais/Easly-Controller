namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using Contracts;

    /// <inheritdoc/>
    public class FocusAssignableCellViewReadOnlyDictionary<TKey> : FrameAssignableCellViewReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>, IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>, IDictionary<TKey, IFocusAssignableCellView>, IReadOnlyCollection<KeyValuePair<TKey, IFocusAssignableCellView>>, IReadOnlyDictionary<TKey, IFocusAssignableCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusAssignableCellViewReadOnlyDictionary(FocusAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region TKey, IFocusAssignableCellView
        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Add(KeyValuePair<TKey, IFocusAssignableCellView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Contains(KeyValuePair<TKey, IFocusAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.CopyTo(KeyValuePair<TKey, IFocusAssignableCellView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this) array[i++] = new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, (IFocusAssignableCellView)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Remove(KeyValuePair<TKey, IFocusAssignableCellView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.IsReadOnly { get { return true; } }
        IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IFocusAssignableCellView>((TKey)iterator.Current.Key, (IFocusAssignableCellView)iterator.Current.Value); } }

        IFocusAssignableCellView IDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return (IFocusAssignableCellView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, IFocusAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusAssignableCellView> IDictionary<TKey, IFocusAssignableCellView>.Values { get { List<IFocusAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IFocusAssignableCellView>.Add(TKey key, IFocusAssignableCellView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFocusAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IFocusAssignableCellView>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (IFocusAssignableCellView)Value; return Result; }

        IFocusAssignableCellView IReadOnlyDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return (IFocusAssignableCellView)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusAssignableCellView> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Values { get { List<IFocusAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IFocusAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (IFocusAssignableCellView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusAssignableCellViewReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                IFocusAssignableCellView ThisValue = (IFocusAssignableCellView)this[Key];
                IFocusAssignableCellView OtherValue = (IFocusAssignableCellView)AsOtherReadOnlyDictionary[Key];

                if (!comparer.IsTrue((ThisValue is null && OtherValue is null) || (ThisValue is not null && OtherValue is not null)))
                    return comparer.Failed();

                if (ThisValue is not null)
                {
                    if (!comparer.VerifyEqual(ThisValue, OtherValue))
                        return comparer.Failed();
                }
            }

            return true;
        }
        #endregion
    }
}
