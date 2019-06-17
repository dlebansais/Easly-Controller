namespace EaslyController.Focus
{
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Writeable;

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public interface IFocusNodeSelection : IFocusSelection
    {
    }

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
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
#if !TRAVIS
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
            Debug.Assert(dataObject != null);

            CutOrDelete(dataObject, out isDeleted);
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public override void Paste(out bool isChanged)
        {
            isChanged = false;

            if (ClipboardHelper.TryReadNode(out INode Node))
            {
                IFocusNodeState State = StateView.State;
                if ((State.ParentInner != null && State.ParentInner.InterfaceType.IsAssignableFrom(Node.GetType())) || (State.ParentInner == null && Node.GetType() == State.Node.GetType()))
                {
                    if (State.ParentIndex is IFocusBrowsingInsertableIndex AsInsertableIndex)
                    {
                        IFocusController Controller = StateView.ControllerView.Controller;
                        INode ParentNode = State.ParentInner.Owner.Node;

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
                IFocusController Controller = StateView.ControllerView.Controller;
                INode ParentNode = State.ParentInner.Owner.Node;

                Controller.Remove(AsCollectionInner, AsCollectionNodeIndex);

                isDeleted = true;
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
            return StateView.State.ToString();
        }
        #endregion
    }
}
