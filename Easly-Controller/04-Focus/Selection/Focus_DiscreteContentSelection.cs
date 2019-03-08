namespace EaslyController.Focus
{
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public interface IFocusDiscreteContentSelection : IFocusContentSelection
    {
    }

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public class FocusDiscreteContentSelection : FocusSelection, IFocusDiscreteContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusDiscreteContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        public FocusDiscreteContentSelection(IFocusNodeStateView stateView, string propertyName)
            : base(stateView)
        {
            INode Node = stateView.State.Node;
            Debug.Assert(NodeTreeHelper.IsEnumProperty(Node, propertyName) || NodeTreeHelper.IsBooleanProperty(Node, propertyName));

            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
            int Content = NodeTreeHelper.GetEnumValue(StateView.State.Node, PropertyName);
            dataObject.SetData(typeof(int), Content);
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
            return $"Discrete '{PropertyName}'";
        }
        #endregion
    }
}
