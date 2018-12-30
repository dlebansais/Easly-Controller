using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public interface IReadOnlyStateViewDictionary : IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IEqualComparable
    {
    }

    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public class ReadOnlyStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IReadOnlyStateViewDictionary
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyStateViewDictionary AsStateViewDictionary))
                return false;

            if (Count != AsStateViewDictionary.Count)
                return false;

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this)
            {
                if (!AsStateViewDictionary.ContainsKey(Entry.Key))
                    return false;

                if (!comparer.VerifyEqual(Entry.Value, AsStateViewDictionary[Entry.Key]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
