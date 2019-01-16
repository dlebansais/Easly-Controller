using EaslyController.Frame;
using System;
using System.Diagnostics;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame selectable by name.
    /// </summary>
    public interface IFocusSelectableFrame : IFocusFrame, IFocusNodeFrame
    {
        /// <summary>
        /// The selectable frame.
        /// (Set in Xaml)
        /// </summary>
        IFocusNodeFrame Content { get; set; }

        /// <summary>
        /// Frame name for selection.
        /// (Set in Xaml)
        /// </summary>
        string Name { get; set; }
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
        public virtual bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (Content == null)
                return false;

            if (string.IsNullOrEmpty(Name))
                return false;

            if (!(ParentFrame is IFocusSelectionFrame))
                return false;

            if (!Content.IsValid(nodeType, nodeTemplateTable))
                return false;

            return true;
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
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame)
        {
            frame = null;
            return false;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The first preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            Content.GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }
        #endregion
    }
}
