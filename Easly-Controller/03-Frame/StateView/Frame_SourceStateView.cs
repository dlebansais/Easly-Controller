using BaseNode;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IFrameSourceStateView : IWriteableSourceStateView, IFramePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFrameSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class FrameSourceStateView : WriteableSourceStateView, IFrameSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSourceStateView"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FrameSourceStateView(IFrameSourceState state, IFrameTemplateSet templateSet)
            : base(state)
        {
            Debug.Assert(templateSet != null);
            Debug.Assert(state.ParentInner != null);

            Type NodeType = typeof(IIdentifier);
            Template = templateSet.NodeTypeToTemplate(NodeType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFrameSourceState State { get { return (IFrameSourceState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        IFramePlaceholderNodeState IFramePlaceholderNodeStateView.State { get { return State; } }

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

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            RootCellView = NodeTemplate.BuildNodeCells(controllerView, this);
        }

        public virtual void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            IFrameCellView RootCellView = null;
            RootCellView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion
    }
}
