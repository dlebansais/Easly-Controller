﻿namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using NotNullReflection;

    /// <summary>
    /// An empty selection.
    /// </summary>
    public interface ILayoutEmptySelection : IFocusEmptySelection, ILayoutSelection
    {
    }

    /// <summary>
    /// An empty selection.
    /// </summary>
    public class LayoutEmptySelection : FocusEmptySelection, ILayoutEmptySelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEmptySelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public LayoutEmptySelection(ILayoutNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints the selection.
        /// </summary>
        public virtual void Print()
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionListNodeIndex CreateListNodeIndex(Node parentNode, string propertyName, Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutEmptySelection>());
            return new LayoutInsertionListNodeIndex(parentNode, propertyName, node, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutEmptySelection>());
            return new LayoutInsertionNewBlockNodeIndex(parentNode, propertyName, node, blockIndex, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutEmptySelection>());
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
