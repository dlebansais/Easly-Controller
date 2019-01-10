using BaseNode;
using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IFocusBrowsingCollectionNodeIndex : IFrameBrowsingCollectionNodeIndex, IFocusBrowsingChildIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public abstract class FocusBrowsingCollectionNodeIndex : FrameBrowsingCollectionNodeIndex, IFocusBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public FocusBrowsingCollectionNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
