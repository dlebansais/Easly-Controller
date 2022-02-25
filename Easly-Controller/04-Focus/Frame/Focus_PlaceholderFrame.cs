﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public interface IFocusPlaceholderFrame : IFramePlaceholderFrame, IFocusNamedFrame, IFocusNodeFrameWithVisibility, IFocusFrameWithSelector, IFocusSelectorPropertyFrame
    {
    }

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public class FocusPlaceholderFrame : FramePlaceholderFrame, IFocusPlaceholderFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderFrame"/> class.
        /// </summary>
        public FocusPlaceholderFrame()
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
            ((IFocusCellViewTreeContext)context).AddSelectors(Selectors);

            IFocusCellView EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            Debug.Assert(EmbeddingCellView != null);

            ((IFocusCellViewTreeContext)context).RemoveSelectors(Selectors);
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
        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((IFocusContainerCellView)containerCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ChildStateView == (IFocusNodeStateView)childStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusPlaceholderFrame>());
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected virtual FocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusPlaceholderFrame>());
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
