using EaslyController.Frame;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IFocusBlockStateView : IFrameBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFocusControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new IFocusBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        new IFocusTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        new IFocusCellView RootCellView { get; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        new IFocusCellViewCollection EmbeddingCellView { get; }
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class FocusBlockStateView : FrameBlockStateView, IFocusBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FocusBlockStateView(IFocusControllerView controllerView, IFocusBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public new IFocusCellViewCollection EmbeddingCellView { get { return (IFocusCellViewCollection)base.EmbeddingCellView; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusBlockStateView AsBlockStateView))
                return false;

            if (!base.IsEqual(comparer, AsBlockStateView))
                return false;

            return true;
        }
        #endregion
    }
}
