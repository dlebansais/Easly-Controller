using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public interface IFocusChangeBlockOperation : IFrameChangeBlockOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public class FocusChangeBlockOperation : FrameChangeBlockOperation, IFocusChangeBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusChangeBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block change is taking place.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeBlockOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, int blockIndex, bool isNested)
            : base(inner, blockIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Block state changed.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion
    }
}
