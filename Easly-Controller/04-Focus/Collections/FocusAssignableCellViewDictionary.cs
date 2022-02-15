namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using Contracts;

    /// <inheritdoc/>
    public class FocusAssignableCellViewDictionary<TKey> : FrameAssignableCellViewDictionary<TKey>, ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>, IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>, IDictionary<TKey, IFocusAssignableCellView>, IReadOnlyCollection<KeyValuePair<TKey, IFocusAssignableCellView>>, IReadOnlyDictionary<TKey, IFocusAssignableCellView>, IEqualComparable
        where TKey : notnull
    {
        /// <inheritdoc/>
        public FocusAssignableCellViewDictionary() : base() { }
        /// <inheritdoc/>
        public FocusAssignableCellViewDictionary(IDictionary<TKey, IFocusAssignableCellView> dictionary) : base() { foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusAssignableCellViewDictionary(int capacity) : base(capacity) { }

        #region TKey, IFocusAssignableCellView
        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Add(KeyValuePair<TKey, IFocusAssignableCellView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Contains(KeyValuePair<TKey, IFocusAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.CopyTo(KeyValuePair<TKey, IFocusAssignableCellView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this) array[i++] = new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, (IFocusAssignableCellView)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Remove(KeyValuePair<TKey, IFocusAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IFocusAssignableCellView>((TKey)iterator.Current.Key, (IFocusAssignableCellView)iterator.Current.Value); } }

        IFocusAssignableCellView IDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return (IFocusAssignableCellView)this[key]; } set { this[key] = value; } }
        ICollection<TKey> IDictionary<TKey, IFocusAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusAssignableCellView> IDictionary<TKey, IFocusAssignableCellView>.Values { get { List<IFocusAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IFocusAssignableCellView>.Add(TKey key, IFocusAssignableCellView value) { Add(key, value); }
        bool IDictionary<TKey, IFocusAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IFocusAssignableCellView>.Remove(TKey key) { return Remove(key); }
        bool IDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (IFocusAssignableCellView)Value; return Result; }

        IFocusAssignableCellView IReadOnlyDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return (IFocusAssignableCellView)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusAssignableCellView> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Values { get { List<IFocusAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IFocusAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value) { bool Result = TryGetValue(key, out IFrameAssignableCellView Value); value = (IFocusAssignableCellView)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override FrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new FocusAssignableCellViewReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusAssignableCellViewDictionary<TKey> AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IFocusAssignableCellView Value = (IFocusAssignableCellView)this[Key];

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
