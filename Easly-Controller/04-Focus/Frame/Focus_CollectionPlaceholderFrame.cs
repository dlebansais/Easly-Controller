using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface IFocusCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame, IFocusFrame, IFocusBlockFrame
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class FocusCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusBlockFrameVisibility BlockVisibility { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public override IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            if (BlockVisibility != null && !BlockVisibility.IsBlockVisible((IFocusControllerView)controllerView, (IFocusNodeStateView)stateView, (IFocusBlockStateView)blockStateView, this))
            {
                IFocusCellViewCollection EmbeddingCellView = CreateEmptyEmbeddingCellView((IFocusNodeStateView)stateView);
                AssignEmbeddingCellView(blockStateView, EmbeddingCellView);
                return EmbeddingCellView;
            }
            else
                return base.BuildBlockCells(controllerView, stateView, blockStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates an empty IxxxCellViewCollection object.
        /// </summary>
        protected virtual IFocusCellViewCollection CreateEmptyEmbeddingCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusColumn(stateView, new FocusCellViewList());
        }
        #endregion
    }
}
