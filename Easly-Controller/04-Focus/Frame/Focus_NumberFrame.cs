namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public interface IFocusNumberFrame : IFrameNumberFrame, IFocusTextValueFrame
    {
    }

    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public class FocusNumberFrame : FrameNumberFrame, IFocusNumberFrame
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
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsPreferred { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= Visibility == null || Visibility.IsValid(nodeType);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            else
            {
                IFocusEmptyCellView EmptyCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, (IFocusCellViewCollection)parentCellView);
                ValidateEmptyCellView((IFocusCellViewTreeContext)context, EmptyCellView);

                Result = EmptyCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            if (Visibility == null || Visibility.IsVolatile || IsPreferred)
            {
                if (firstPreferredFrame == null || IsPreferred)
                    firstPreferredFrame = this;

                lastPreferredFrame = this;
            }
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((IFocusVisibleCellView)cellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            Debug.Assert(((IFocusVisibleCellView)cellView).Frame == this);
            IFocusCellViewCollection ParentCellView = ((IFocusVisibleCellView)cellView).ParentCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
            IFocusCellViewCollection ParentCellView = emptyCellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNumberFrame));
            return new FocusTextFocusableCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this, PropertyName);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNumberFrame));
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
