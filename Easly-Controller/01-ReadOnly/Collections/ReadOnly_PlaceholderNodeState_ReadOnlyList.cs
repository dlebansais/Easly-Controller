using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IReadOnlyPlaceholderNodeStateReadOnlyList : IReadOnlyList<IReadOnlyPlaceholderNodeState>, IEqualComparable
    {
        bool Contains(IReadOnlyPlaceholderNodeState value);
        int IndexOf(IReadOnlyPlaceholderNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public class ReadOnlyPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateReadOnlyList
    {
        public ReadOnlyPlaceholderNodeStateReadOnlyList(IReadOnlyPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyPlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
                return false;

            if (Count != AsPlaceholderNodeStateReadOnlyList.Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateReadOnlyList[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
