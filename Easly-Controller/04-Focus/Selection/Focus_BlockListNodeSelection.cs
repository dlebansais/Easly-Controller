namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public interface IFocusBlockListNodeSelection : IFocusContentSelection
    {
        /// <summary>
        /// Index of the block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Index of the first selected node in the block.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Index of the last selected node in the block.
        /// </summary>
        int EndIndex { get; }

        /// <summary>
        /// Updates the selection with new start and end index values.
        /// </summary>
        /// <param name="startIndex">The new start index value.</param>
        /// <param name="endIndex">The new end index value.</param>
        void Update(int startIndex, int endIndex);
    }

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public class FocusBlockListNodeSelection : FocusSelection, IFocusBlockListNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first selected node in the block.</param>
        /// <param name="endIndex">Index of the last selected node in the block.</param>
        public FocusBlockListNodeSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(Node, propertyName, out Type childInterfaceType, out Type childNodeType));

            PropertyName = propertyName;
            BlockIndex = blockIndex;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Index of the first selected node in the block.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Index of the last selected node in the block.
        /// </summary>
        public int EndIndex { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the selection with new start and end index values.
        /// </summary>
        /// <param name="startIndex">The new start index value.</param>
        /// <param name="endIndex">The new end index value.</param>
        public virtual void Update(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
        #endregion
    }
}
