namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Controller;

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
                BlockList.Add(ParentInner.BlockStateList[i].ChildBlock);

            ClipboardHelper.WriteBlockList(dataObject, BlockList);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        public override void Cut()
        {
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        public override void Paste()
        {
        }
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
    }
}
