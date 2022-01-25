namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Contracts;

    /// <inheritdoc/>
    public class FrameAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameAssignableCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameAssignableCellViewReadOnlyDictionary(FrameAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameAssignableCellViewReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                IFrameAssignableCellView ThisValue = Entry.Value;
                IFrameAssignableCellView OtherValue = AsOtherReadOnlyDictionary[Entry.Key];

                if (!comparer.IsTrue((ThisValue is null && OtherValue is null) || (ThisValue is not null && OtherValue is not null)))
                    return comparer.Failed();

                if (ThisValue is not null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, AsOtherReadOnlyDictionary[Entry.Key]))
                        return comparer.Failed();
                }
            }

            return true;
        }
        #endregion
    }
}
