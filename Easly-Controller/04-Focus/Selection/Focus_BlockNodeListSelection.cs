﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using EaslyController.Controller;
    using EaslyController.Writeable;
    using NotNullReflection;

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
        /// Index following the last selected node in the block.
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
    [DebuggerDisplay("Block Index {BlockIndex}, from {StartIndex} to {EndIndex}")]
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
        /// <param name="endIndex">Index following the last selected node in the block.</param>
        public FocusBlockNodeListSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
            : base(stateView)
        {
            Node Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(Node, propertyName, /*out Type childInterfaceType,*/ out Type childNodeType));

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
        /// Index following the last selected node in the block.
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

            IFocusBlockState BlockState = (IFocusBlockState)ParentInner.BlockStateList[BlockIndex];

            List<Node> NodeList = new List<Node>();
            for (int i = StartIndex; i < EndIndex; i++)
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
            Contract.RequireNotNull(dataObject, out IDataObject DataObject);

            CutOrDelete(DataObject, out isDeleted);
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

            IFocusBlockState BlockState = (IFocusBlockState)ParentInner.BlockStateList[BlockIndex];
            Debug.Assert(StartIndex < BlockState.StateList.Count);
            Debug.Assert(EndIndex <= BlockState.StateList.Count);
            Debug.Assert(StartIndex <= EndIndex);

            IList<Node> NodeList = null;

            if (ClipboardHelper.TryReadNodeList(out NodeList))
            { }
            else if (ClipboardHelper.TryReadNode(out Node Node))
            {
                NodeList = new List<Node>() { Node };
            }

            if (NodeList != null)
            {
                if (NodeList.Count == 0 || ParentInner.InterfaceType.IsAssignableFrom(Type.FromGetType(NodeList[0])))
                {
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>();
                    FocusController Controller = StateView.ControllerView.Controller;
                    int OldNodeCount = ParentInner.Count;
                    int SelectionCount = EndIndex - StartIndex;
                    int InsertionNodeIndex = EndIndex;

                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        Node NewNode = NodeList[i] as Node;
                        IFocusInsertionExistingBlockNodeIndex InsertedIndex = CreateExistingBlockNodeIndex(ParentInner.Owner.Node, PropertyName, NewNode, BlockIndex, StartIndex + i);
                        IndexList.Add(InsertedIndex);
                    }

                    Controller.ReplaceNodeRange(ParentInner, BlockIndex, StartIndex, EndIndex, IndexList);

                    Debug.Assert(ParentInner.Count == OldNodeCount + NodeList.Count - SelectionCount);

                    StateView.ControllerView.ClearSelection();
                    isChanged = NodeList.Count > 0 || SelectionCount > 0;
                }
            }
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Delete(out bool isDeleted)
        {
            CutOrDelete(null, out isDeleted);
        }

        private void CutOrDelete(IDataObject dataObject, out bool isDeleted)
        {
            isDeleted = false;

            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = (IFocusBlockState)ParentInner.BlockStateList[BlockIndex];

            int OldNodeCount = ParentInner.Count;
            int SelectionCount = EndIndex - StartIndex;

            if (SelectionCount < BlockState.StateList.Count || ParentInner.BlockStateList.Count > 1 || !NodeHelper.IsCollectionNeverEmpty(State.Node, PropertyName))
            {
                if (dataObject != null)
                {
                    List<Node> NodeList = new List<Node>();
                    for (int i = StartIndex; i < EndIndex; i++)
                        NodeList.Add(BlockState.StateList[i].Node);

                    ClipboardHelper.WriteNodeList(dataObject, NodeList);
                }

                FocusController Controller = StateView.ControllerView.Controller;
                Controller.RemoveNodeRange(ParentInner, BlockIndex, StartIndex, EndIndex);

                Debug.Assert(ParentInner.Count == OldNodeCount - SelectionCount);

                StateView.ControllerView.ClearSelection();
                isDeleted = true;
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockNodeListSelection>());
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
