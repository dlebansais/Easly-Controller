﻿namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class LayoutBlockStateViewReadOnlyDictionary : FocusBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IEnumerable<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IDictionary<ILayoutBlockState, LayoutBlockStateView>, IReadOnlyCollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>, IReadOnlyDictionary<ILayoutBlockState, LayoutBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutBlockStateViewReadOnlyDictionary(LayoutBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region ILayoutBlockState, LayoutBlockStateView
        void ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Add(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Contains(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.CopyTo(KeyValuePair<ILayoutBlockState, LayoutBlockStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this) array[i++] = new KeyValuePair<ILayoutBlockState, LayoutBlockStateView>((ILayoutBlockState)Entry.Key, (LayoutBlockStateView)Entry.Value); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.Remove(KeyValuePair<ILayoutBlockState, LayoutBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>> IEnumerable<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<ILayoutBlockState, LayoutBlockStateView>((ILayoutBlockState)iterator.Current.Key, (LayoutBlockStateView)iterator.Current.Value); } }

        LayoutBlockStateView IDictionary<ILayoutBlockState, LayoutBlockStateView>.this[ILayoutBlockState key] { get { return (LayoutBlockStateView)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<ILayoutBlockState> IDictionary<ILayoutBlockState, LayoutBlockStateView>.Keys { get { List<ILayoutBlockState> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<LayoutBlockStateView> IDictionary<ILayoutBlockState, LayoutBlockStateView>.Values { get { List<LayoutBlockStateView> Result = new(); foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutBlockState, LayoutBlockStateView>.Add(ILayoutBlockState key, LayoutBlockStateView value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<ILayoutBlockState, LayoutBlockStateView>.ContainsKey(ILayoutBlockState key) { return ContainsKey(key); }
        bool IDictionary<ILayoutBlockState, LayoutBlockStateView>.Remove(ILayoutBlockState key) { throw new NotSupportedException("Collection is read-only."); }
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutBlockStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (ILayoutBlockState Key in Keys)
            {
                LayoutBlockStateView Value = (LayoutBlockStateView)this[Key];

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
