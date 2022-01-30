namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface ILayoutRootNodeIndex : IFocusRootNodeIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class LayoutRootNodeIndex : FocusRootNodeIndex, ILayoutRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutRootNodeIndex"/> object.
        /// </summary>
        public static new LayoutRootNodeIndex Empty { get; } = new LayoutRootNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRootNodeIndex"/> class.
        /// </summary>
        protected LayoutRootNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public LayoutRootNodeIndex(Node node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutRootNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
