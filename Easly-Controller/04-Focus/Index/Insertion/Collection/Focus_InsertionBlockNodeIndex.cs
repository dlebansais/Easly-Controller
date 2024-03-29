﻿namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public interface IFocusInsertionBlockNodeIndex : IFrameInsertionBlockNodeIndex, IFocusInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public abstract class FocusInsertionBlockNodeIndex : FrameInsertionBlockNodeIndex, IFocusInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FocusInsertionBlockNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
