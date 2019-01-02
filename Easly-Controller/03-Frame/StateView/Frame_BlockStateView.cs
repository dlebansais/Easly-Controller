using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IFrameBlockStateView : IWriteableBlockStateView
    {
        /// <summary>
        /// The block state.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        IFrameTemplate Template { get; }
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
        /// <param name="blockState">The block state.</param>
        /// <param name="templateSet">The template set used to display the block state.</param>
        public FrameBlockStateView(IFrameBlockState blockState, IFrameTemplateSet templateSet)
            : base(blockState)
        {
            Debug.Assert(templateSet != null);
            Debug.Assert(blockState.ParentInner != null);

            Type NodeType = blockState.ChildBlock.GetType();
            Template = templateSet.BlockTypeToTemplate(NodeType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public IFrameTemplate Template { get; }
        #endregion
    }
}
