using EaslyController.Frame;
using System;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface IFocusDiscreteFrame : IFrameDiscreteFrame, IFocusValueFrame
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        new IFocusKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Focus describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusDiscreteFrame : FrameDiscreteFrame, IFocusDiscreteFrame
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
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusKeywordFrameList Items { get { return (IFocusKeywordFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Visibility != null && !Visibility.IsValid(nodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public override IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            if (Visibility != null && !Visibility.IsVisible((IFocusControllerView)controllerView, (IFocusNodeStateView)stateView, this))
            {
                IFocusEmptyCellView EmbeddingCellView = CreateEmptyCellView((IFocusNodeStateView)stateView);
                return EmbeddingCellView;
            }
            else
                return base.BuildNodeCells(controllerView, stateView, parentCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxKeywordFrameList object.
        /// </summary>
        protected override IFrameKeywordFrameList CreateKeywordFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusDiscreteFrame));
            return new FocusKeywordFrameList();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusDiscreteFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
