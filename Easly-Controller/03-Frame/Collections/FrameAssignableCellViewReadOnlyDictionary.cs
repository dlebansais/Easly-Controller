namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameAssignableCellViewReadOnlyDictionary<TKey> : System.Collections.ObjectModel.ReadOnlyDictionary<TKey, IFrameAssignableCellView>, IEqualComparable
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
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameAssignableCellViewReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                IFrameAssignableCellView ThisValue = Entry.Value;
                IFrameAssignableCellView OtherValue = AsOtherReadOnlyDictionary[Entry.Key];

                if (!comparer.IsTrue((ThisValue == null && OtherValue == null) || (ThisValue != null && OtherValue != null)))
                    return comparer.Failed();

                if (ThisValue != null)
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
