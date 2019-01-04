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
        IFrameMutableCellViewCollection EmbeddingCellView { get; }

        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the state is initialized.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        void BuildRootCellView(IFrameControllerView controllerView, IFrameNodeStateView stateView);

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        void AssignEmbeddingCellView(IFrameMutableCellViewCollection embeddingCellView);
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
        /// <param name="templateSet">The template set used to display the block state.</param>
        public FrameBlockStateView(IFrameControllerView controllerView, IFrameBlockState blockState, IFrameTemplateSet templateSet)
            : base(controllerView, blockState)
        {
            Debug.Assert(templateSet != null);
            Debug.Assert(blockState.ParentInner != null);

            Type BlockType = blockState.ParentInner.BlockType;
            Template = templateSet.BlockTypeToTemplate(BlockType);
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
        public IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public IFrameMutableCellViewCollection EmbeddingCellView { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the state is initialized.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public virtual void BuildRootCellView(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            Debug.Assert(controllerView != null);

            IFrameBlockTemplate NodeTemplate = Template as IFrameBlockTemplate;
            Debug.Assert(NodeTemplate != null);

            RootCellView = NodeTemplate.BuildBlockCells(controllerView, stateView, this);

            Debug.Assert(EmbeddingCellView != null);
        }

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        public virtual void AssignEmbeddingCellView(IFrameMutableCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView != null);
            Debug.Assert(EmbeddingCellView == null);

            EmbeddingCellView = embeddingCellView;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameBlockStateView"/> objects.
        /// </summary>
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

            if (!comparer.VerifyEqual(RootCellView, AsBlockStateView.RootCellView))
                return false;

            if (!comparer.VerifyEqual(EmbeddingCellView, AsBlockStateView.EmbeddingCellView))
                return false;

            return true;
        }
        #endregion
    }
}
