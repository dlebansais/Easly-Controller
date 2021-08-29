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
    /// A selection of nodes in a list.
    /// </summary>
    public interface IFocusNodeListSelection : IFocusContentSelection
    {
        /// <summary>
        /// Index of the first selected node in the list.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Index following the last selected node in the list.
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
    /// A selection of nodes in a list.
    /// </summary>
    public class FocusNodeListSelection : FocusSelection, IFocusNodeListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index following the last selected node in the list.</param>
        public FocusNodeListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView)
        {
            Node Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(Node, propertyName, out Type childNodeType));

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
        /// Index of the first selected node in the list.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Index following the last selected node in the list.
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
            IFocusListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusListInner;
            Debug.Assert(ParentInner != null);

            List<Node> NodeList = new List<Node>();
            for (int i = StartIndex; i < EndIndex; i++)
                NodeList.Add(ParentInner.StateList[i].Node);

            ClipboardHelper.WriteNodeList(dataObject, NodeList);
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

            IFocusNodeState State = StateView.State;
            IFocusListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusListInner;
            Debug.Assert(ParentInner != null);

            Debug.Assert(StartIndex < ParentInner.StateList.Count);
            Debug.Assert(EndIndex <= ParentInner.StateList.Count);
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
                if (NodeList.Count == 0 || ParentInner.InterfaceType.IsAssignableFrom(NodeList[0].GetType()))
                {
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>();
                    IFocusController Controller = StateView.ControllerView.Controller;
                    int OldNodeCount = ParentInner.Count;
                    int SelectionCount = EndIndex - StartIndex;
                    int InsertionNodeIndex = EndIndex;

                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        Node NewNode = NodeList[i] as Node;
                        IFocusInsertionListNodeIndex InsertedIndex = CreateListNodeIndex(ParentInner.Owner.Node, PropertyName, NewNode, StartIndex + i);
                        IndexList.Add(InsertedIndex);
                    }

                    Controller.ReplaceNodeRange(ParentInner, -1, StartIndex, EndIndex, IndexList);

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
            IFocusListInner ParentInner = State.PropertyToInner(PropertyName) as IFocusListInner;
            Debug.Assert(ParentInner != null);

            int OldNodeCount = ParentInner.Count;
            int SelectionCount = EndIndex - StartIndex;

            if (SelectionCount < ParentInner.StateList.Count || !NodeHelper.IsCollectionNeverEmpty(State.Node, PropertyName))
            {
                if (dataObject != null)
                {
                    List<Node> NodeList = new List<Node>();
                    for (int i = StartIndex; i < EndIndex; i++)
                        NodeList.Add(ParentInner.StateList[i].Node);

                    ClipboardHelper.WriteNodeList(dataObject, NodeList);
                }

                IFocusController Controller = StateView.ControllerView.Controller;
                Controller.RemoveNodeRange(ParentInner, -1, StartIndex, EndIndex);

                Debug.Assert(ParentInner.Count == OldNodeCount - SelectionCount);

                StateView.ControllerView.ClearSelection();
                isDeleted = true;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"From node {StartIndex} to {EndIndex}";
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(Node parentNode, string propertyName, Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNodeListSelection));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }
        #endregion
    }
}
