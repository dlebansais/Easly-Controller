namespace EaslyController.Focus
{
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using Contracts;
    using EaslyController.Controller;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public interface IFocusNodeSelection : IFocusSelection
    {
    }

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    [DebuggerDisplay("{StateView.State}")]
    public class FocusNodeSelection : FocusSelection, IFocusNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public FocusNodeSelection(IFocusNodeStateView stateView)
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
            ClipboardHelper.WriteNode(dataObject, StateView.State.Node);
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

            if (ClipboardHelper.TryReadNode(out Node Node))
            {
                IFocusNodeState State = StateView.State;
                if ((State.ParentInner != null && State.ParentInner.InterfaceType.IsAssignableFrom(Type.FromGetType(Node))) || (State.ParentInner == null && Type.FromGetType(Node) == Type.FromGetType(State.Node)))
                {
                    if (State.ParentIndex is IFocusBrowsingInsertableIndex AsInsertableIndex)
                    {
                        FocusController Controller = StateView.ControllerView.Controller;
                        Node ParentNode = State.ParentInner.Owner.Node;

                        IFocusInsertionChildIndex ReplaceIndex = (IFocusInsertionChildIndex)AsInsertableIndex.ToInsertionIndex(ParentNode, Node);
                        Controller.Replace(State.ParentInner, ReplaceIndex, out IWriteableBrowsingChildIndex NewIndex);

                        isChanged = true;
                    }
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
            if (State.ParentInner is IFocusCollectionInner AsCollectionInner && State.ParentIndex is IFocusBrowsingCollectionNodeIndex AsCollectionNodeIndex)
            {
                FocusController Controller = StateView.ControllerView.Controller;
                Node ParentNode = State.ParentInner.Owner.Node;

                Controller.Remove(AsCollectionInner, AsCollectionNodeIndex);

                isDeleted = true;
            }
        }
        #endregion
    }
}
