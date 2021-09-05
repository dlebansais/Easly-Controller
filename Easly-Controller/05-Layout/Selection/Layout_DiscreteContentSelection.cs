namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNodeHelper;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public interface ILayoutDiscreteContentSelection : IFocusDiscreteContentSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public class LayoutDiscreteContentSelection : FocusDiscreteContentSelection, ILayoutDiscreteContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDiscreteContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        public LayoutDiscreteContentSelection(ILayoutNodeStateView stateView, string propertyName)
            : base(stateView, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints the selection.
        /// </summary>
        public virtual void Print()
        {
            LayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

            ILayoutTemplateSet TemplateSet = ControllerView.TemplateSet;
            IList<FocusFrameSelectorList> SelectorStack = StateView.GetSelectorStack();
            ILayoutDiscreteFrame Frame = (ILayoutDiscreteFrame)TemplateSet.PropertyToFrame(StateView.State, PropertyName, SelectorStack);
            Debug.Assert(Frame != null);

            int Value = NodeTreeHelper.GetEnumValue(StateView.State.Node, PropertyName);
            Frame.Print(ControllerView.PrintContext, Value, Point.Origin);
        }
        #endregion
    }
}
