﻿namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using Contracts;

    /// <inheritdoc/>
    public class LayoutAssignableCellViewReadOnlyDictionary<TKey> : FocusAssignableCellViewReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>, IDictionary<TKey, ILayoutAssignableCellView>, IReadOnlyCollection<KeyValuePair<TKey, ILayoutAssignableCellView>>, IReadOnlyDictionary<TKey, ILayoutAssignableCellView>, IEqualComparable
        where TKey : notnull
    {
        /// <inheritdoc/>
        public LayoutAssignableCellViewReadOnlyDictionary(LayoutAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region TKey, ILayoutAssignableCellView
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Add(KeyValuePair<TKey, ILayoutAssignableCellView> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Contains(KeyValuePair<TKey, ILayoutAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.CopyTo(KeyValuePair<TKey, ILayoutAssignableCellView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this) array[i++] = new KeyValuePair<TKey, ILayoutAssignableCellView>(Entry.Key, (ILayoutAssignableCellView)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.Remove(KeyValuePair<TKey, ILayoutAssignableCellView> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> IEnumerable<KeyValuePair<TKey, ILayoutAssignableCellView>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, ILayoutAssignableCellView>((TKey)iterator.Current.Key, (ILayoutAssignableCellView)iterator.Current.Value); } }

        ILayoutAssignableCellView IDictionary<TKey, ILayoutAssignableCellView>.this[TKey key] { get { return (ILayoutAssignableCellView)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<TKey> IDictionary<TKey, ILayoutAssignableCellView>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutAssignableCellView> IDictionary<TKey, ILayoutAssignableCellView>.Values { get { List<ILayoutAssignableCellView> Result = new(); foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in (ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, ILayoutAssignableCellView>.Add(TKey key, ILayoutAssignableCellView value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, ILayoutAssignableCellView>.Remove(TKey key) { throw new NotSupportedException("Collection is read-only."); }
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutAssignableCellViewReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                ILayoutAssignableCellView ThisValue = (ILayoutAssignableCellView)this[Key];
                ILayoutAssignableCellView OtherValue = (ILayoutAssignableCellView)AsOtherReadOnlyDictionary[Key];

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
