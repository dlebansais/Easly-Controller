﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public interface IFocusChangeNodeOperation : IFrameChangeNodeOperation, IFocusOperation
    {
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        new IFocusIndex NodeIndex { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        new IFocusPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class FocusChangeNodeOperation : FrameChangeNodeOperation, IFocusChangeNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusChangeNodeOperation"/>.
        /// </summary>
        /// <param name="nodeIndex">Index of the changed node.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeNodeOperation(IFocusIndex nodeIndex, bool isNested)
            : base(nodeIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        public new IFocusIndex NodeIndex { get { return (IFocusIndex)base.NodeIndex; } }

        /// <summary>
        /// State changed.
        /// </summary>
        public new IFocusPlaceholderNodeState State { get { return (IFocusPlaceholderNodeState)base.State; } }
        #endregion
    }
}
