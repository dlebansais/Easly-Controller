﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface IFocusDiscreteFrame : IFrameDiscreteFrame, IFocusValueFrame
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        new FocusKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
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
        public new FocusKeywordFrameList Items { get { return (FocusKeywordFrameList)base.Items; } }

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
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
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

        /// <summary>
        /// Gets selectors in the frame and nested frames.
        /// </summary>
        /// <param name="selectorTable">The table of selectors to update.</param>
        public virtual void CollectSelectors(Dictionary<string, FocusFrameSelectorList> selectorTable)
        {
            Debug.Assert(!selectorTable.ContainsKey(PropertyName));
        }
        #endregion

        #region Implementation
        private protected override void ValidateDiscreteContentFocusableCellView(IFrameCellViewTreeContext context, IFrameKeywordFrame keywordFrame, IFrameDiscreteContentFocusableCellView cellView)
        {
            Debug.Assert(((IFocusDiscreteContentFocusableCellView)cellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            Debug.Assert(((IFocusDiscreteContentFocusableCellView)cellView).Frame == this);
            Debug.Assert(((IFocusDiscreteContentFocusableCellView)cellView).KeywordFrame == (IFocusKeywordFrame)keywordFrame);
            IFocusCellViewCollection ParentCellView = ((IFocusDiscreteContentFocusableCellView)cellView).ParentCellView;
        }

        private protected virtual void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
            IFocusCellViewCollection ParentCellView = emptyCellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxKeywordFrameList object.
        /// </summary>
        private protected override FrameKeywordFrameList CreateKeywordFrameList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusDiscreteFrame>());
            return new FocusKeywordFrameList();
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentFocusableCellView object.
        /// </summary>
        private protected override IFrameDiscreteContentFocusableCellView CreateDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameKeywordFrame keywordFrame)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusDiscreteFrame>());
            return new FocusDiscreteContentFocusableCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this, PropertyName, (IFocusKeywordFrame)keywordFrame);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusDiscreteFrame>());
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
