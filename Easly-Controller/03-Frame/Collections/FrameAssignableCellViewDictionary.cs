﻿namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using Contracts;

    /// <inheritdoc/>
    public class FrameAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFrameAssignableCellView>, IEqualComparable
        where TKey : notnull
    {
        /// <inheritdoc/>
        public FrameAssignableCellViewDictionary() : base() { }
        /// <inheritdoc/>
        public FrameAssignableCellViewDictionary(IDictionary<TKey, IFrameAssignableCellView> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public FrameAssignableCellViewDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual FrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new FrameAssignableCellViewReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameAssignableCellViewDictionary<TKey> AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsOtherDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
