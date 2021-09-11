namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateViewReadOnlyDictionary : System.Collections.ObjectModel.ReadOnlyDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewReadOnlyDictionary(ReadOnlyNodeStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyNodeStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsOtherReadOnlyDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
