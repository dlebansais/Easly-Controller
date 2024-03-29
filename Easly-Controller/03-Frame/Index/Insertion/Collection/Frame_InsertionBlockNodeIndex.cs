﻿namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public interface IFrameInsertionBlockNodeIndex : IWriteableInsertionBlockNodeIndex, IFrameInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public abstract class FrameInsertionBlockNodeIndex : WriteableInsertionBlockNodeIndex, IFrameInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FrameInsertionBlockNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
