namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface ILayoutBrowsingNewBlockNodeIndex : IFocusBrowsingNewBlockNodeIndex, ILayoutBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class LayoutBrowsingNewBlockNodeIndex : FocusBrowsingNewBlockNodeIndex, ILayoutBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutBrowsingNewBlockNodeIndex"/> object.
        /// </summary>
        public static new LayoutBrowsingNewBlockNodeIndex Empty { get; } = new LayoutBrowsingNewBlockNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        protected LayoutBrowsingNewBlockNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public LayoutBrowsingNewBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex)
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutBrowsingNewBlockNodeIndex));
            return new LayoutBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
