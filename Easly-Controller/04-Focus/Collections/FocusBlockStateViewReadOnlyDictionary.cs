namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class FocusBlockStateViewReadOnlyDictionary : FrameBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IEnumerable<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IDictionary<IFocusBlockState, FocusBlockStateView>, IReadOnlyCollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusBlockStateViewReadOnlyDictionary(FocusBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IFocusBlockState, FocusBlockStateView
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Add(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Contains(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.CopyTo(KeyValuePair<IFocusBlockState, FocusBlockStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this) array[i++] = new KeyValuePair<IFocusBlockState, FocusBlockStateView>((IFocusBlockState)Entry.Key, (FocusBlockStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Remove(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<IFocusBlockState, FocusBlockStateView>> IEnumerable<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IFocusBlockState, FocusBlockStateView>((IFocusBlockState)iterator.Current.Key, (FocusBlockStateView)iterator.Current.Value); } }

        FocusBlockStateView IDictionary<IFocusBlockState, FocusBlockStateView>.this[IFocusBlockState key] { get { return (FocusBlockStateView)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<IFocusBlockState> IDictionary<IFocusBlockState, FocusBlockStateView>.Keys { get { List<IFocusBlockState> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FocusBlockStateView> IDictionary<IFocusBlockState, FocusBlockStateView>.Values { get { List<FocusBlockStateView> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusBlockState, FocusBlockStateView>.Add(IFocusBlockState key, FocusBlockStateView value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.ContainsKey(IFocusBlockState key) { return ContainsKey(key); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.Remove(IFocusBlockState key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.TryGetValue(IFocusBlockState key, out FocusBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FocusBlockStateView)Value; return Result; }

        FocusBlockStateView IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.this[IFocusBlockState key] { get { return (FocusBlockStateView)this[key]; } }
        IEnumerable<IFocusBlockState> IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.Keys { get { List<IFocusBlockState> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<FocusBlockStateView> IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.Values { get { List<FocusBlockStateView> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.ContainsKey(IFocusBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.TryGetValue(IFocusBlockState key, out FocusBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FocusBlockStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusBlockStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IFocusBlockState Key in Keys)
            {
                FocusBlockStateView Value = (FocusBlockStateView)this[Key];

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
