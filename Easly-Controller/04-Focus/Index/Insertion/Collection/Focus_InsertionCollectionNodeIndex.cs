using BaseNode;
using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IFocusInsertionCollectionNodeIndex : IFrameInsertionCollectionNodeIndex, IFocusInsertionChildIndex, IFocusNodeIndex
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
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusInsertionCollectionNodeIndex AsInsertionCollectionNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsInsertionCollectionNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
