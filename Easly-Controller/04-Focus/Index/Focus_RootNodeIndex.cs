namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IFocusRootNodeIndex : IFrameRootNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class FocusRootNodeIndex : FrameRootNodeIndex, IFocusRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public FocusRootNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusRootNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
