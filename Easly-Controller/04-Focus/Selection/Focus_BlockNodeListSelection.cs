namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Controller;
    using EaslyController.Writeable;

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public interface IFocusBlockNodeListSelection : IFocusContentSelection
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
    public class FocusBlockNodeListSelection : FocusSelection, IFocusBlockNodeListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockNodeListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first selected node in the block.</param>
        /// <param name="endIndex">Index of the last selected node in the block.</param>
        public FocusBlockNodeListSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(Node, propertyName, out Type childInterfaceType, out Type childNodeType));

            PropertyName = propertyName;
            BlockIndex = blockIndex;

            if (startIndex <= endIndex)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
            else
            {
                StartIndex = endIndex;
                EndIndex = startIndex;
            }
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
            if (startIndex <= endIndex)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
            else
            {
                StartIndex = endIndex;
                EndIndex = startIndex;
            }
        }

        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = ParentInner.BlockStateList[BlockIndex];

            List<INode> NodeList = new List<INode>();
            for (int i = StartIndex; i <= EndIndex; i++)
                NodeList.Add(BlockState.StateList[i].Node);

            ClipboardHelper.WriteNodeList(dataObject, NodeList);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Cut(IDataObject dataObject, out bool isDeleted)
        {
            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = ParentInner.BlockStateList[BlockIndex];

            int SelectionCount = EndIndex - StartIndex + 1;
            if (SelectionCount < BlockState.StateList.Count)
            {
                List<INode> NodeList = new List<INode>();
                for (int i = StartIndex; i <= EndIndex; i++)
                    NodeList.Add(BlockState.StateList[i].Node);

                ClipboardHelper.WriteNodeList(dataObject, NodeList);

                IFocusController Controller = StateView.ControllerView.Controller;
                int OldNodeCount = BlockState.StateList.Count;

                for (int i = StartIndex; i <= EndIndex; i++)
                {
                    IFocusNodeState ChildState = BlockState.StateList[StartIndex];
                    IFocusBrowsingCollectionNodeIndex NodeIndex = ChildState.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                    Debug.Assert(NodeIndex != null);

                    Controller.Remove(ParentInner, NodeIndex);
                }

                Debug.Assert(BlockState.StateList.Count == OldNodeCount + NodeList.Count - SelectionCount);

                StateView.ControllerView.ClearSelection();
                isDeleted = true;
            }
            else
                isDeleted = false;
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public override void Paste(out bool isChanged)
        {
            isChanged = false;

            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = ParentInner.BlockStateList[BlockIndex];
            Debug.Assert(StartIndex <= BlockState.StateList.Count);
            Debug.Assert(EndIndex <= BlockState.StateList.Count);
            Debug.Assert(StartIndex <= EndIndex);

            IList<INode> NodeList = null;

            if (ClipboardHelper.TryReadNodeList(out NodeList))
            { }

            else if (ClipboardHelper.TryReadNode(out INode Node))
            {
                NodeList = new List<INode>() { Node };
            }

            if (NodeList != null && NodeList.Count > 0)
            {
                if (ParentInner.InterfaceType.IsAssignableFrom(NodeList[0].GetType()))
                {
                    // Insert first to prevent empty lists.
                    IFocusController Controller = StateView.ControllerView.Controller;
                    int OldNodeCount = BlockState.StateList.Count;
                    int SelectionCount = EndIndex - StartIndex + 1;
                    int InsertionNodeIndex = EndIndex + 1;

                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        INode NewNode = NodeList[i] as INode;
                        IFocusInsertionCollectionNodeIndex InsertedIndex = new FocusInsertionExistingBlockNodeIndex(ParentInner.Owner.Node, PropertyName, NewNode, BlockIndex, InsertionNodeIndex);
                        Debug.Assert(InsertedIndex != null);

                        Controller.Insert(ParentInner, InsertedIndex, out IWriteableBrowsingCollectionNodeIndex NodeIndex);

                        InsertionNodeIndex++;
                    }

                    for (int i = StartIndex; i <= EndIndex; i++)
                    {
                        IFocusNodeState ChildState = BlockState.StateList[StartIndex];
                        IFocusBrowsingCollectionNodeIndex NodeIndex = ChildState.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(NodeIndex != null);

                        Controller.Remove(ParentInner, NodeIndex);
                    }

                    Debug.Assert(BlockState.StateList.Count == OldNodeCount + NodeList.Count - SelectionCount);

                    StateView.ControllerView.ClearSelection();
                    isChanged = true;
                }
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Block Index {BlockIndex}, from {StartIndex} to {EndIndex}";
        }
        #endregion
    }
}
