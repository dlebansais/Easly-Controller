﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public interface IFocusHorizontalBlockListFrame : IFrameHorizontalBlockListFrame, IFocusBlockListFrame
    {
    }

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public class FocusHorizontalBlockListFrame : FrameHorizontalBlockListFrame, IFocusHorizontalBlockListFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusHorizontalBlockListFrame"/> class.
        /// </summary>
        public FocusHorizontalBlockListFrame()
        {
            Selectors = CreateEmptySelectorList();
        }
        #endregion

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
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public FocusFrameSelectorList Selectors { get; }

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

            foreach (IFocusFrameSelector Selector in Selectors)
                IsValid &= Selector.IsValid(nodeType, (FocusTemplateReadOnlyDictionary)nodeTemplateTable, PropertyName);

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
            ((IFocusCellViewTreeContext)context).ChangeIsUserVisible(false, out bool OldIsUserVisible);
            ((IFocusCellViewTreeContext)context).AddSelectors(Selectors);

            IFocusCellViewCollection EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            ((IFocusCellViewTreeContext)context).RemoveSelectors(Selectors);
            ((IFocusCellViewTreeContext)context).RestoreIsUserVisible(OldIsUserVisible);
            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame)
        {
            frame = null;
            bool Found = false;

            if (propertyName == PropertyName)
            {
                frame = this;
                Found = true;
            }

            return Found;
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

            selectorTable.Add(PropertyName, Selectors);
        }
        #endregion

        #region Implementation
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((IFocusCellViewCollection)embeddingCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            IFocusCellViewCollection ParentCellView = ((IFocusCellViewCollection)embeddingCellView).ParentCellView;
        }

        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((IFocusBlockCellView)blockCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusBlockCellView)blockCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusBlockCellView)blockCellView).BlockStateView == (FocusBlockStateView)blockStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalBlockListFrame>());
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalBlockListFrame>());
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (FocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalBlockListFrame>());
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (FocusCellViewList)list, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected virtual FocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalBlockListFrame>());
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
