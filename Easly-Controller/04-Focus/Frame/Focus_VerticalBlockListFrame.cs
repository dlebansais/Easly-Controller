using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for a block list displayed vertically.
    /// </summary>
    public interface IFocusVerticalBlockListFrame : IFrameVerticalBlockListFrame, IFocusBlockListFrame
    {
    }

    /// <summary>
    /// Focus for a block list displayed vertically.
    /// </summary>
    public class FocusVerticalBlockListFrame : FrameVerticalBlockListFrame, IFocusVerticalBlockListFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusVerticalBlockListFrame"/> object.
        /// </summary>
        public FocusVerticalBlockListFrame()
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
        public IFocusFrameSelectorList Selectors { get; }

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
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (!base.IsValid(nodeType, nodeTemplateTable))
                return false;

            if (Visibility != null && !Visibility.IsValid(nodeType))
                return false;

            return true;
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

            IFocusCellViewCollection EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                foreach (IFocusCellView CellView in EmbeddingCellView.CellViewList)
                {
                    IFocusBlockCellView AsBlockCellView = CellView as IFocusBlockCellView;
                    Debug.Assert(AsBlockCellView != null);
                    Debug.Assert(AsBlockCellView.BlockStateView != null);
                    Debug.Assert(AsBlockCellView.BlockStateView.RootCellView is IFocusEmptyCellView);
                }
            }

            ((IFocusCellViewTreeContext)context).RemoveSelectors(Selectors);
            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame)
        {
            if (propertyName == PropertyName)
            {
                frame = this;
                return true;
            }
            else
            {
                frame = null;
                return false;
            }
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The first preferred frame found.</param>
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

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalBlockListFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalBlockListFrame));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalBlockListFrame));
            return new FocusColumn((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        protected virtual IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalBlockListFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
