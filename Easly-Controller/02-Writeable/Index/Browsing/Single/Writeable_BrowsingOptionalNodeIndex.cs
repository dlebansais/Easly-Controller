using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IWriteableBrowsingOptionalNodeIndex : IReadOnlyBrowsingOptionalNodeIndex, IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public class WriteableBrowsingOptionalNodeIndex : ReadOnlyBrowsingOptionalNodeIndex, IWriteableBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.
        public WriteableBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion
    }
}
