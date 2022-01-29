namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IWriteableRootNodeIndex : IReadOnlyRootNodeIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class WriteableRootNodeIndex : ReadOnlyRootNodeIndex, IWriteableRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="WriteableRootNodeIndex"/> object.
        /// </summary>
        public static new WriteableRootNodeIndex Empty { get; } = new WriteableRootNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRootNodeIndex"/> class.
        /// </summary>
        protected WriteableRootNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public WriteableRootNodeIndex(Node node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableRootNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
