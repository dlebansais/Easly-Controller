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
    /// A selection of blocks in a block list.
    /// </summary>
    public interface IFocusBlockListSelection : IFocusContentSelection
    {
        /// <summary>
        /// Index of the first selected block.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Index of the last selected block.
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
    /// A selection of blocks in a block list.
    /// </summary>
    public class FocusBlockListSelection : FocusSelection, IFocusBlockListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected block.</param>
        /// <param name="endIndex">Index of the last selected block.</param>
        public FocusBlockListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(Node, propertyName, out Type childInterfaceType, out Type childNodeType));

            PropertyName = propertyName;

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
        /// Index of the first selected block.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Index of the last selected block.
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

#if !TRAVIS
        /// <summary>
        /// Adds the selection to the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);

            Debug.Assert(StartIndex <= EndIndex);

            List<IBlock> BlockList = new List<IBlock>();
            for (int i = StartIndex; i <= EndIndex; i++)
            {
                IFocusBlockState BlockState = ParentInner.BlockStateList[i];
                BlockList.Add(BlockState.ChildBlock);
            }

            ClipboardHelper.WriteBlockList(dataObject, BlockList);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Cut(IDataObject dataObject, out bool isDeleted)
        {
            isDeleted = false;

            IFocusNodeState State = StateView.State;
            IFocusBlockListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);

            Debug.Assert(StartIndex <= EndIndex);

            int OldBlockCount = ParentInner.BlockStateList.Count;
            int SelectionCount = EndIndex - StartIndex + 1;

            if (SelectionCount < ParentInner.BlockStateList.Count || !NodeHelper.IsCollectionNeverEmpty(State.Node, PropertyName))
            {
                List<IBlock> BlockList = new List<IBlock>();
                for (int i = StartIndex; i <= EndIndex; i++)
                {
                    IFocusBlockState BlockState = ParentInner.BlockStateList[i];
                    BlockList.Add(BlockState.ChildBlock);
                }

                ClipboardHelper.WriteBlockList(dataObject, BlockList);

                IFocusController Controller = StateView.ControllerView.Controller;
                Controller.RemoveBlockRange(ParentInner, StartIndex, EndIndex);

                Debug.Assert(ParentInner.BlockStateList.Count == OldBlockCount - SelectionCount);

                StateView.ControllerView.ClearSelection();
                isDeleted = true;
            }
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
            Debug.Assert(StartIndex <= EndIndex);

            int OldBlockCount = ParentInner.BlockStateList.Count;
            int SelectionCount = EndIndex - StartIndex + 1;

            if (ClipboardHelper.TryReadBlockList(out IList<IBlock> BlockList) && BlockList.Count > 0)
            {
                NodeTreeHelperBlockList.GetBlockType(BlockList[0], out Type ChildInterfaceType, out Type ChildItemType);

                if (ParentInner.InterfaceType.IsAssignableFrom(ChildInterfaceType))
                {
                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>();
                    IFocusController Controller = StateView.ControllerView.Controller;
                    int InsertionBlockIndex = StartIndex;

                    for (int i = 0; i < BlockList.Count; i++)
                    {
                        IBlock NewBlock = BlockList[i];

                        for (int j = 0; j < NewBlock.NodeList.Count; j++)
                        {
                            INode NewNode = NewBlock.NodeList[j] as INode;
                            IFocusInsertionBlockNodeIndex InsertedIndex;

                            if (j == 0)
                                InsertedIndex = CreateNewBlockNodeIndex(ParentInner.Owner.Node, PropertyName, NewNode, InsertionBlockIndex, NewBlock.ReplicationPattern, NewBlock.SourceIdentifier);
                            else
                                InsertedIndex = CreateExistingBlockNodeIndex(ParentInner.Owner.Node, PropertyName, NewNode, InsertionBlockIndex, j);

                            Debug.Assert(InsertedIndex != null);

                            IndexList.Add(InsertedIndex);
                        }

                        InsertionBlockIndex++;
                    }

                    Controller.ReplaceBlockRange(ParentInner, StartIndex, EndIndex, IndexList);

                    Debug.Assert(ParentInner.BlockStateList.Count == OldBlockCount + BlockList.Count - SelectionCount);

                    StateView.ControllerView.ClearSelection();
                    isChanged = true;
                }
            }
        }
#endif
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"From block {StartIndex} to {EndIndex}";
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListSelection));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, blockIndex, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListSelection));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
