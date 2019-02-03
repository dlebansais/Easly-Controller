namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IWriteableBrowsingSourceIndex : IReadOnlyBrowsingSourceIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    internal class WriteableBrowsingSourceIndex : ReadOnlyBrowsingSourceIndex, IWriteableBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public WriteableBrowsingSourceIndex(IBlock block)
            : base(block)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBrowsingSourceIndex AsBrowsingSourceIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingSourceIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
