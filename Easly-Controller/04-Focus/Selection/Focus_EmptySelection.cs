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

            if (ClipboardHelper.TryReadNode(out INode Node))
                PasteNode(Node, out isChanged);

            else if (ClipboardHelper.TryReadNodeList(out IList<INode> NodeList))
                PasteNodeList(NodeList, out isChanged);

            else if (ClipboardHelper.TryReadText(out string Text) && Text.Length > 0)
                PasteText(Text, out isChanged);
        }

        /// <summary></summary>
        protected virtual void PasteNode(INode node, out bool isChanged)
        {
            isChanged = false;

            IFocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusTextFocus AsTextFocus)
            {
                IFocusNodeState State = AsTextFocus.CellView.StateView.State;
                if (State.Node.GetType().IsAssignableFrom(node.GetType()))
                {
                    if (State.ParentIndex is IFocusBrowsingInsertableIndex AsInsertableIndex)
                    {
                        IFocusController Controller = StateView.ControllerView.Controller;
                        INode ParentNode = State.ParentInner.Owner.Node;

                        IFocusInsertionChildIndex ReplaceIndex = (IFocusInsertionChildIndex)AsInsertableIndex.ToInsertionIndex(ParentNode, node);
                        Controller.Replace(State.ParentInner, ReplaceIndex, out IWriteableBrowsingChildIndex NewIndex);

                        isChanged = true;
                    }
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteNodeList(IList<INode> nodeList, out bool isChanged)
        {
            isChanged = false;

            IFocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusTextFocus AsTextFocus)
            {
                IFocusNodeState State = AsTextFocus.CellView.StateView.State;
                while (State != null && !(State.ParentInner is IFocusCollectionInner))
                    State = State.ParentInner.Owner;

                if (State.ParentInner is IFocusListInner AsListInner && State.ParentIndex is IFocusBrowsingListNodeIndex AsListNodeIndex)
                    PasteNodeListToList(nodeList, AsListInner, AsListNodeIndex, out isChanged);
                else if (State.ParentInner is IFocusBlockListInner AsBlockListInner && State.ParentIndex is IFocusBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                    PasteNodeListToBlockList(nodeList, AsBlockListInner, AsExistingBlockNodeIndex, out isChanged);
            }
        }

        /// <summary></summary>
        protected virtual void PasteNodeListToList(IList<INode> nodeList, IFocusListInner listInner, IFocusBrowsingListNodeIndex parentIndex, out bool isChanged)
        {
            isChanged = false;

            if (nodeList.Count > 0)
            {
                if (listInner.InterfaceType.IsAssignableFrom(nodeList[0].GetType()))
                {
                    IFocusController Controller = StateView.ControllerView.Controller;
                    INode ParentNode = listInner.Owner.Node;

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        INode Node = nodeList[nodeList.Count - i - 1];
                        IFocusInsertionListNodeIndex ReplaceIndex = (IFocusInsertionListNodeIndex)parentIndex.ToInsertionIndex(ParentNode, Node);
                        Controller.Insert(listInner, ReplaceIndex, out IWriteableBrowsingCollectionNodeIndex NewIndex);
                    }

                    isChanged = true;
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteNodeListToBlockList(IList<INode> nodeList, IFocusBlockListInner blockListInner, IFocusBrowsingExistingBlockNodeIndex parentIndex, out bool isChanged)
        {
            isChanged = false;

            if (nodeList.Count > 0)
            {
                if (blockListInner.InterfaceType.IsAssignableFrom(nodeList[0].GetType()))
                {
                    IFocusController Controller = StateView.ControllerView.Controller;
                    INode ParentNode = blockListInner.Owner.Node;

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        INode Node = nodeList[nodeList.Count - i - 1];
                        IFocusInsertionExistingBlockNodeIndex ReplaceIndex = (IFocusInsertionExistingBlockNodeIndex)parentIndex.ToInsertionIndex(ParentNode, Node);
                        Controller.Insert(blockListInner, ReplaceIndex, out IWriteableBrowsingCollectionNodeIndex NewIndex);
                    }

                    isChanged = true;
                }
            }
        }

        /// <summary></summary>
        protected virtual void PasteText(string text, out bool isChanged)
        {
            isChanged = false;

            IFocusControllerView ControllerView = StateView.ControllerView;
            if (ControllerView.Focus is IFocusCommentFocus AsCommentFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                string Content = FocusedText.Substring(0, CaretPosition) + text + FocusedText.Substring(CaretPosition);

                IFocusController Controller = StateView.ControllerView.Controller;
                Controller.ChangeComment(AsCommentFocus.CellView.StateView.State.ParentIndex, Content);

                isChanged = true;
            }
            else if (ControllerView.Focus is IFocusStringContentFocus AsStringContentFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                string Content = FocusedText.Substring(0, CaretPosition) + text + FocusedText.Substring(CaretPosition);

                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;

                IFocusController Controller = StateView.ControllerView.Controller;
                Controller.ChangeText(CellView.StateView.State.ParentIndex, CellView.PropertyName, Content);

                isChanged = true;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Empty";
        }
        #endregion
    }
}
