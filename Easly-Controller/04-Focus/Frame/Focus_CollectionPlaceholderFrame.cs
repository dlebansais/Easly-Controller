namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface IFocusCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame, IFocusFrame, IFocusBlockFrame, IFocusFrameWithSelector
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class FocusCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCollectionPlaceholderFrame"/> class.
        /// </summary>
        public FocusCollectionPlaceholderFrame()
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
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public FocusFrameSelectorList Selectors { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public abstract override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView);
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected virtual FocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
