namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Writeable;

    /// <summary>
    /// An empty selection.
    /// </summary>
    public interface IFocusEmptySelection : IFocusSelection
    {
    }

    /// <summary>
    /// An empty selection.
    /// </summary>
    [DebuggerDisplay("Empty")]
    public class FocusEmptySelection : FocusSelection, IFocusEmptySelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusEmptySelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public FocusEmptySelection(IFocusNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Cut(IDataObject dataObject, out bool isDeleted)
        {
            isDeleted = false;
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public override void Paste(out bool isChanged)
        {
            isChanged = false;

            if (ClipboardHelper.TryReadNode(out Node Node))
                PasteNode(Node, out isChanged);
            else if (ClipboardHelper.TryReadNodeList(out IList<Node> NodeList))
                PasteNodeList(NodeList, out isChanged);
            else if (ClipboardHelper.TryReadBlockList(out IList<IBlock> BlockList))
                PasteBlockList(BlockList, out isChanged);
            else if (ClipboardHelper.TryReadText(out string Text) && Text.Length > 0)
                PasteText(Text, out isChanged);
        }

        /// <summary></summary>
        protected virtual void PasteNode(Node node, out bool isChanged)
        {
            isChanged = false;

            FocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusTextFocus AsTextFocus)
            {
                IFocusNodeState State = AsTextFocus.CellView.StateView.State;
                if (State.Node.GetType().IsAssignableFrom(node.GetType()))
                {
                    if (State.ParentIndex is IFocusBrowsingInsertableIndex AsInsertableIndex)
                    {
                        FocusController Controller = StateView.ControllerView.Controller;
                        Node ParentNode = State.ParentInner.Owner.Node;

                        IFocusInsertionChildIndex ReplaceIndex = (IFocusInsertionChildIndex)AsInsertableIndex.ToInsertionIndex(ParentNode, node);
                        Controller.Replace(State.ParentInner, ReplaceIndex, out IWriteableBrowsingChildIndex NewIndex);

                        isChanged = true;
                    }
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteNodeList(IList<Node> nodeList, out bool isChanged)
        {
            isChanged = false;

            FocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusTextFocus AsTextFocus)
            {
                IFocusNodeState State = AsTextFocus.CellView.StateView.State;
                IFocusNodeState ParentState = State;
                bool IsAssignable = false;

                while (!IsAssignable && ParentState != null)
                {
                    State = ParentState;

                    if (State.ParentInner is IFocusCollectionInner AsCollectionInner && (nodeList.Count == 0 || AsCollectionInner.InterfaceType.IsAssignableFrom(nodeList[0].GetType())))
                        IsAssignable = true;

                    ParentState = State.ParentState;
                }

                if (IsAssignable)
                {
                    if (State.ParentInner is IFocusListInner AsListInner && State.ParentIndex is IFocusBrowsingListNodeIndex AsListNodeIndex)
                        PasteNodeListToList(nodeList, AsListInner, AsListNodeIndex, out isChanged);
                    else if (State.ParentInner is IFocusBlockListInner AsBlockListInner && State.ParentIndex is IFocusBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                        PasteNodeListToBlockList(nodeList, AsBlockListInner, AsExistingBlockNodeIndex, out isChanged);
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteNodeListToList(IList<Node> nodeList, IFocusListInner listInner, IFocusBrowsingListNodeIndex parentIndex, out bool isChanged)
        {
            isChanged = false;

            Debug.Assert(nodeList.Count == 0 || listInner.InterfaceType.IsAssignableFrom(nodeList[0].GetType()));

            List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>();
            FocusController Controller = StateView.ControllerView.Controller;

            for (int i = 0; i < nodeList.Count; i++)
            {
                Node NewNode = nodeList[i] as Node;
                IFocusInsertionListNodeIndex InsertedIndex = CreateListNodeIndex(listInner.Owner.Node, listInner.PropertyName, NewNode, parentIndex.Index + i);
                IndexList.Add(InsertedIndex);
            }

            Controller.InsertNodeRange(listInner, -1, parentIndex.Index, IndexList);

            isChanged = nodeList.Count > 0;
        }

        /// <summary></summary>
        protected virtual void PasteNodeListToBlockList(IList<Node> nodeList, IFocusBlockListInner blockListInner, IFocusBrowsingExistingBlockNodeIndex parentIndex, out bool isChanged)
        {
            isChanged = false;

            Debug.Assert(nodeList.Count == 0 || blockListInner.InterfaceType.IsAssignableFrom(nodeList[0].GetType()));

            List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>();
            FocusController Controller = StateView.ControllerView.Controller;

            for (int i = 0; i < nodeList.Count; i++)
            {
                Node NewNode = nodeList[i] as Node;
                IFocusInsertionExistingBlockNodeIndex InsertedIndex = CreateExistingBlockNodeIndex(blockListInner.Owner.Node, blockListInner.PropertyName, NewNode, parentIndex.BlockIndex, parentIndex.Index + i);
                IndexList.Add(InsertedIndex);
            }

            Controller.InsertNodeRange(blockListInner, parentIndex.BlockIndex, parentIndex.Index, IndexList);

            isChanged = nodeList.Count > 0;
        }

        /// <summary></summary>
        protected virtual void PasteBlockList(IList<IBlock> blockList, out bool isChanged)
        {
            isChanged = false;

            FocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusTextFocus AsTextFocus)
            {
                IFocusNodeState State = AsTextFocus.CellView.StateView.State;
                IFocusNodeState ParentState = State;
                bool IsAssignable = false;

                while (!IsAssignable && ParentState != null)
                {
                    State = ParentState;

                    if (State.ParentInner is IFocusBlockListInner AsCollectionInner && (blockList.Count == 0 || AsCollectionInner.InterfaceType.IsAssignableFrom(blockList[0].NodeList[0].GetType())))
                        IsAssignable = true;

                    ParentState = State.ParentState;
                }

                if (IsAssignable && State.ParentInner is IFocusBlockListInner AsBlockListInner && State.ParentIndex is IFocusBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    Debug.Assert(blockList.Count == 0 || AsBlockListInner.InterfaceType.IsAssignableFrom(blockList[0].NodeList[0].GetType()));

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>();
                    FocusController Controller = StateView.ControllerView.Controller;

                    int InsertionBlockIndex = AsExistingBlockNodeIndex.BlockIndex;

                    for (int i = 0; i < blockList.Count; i++)
                    {
                        IBlock NewBlock = blockList[i] as IBlock;

                        for (int j = 0; j < NewBlock.NodeList.Count; j++)
                        {
                            Node NewNode = NewBlock.NodeList[j] as Node;
                            IFocusInsertionBlockNodeIndex InsertedIndex;

                            if (j == 0)
                                InsertedIndex = CreateNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertionBlockIndex, NewBlock.ReplicationPattern, NewBlock.SourceIdentifier);
                            else
                                InsertedIndex = CreateExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertionBlockIndex, j);

                            Debug.Assert(InsertedIndex != null);

                            IndexList.Add(InsertedIndex);
                        }

                        InsertionBlockIndex++;
                    }

                    Controller.InsertBlockRange(AsBlockListInner, AsExistingBlockNodeIndex.BlockIndex, IndexList);

                    isChanged = blockList.Count > 0;
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteText(string text, out bool isChanged)
        {
            isChanged = false;

            FocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusCommentFocus AsCommentFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                string Content = FocusedText.Substring(0, CaretPosition) + text + FocusedText.Substring(CaretPosition);

                FocusController Controller = StateView.ControllerView.Controller;
                int OldCaretPosition = StateView.ControllerView.CaretPosition;
                int NewCaretPosition = CaretPosition + text.Length;
                Controller.ChangeCommentAndCaretPosition(AsCommentFocus.CellView.StateView.State.ParentIndex, Content, OldCaretPosition, NewCaretPosition, false);

                isChanged = true;
            }
            else if (ControllerView.Focus is IFocusStringContentFocus AsStringContentFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                string Content = FocusedText.Substring(0, CaretPosition) + text + FocusedText.Substring(CaretPosition);

                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;

                FocusController Controller = StateView.ControllerView.Controller;
                int OldCaretPosition = StateView.ControllerView.CaretPosition;
                int NewCaretPosition = CaretPosition + text.Length;
                Controller.ChangeTextAndCaretPosition(CellView.StateView.State.ParentIndex, CellView.PropertyName, Content, OldCaretPosition, NewCaretPosition, false);

                isChanged = true;
            }
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Delete(out bool isDeleted)
        {
            isDeleted = false;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(Node parentNode, string propertyName, Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusEmptySelection));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusEmptySelection));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, blockIndex, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusEmptySelection));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
