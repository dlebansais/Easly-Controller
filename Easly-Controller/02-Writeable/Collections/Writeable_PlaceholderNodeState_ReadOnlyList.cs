using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateReadOnlyList : IReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new int Count { get; }
        new IWriteablePlaceholderNodeState this[int index] { get; }
        bool Contains(IWriteablePlaceholderNodeState value);
        int IndexOf(IWriteablePlaceholderNodeState value);
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateReadOnlyList
    {
        public WriteablePlaceholderNodeStateReadOnlyList(IWriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IWriteablePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IWriteablePlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteablePlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteablePlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
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
