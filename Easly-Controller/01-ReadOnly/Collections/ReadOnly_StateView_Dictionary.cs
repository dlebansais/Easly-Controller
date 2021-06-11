namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public interface IReadOnlyStateViewDictionary : IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IEqualComparable
    {
    }

    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    internal class ReadOnlyStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IReadOnlyStateViewDictionary
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsStateViewDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsStateViewDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
