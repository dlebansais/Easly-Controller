﻿namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IFocusBrowsingNewBlockNodeIndex : IFrameBrowsingNewBlockNodeIndex, IFocusBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class FocusBrowsingNewBlockNodeIndex : FrameBrowsingNewBlockNodeIndex, IFocusBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusBrowsingNewBlockNodeIndex"/> object.
        /// </summary>
        public static new FocusBrowsingNewBlockNodeIndex Empty { get; } = new FocusBrowsingNewBlockNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        protected FocusBrowsingNewBlockNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public FocusBrowsingNewBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex)
            : base(parentNode, node, propertyName, blockIndex)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBrowsingNewBlockNodeIndex>());
            return new FocusBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
