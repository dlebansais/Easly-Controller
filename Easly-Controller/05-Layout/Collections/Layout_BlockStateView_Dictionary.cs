﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBlockStateViewDictionary : FocusBlockStateViewDictionary, ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IEnumerable<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IDictionary<ILayoutBlockState, LayoutBlockStateView>, IReadOnlyCollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>, IEqualComparable
    {
        #region ILayoutBlockState, LayoutBlockStateView
        void ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Add(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Contains(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.CopyTo(KeyValuePair<ILayoutBlockState, LayoutBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Remove(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>> IEnumerable<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this).GetEnumerator(); }

        LayoutBlockStateView IDictionary<ILayoutBlockState, LayoutBlockStateView>.this[ILayoutBlockState key] { get { return (LayoutBlockStateView)this[key]; } set { this[key] = value; } }
        ICollection<ILayoutBlockState> IDictionary<ILayoutBlockState, LayoutBlockStateView>.Keys { get { List<ILayoutBlockState> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<LayoutBlockStateView> IDictionary<ILayoutBlockState, LayoutBlockStateView>.Values { get { List<LayoutBlockStateView> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutBlockState, LayoutBlockStateView>.Add(ILayoutBlockState key, LayoutBlockStateView value) { Add(key, value); }
        bool IDictionary<ILayoutBlockState, LayoutBlockStateView>.ContainsKey(ILayoutBlockState key) { return ContainsKey(key); }
        bool IDictionary<ILayoutBlockState, LayoutBlockStateView>.Remove(ILayoutBlockState key) { return Remove(key); }
        bool IDictionary<ILayoutBlockState, LayoutBlockStateView>.TryGetValue(ILayoutBlockState key, out LayoutBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (LayoutBlockStateView)Value; return Result; }

        LayoutBlockStateView IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>.this[ILayoutBlockState key] { get { return (LayoutBlockStateView)this[key]; } }
        IEnumerable<ILayoutBlockState> IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>.Keys { get { List<ILayoutBlockState> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<LayoutBlockStateView> IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>.Values { get { List<LayoutBlockStateView> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>.ContainsKey(ILayoutBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>.TryGetValue(ILayoutBlockState key, out LayoutBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (LayoutBlockStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (ILayoutBlockState Key in Keys)
            {
                LayoutBlockStateView Value = (LayoutBlockStateView)this[Key];

                if (!comparer.IsTrue(AsBlockStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new LayoutBlockStateViewReadOnlyDictionary(this);
        }
    }
}
