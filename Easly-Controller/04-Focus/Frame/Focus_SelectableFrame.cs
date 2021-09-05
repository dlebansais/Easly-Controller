namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Frame selectable by name.
    /// </summary>
    public interface IFocusSelectableFrame : IFocusFrame, IFocusNodeFrame
    {
        /// <summary>
        /// The selectable frame.
        /// (Set in Xaml)
        /// </summary>
        IFocusNodeFrame Content { get; }

        /// <summary>
        /// Frame name for selection.
        /// (Set in Xaml)
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Frame selectable by name.
    /// </summary>
    [ContentProperty("Content")]
    public class FocusSelectableFrame : IFocusSelectableFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public IFocusTemplate ParentTemplate { get; private set; }
        IFrameTemplate IFrameFrame.ParentTemplate { get { return ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public IFocusFrame ParentFrame { get; private set; }
        IFrameFrame IFrameFrame.ParentFrame { get { return ParentFrame; } }

        /// <summary>
        /// The selectable frame.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrame Content { get; set; }

        /// <summary>
        /// Frame name for selection.
        /// (Set in Xaml)
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public virtual bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= Content != null;
            IsValid &= !string.IsNullOrEmpty(Name);
            IsValid &= ParentFrame is IFocusSelectionFrame;
            IsValid &= Content.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public virtual void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            Debug.Assert(parentTemplate is IFocusTemplate);
            Debug.Assert(parentFrame is IFocusFrame);

            Debug.Assert(ParentTemplate == null);
            ParentTemplate = (IFocusTemplate)parentTemplate;

            Debug.Assert(ParentFrame == null);
            ParentFrame = (IFocusFrame)parentFrame;

            Content.UpdateParent(parentTemplate, this);
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            return Content.BuildNodeCells(context, parentCellView);
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            Content.GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }

        /// <summary>
        /// Gets selectors in the frame and nested frames.
        /// </summary>
        /// <param name="selectorTable">The table of selectors to update.</param>
        public virtual void CollectSelectors(Dictionary<string, FocusFrameSelectorList> selectorTable)
        {
            Content.CollectSelectors(selectorTable);
        }
        #endregion
    }
}
