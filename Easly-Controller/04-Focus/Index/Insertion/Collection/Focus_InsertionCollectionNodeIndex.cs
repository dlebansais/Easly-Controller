namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IFocusInsertionCollectionNodeIndex : IFrameInsertionCollectionNodeIndex, IFocusInsertionChildNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class FocusInsertionCollectionNodeIndex : FrameInsertionCollectionNodeIndex, IFocusInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FocusInsertionCollectionNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public abstract override bool IsEqual(CompareEqual comparer, IEqualComparable other);
        #endregion
    }
}
