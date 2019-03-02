namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// A selection of nodes in a list.
    /// </summary>
    public interface IFocusListNodeSelection : IFocusContentSelection
    {
        /// <summary>
        /// Index of the first selected node in the list.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Index of the last selected node in the list.
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
    public class FocusListNodeSelection : FocusSelection, IFocusListNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusListNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index of the last selected node in the list.</param>
        public FocusListNodeSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(Node, propertyName, out Type childNodeType));

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
        /// Index of the first selected node in the list.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Index of the last selected node in the list.
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
    }
}
