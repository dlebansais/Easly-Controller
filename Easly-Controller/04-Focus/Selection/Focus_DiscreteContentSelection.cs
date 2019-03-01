namespace EaslyController.Focus
{
    using System.Diagnostics;
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
    }
}
