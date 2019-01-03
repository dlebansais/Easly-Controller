using BaseNodeHelper;
using Easly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IFrameOptionalNodeStateView : IWriteableOptionalNodeStateView, IFrameNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IFrameOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class FrameOptionalNodeStateView : WriteableOptionalNodeStateView, IFrameOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The optional node state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FrameOptionalNodeStateView(IFrameOptionalNodeState state, IFrameTemplateSet templateSet)
            : base(state)
        {
            Debug.Assert(templateSet != null);
            Debug.Assert(state.ParentIndex != null);

            IOptionalReference Optional = state.ParentIndex.Optional;
            Debug.Assert(Optional != null);

            if (Optional.IsAssigned)
            {
                Type NodeType = state.Node.GetType();
                Debug.Assert(!NodeType.IsInterface && !NodeType.IsAbstract);

                Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);

                Template = templateSet.NodeTypeToTemplate(InterfaceType);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the state is initialized.</param>
        public virtual void BuildRootCellView(IFrameControllerView controllerView)
        {
            Debug.Assert(controllerView != null);

            IOptionalReference Optional = State.ParentIndex.Optional;
            Debug.Assert(Optional != null);

            if (Optional.IsAssigned)
            {
                IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
                Debug.Assert(NodeTemplate != null);

                RootCellView = NodeTemplate.BuildNodeCells(controllerView, this);
            }
        }

        public virtual void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            IOptionalReference Optional = State.ParentIndex.Optional;
            Debug.Assert(Optional != null);

            if (Optional.IsAssigned)
            {
                IFrameCellView RootCellView = null;
                RootCellView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
            }
        }
        #endregion
    }
}
