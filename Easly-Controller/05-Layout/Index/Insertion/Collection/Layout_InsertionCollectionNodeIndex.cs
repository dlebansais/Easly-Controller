namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface ILayoutInsertionCollectionNodeIndex : IFocusInsertionCollectionNodeIndex, ILayoutInsertionChildNodeIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class LayoutInsertionCollectionNodeIndex : FocusInsertionCollectionNodeIndex, ILayoutInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public LayoutInsertionCollectionNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInsertionCollectionNodeIndex AsInsertionCollectionNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionCollectionNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
