using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IFrameBlockStateView : IWriteableBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFrameControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        IFrameCellView RootCellView { get; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        IFrameCellViewCollection EmbeddingCellView { get; }

        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to create cells.</param>
        void BuildRootCellView(IFrameNodeStateView stateView);

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        void AssignEmbeddingCellView(IFrameCellViewCollection embeddingCellView);

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to delete cells.</param>
        void ClearRootCellView(IFrameNodeStateView stateView);

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        void UpdateLineNumbers(ref int lineNumber, ref int columnNumber);
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class FrameBlockStateView : WriteableBlockStateView, IFrameBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FrameBlockStateView(IFrameControllerView controllerView, IFrameBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFrameControllerView ControllerView { get { return (IFrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type BlockType = BlockState.ParentInner.BlockType;
                return ControllerView.TemplateSet.BlockTypeToTemplate(BlockType);
            }
        }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public IFrameCellViewCollection EmbeddingCellView { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to create cells.</param>
        public virtual void BuildRootCellView(IFrameNodeStateView stateView)
        {
            IFrameBlockTemplate BlockTemplate = Template as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            Debug.Assert(RootCellView == null);
            RootCellView = BlockTemplate.BuildBlockCells(ControllerView, stateView, this);

            Debug.Assert(EmbeddingCellView != null);
        }

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        public virtual void AssignEmbeddingCellView(IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView != null);
            Debug.Assert(EmbeddingCellView == null);

            EmbeddingCellView = embeddingCellView;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to delete cells.</param>
        public virtual void ClearRootCellView(IFrameNodeStateView stateView)
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            EmbeddingCellView = null;
        }

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int columnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBlockStateView AsBlockStateView))
                return false;

            if (!base.IsEqual(comparer, AsBlockStateView))
                return false;

            if (Template != AsBlockStateView.Template)
                return false;

            if ((RootCellView != null && AsBlockStateView.RootCellView == null) || (RootCellView == null && AsBlockStateView.RootCellView != null))
                return false;

            if (RootCellView != null)
            {
                Debug.Assert(EmbeddingCellView != null);
                Debug.Assert(AsBlockStateView.EmbeddingCellView != null);

                if (!comparer.VerifyEqual(RootCellView, AsBlockStateView.RootCellView))
                    return false;

                if (!comparer.VerifyEqual(EmbeddingCellView, AsBlockStateView.EmbeddingCellView))
                    return false;
            }
            else
            {
                Debug.Assert(EmbeddingCellView == null);
                Debug.Assert(AsBlockStateView.EmbeddingCellView == null);
            }

            return true;
        }
        #endregion
    }
}
