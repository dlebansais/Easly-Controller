using BaseNodeHelper;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IFramePlaceholderNodeStateView : IWriteablePlaceholderNodeStateView, IFrameNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class FramePlaceholderNodeStateView : WriteablePlaceholderNodeStateView, IFramePlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The child node state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FramePlaceholderNodeStateView(IFramePlaceholderNodeState state, IFrameTemplateSet templateSet)
            : base(state)
        {
            Debug.Assert(templateSet != null);

            IFrameNodeIndex NodeIndex = state.ParentIndex as IFrameNodeIndex;
            Debug.Assert(NodeIndex != null);

            Type NodeType = NodeIndex.Node.GetType();
            Debug.Assert(!NodeType.IsInterface && !NodeType.IsAbstract);

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);

            Template = templateSet.NodeTypeToTemplate(InterfaceType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template { get; }
        #endregion

        #region Client Interface
        public virtual void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            IFrameCellView RootCellView = null;
            RootCellView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion
    }
}
