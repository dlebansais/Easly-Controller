namespace EaslyController.Frame
{
    using BaseNode;
    using Contracts;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IFrameRootNodeIndex : IWriteableRootNodeIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class FrameRootNodeIndex : WriteableRootNodeIndex, IFrameRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameRootNodeIndex"/> object.
        /// </summary>
        public static new FrameRootNodeIndex Empty { get; } = new FrameRootNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRootNodeIndex"/> class.
        /// </summary>
        protected FrameRootNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public FrameRootNodeIndex(Node node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameRootNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
