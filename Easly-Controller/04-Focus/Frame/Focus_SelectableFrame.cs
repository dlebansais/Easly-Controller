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
        IFocusFrame Content { get; set; }

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
        public IFocusFrame Content { get; set; }

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
        public virtual bool IsValid(Type nodeType)
        {
            if (Content == null)
                return false;

            if (string.IsNullOrEmpty(Name))
                return false;

            if (!(ParentFrame is IFocusSelectionFrame))
                return false;

            if (!Content.IsValid(nodeType))
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
            IFocusNodeFrame AsNodeFrame = Content as IFocusNodeFrame;
            Debug.Assert(AsNodeFrame != null);

            return AsNodeFrame.BuildNodeCells(context, parentCellView);
        }
        #endregion
    }
}
