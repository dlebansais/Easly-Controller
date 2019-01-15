using EaslyController.Frame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame selecting sub-frames.
    /// </summary>
    public interface IFocusSelectionFrame : IFocusFrame, IFocusNodeFrame
    {
        /// <summary>
        /// List of frames among which to select.
        /// </summary>
        IFocusFrameList Items { get; }
    }

    /// <summary>
    /// Frame selecting sub-frames.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusSelectionFrame : IFocusSelectionFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusSelectionFrame"/> object.
        /// </summary>
        public FocusSelectionFrame()
        {
            Items = CreateFrameList();
        }
        #endregion

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
        /// List of frames among which to select.
        /// </summary>
        public IFocusFrameList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public virtual bool IsValid(Type nodeType)
        {
            if (Items.Count == 0)
                return false;

            if (ParentFrame != FrameFrame.Root)
                return false;

            List<string> NameList = new List<string>();
            foreach (IFocusFrame Item in Items)
                if (Item is IFocusSelectableFrame AsSelectable)
                {
                    if (!AsSelectable.IsValid(nodeType))
                        return false;

                    if (NameList.Contains(AsSelectable.Name))
                        return false;

                    NameList.Add(AsSelectable.Name);
                }
                else
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

            foreach (IFocusFrame Item in Items)
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

            foreach (IFocusFrame Item in Items)
                if (Item is IFocusSelectableFrame AsSelectable)
                    if (AsSelectable.Name == SelectorName)
                    {
                        AsSelectable.BuildNodeCells(context, parentCellView);
                        break;
                    }

            Debug.Assert(false);
            return null;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected virtual IFocusFrameList CreateFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSelectionFrame));
            return new FocusFrameList();
        }
        #endregion
    }
}
