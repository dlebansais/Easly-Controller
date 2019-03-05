namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// A selection of blocks in a block list.
    /// </summary>
    public interface IFocusBlockSelection : IFocusContentSelection
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
    public class FocusBlockSelection : FocusSelection, IFocusBlockSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected block.</param>
        /// <param name="endIndex">Index of the last selected block.</param>
        public FocusBlockSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(Node, propertyName, out Type childInterfaceType, out Type childNodeType));

            PropertyName = propertyName;
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
            StartIndex = startIndex;
            EndIndex = endIndex;
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
