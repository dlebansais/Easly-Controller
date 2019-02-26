﻿namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Frame selecting sub-frames.
    /// </summary>
    public interface IFocusSelectionFrame : IFocusFrame, IFocusNodeFrame
    {
        /// <summary>
        /// List of frames among which to select.
        /// </summary>
        IFocusSelectableFrameList Items { get; }
    }

    /// <summary>
    /// Frame selecting sub-frames.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusSelectionFrame : FocusFrame, IFocusSelectionFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSelectionFrame"/> class.
        /// </summary>
        public FocusSelectionFrame()
        {
            Items = CreateSelectableFrameList();
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of frames among which to select.
        /// </summary>
        public IFocusSelectableFrameList Items { get; }

        /// <summary></summary>
        private protected virtual bool IsParentRoot { get { return ParentFrame == FocusFrame.FocusRoot; } }
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
            IsValid &= Items.Count > 0;
            IsValid &= IsParentRoot;

            List<string> NameList = new List<string>();
            int SelectionCommentFrameCount = -1;
            foreach (IFocusSelectableFrame Item in Items)
            {
                int SelectableCommentFrameCount = 0;
                IsValid &= Item.IsValid(nodeType, nodeTemplateTable, ref SelectableCommentFrameCount);
                IsValid &= !NameList.Contains(Item.Name);

                // Use the count for the first frame as base count.
                if (SelectionCommentFrameCount < 0)
                    SelectionCommentFrameCount = SelectableCommentFrameCount;

                // All selectable frames must have the same count.
                IsValid &= SelectionCommentFrameCount == SelectableCommentFrameCount;

                NameList.Add(Item.Name);
            }

            // Use the common count of all selectable frames as the count of the selection frame.
            commentFrameCount += SelectionCommentFrameCount;

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public override void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            base.UpdateParent(parentTemplate, parentFrame);

            Debug.Assert(ParentTemplate == parentTemplate);
            Debug.Assert(ParentFrame == parentFrame);

            foreach (IFocusSelectableFrame Item in Items)
                Item.UpdateParent(parentTemplate, this);
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            Debug.Assert(((IFocusCellViewTreeContext)context).SelectorTable.ContainsKey(ParentTemplate.NodeType));
            string SelectorName = ((IFocusCellViewTreeContext)context).SelectorTable[ParentTemplate.NodeType];

            IFrameCellView Result = null;

            foreach (IFocusSelectableFrame Item in Items)
                if (Item.Name == SelectorName)
                {
                    Result = Item.BuildNodeCells(context, parentCellView);
                    break;
                }

            Debug.Assert(Result != null);
            return Result;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            Debug.Assert(Items.Count > 0);

            Items[0].GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSelectableFrameList object.
        /// </summary>
        private protected virtual IFocusSelectableFrameList CreateSelectableFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSelectionFrame));
            return new FocusSelectableFrameList();
        }
        #endregion
    }
}
