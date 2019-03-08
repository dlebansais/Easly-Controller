namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNodeHelper;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of part of a string property.
    /// </summary>
    public interface ILayoutStringContentSelection : IFocusStringContentSelection, ILayoutContentSelection, ILayoutTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class LayoutStringContentSelection : FocusStringContentSelection, ILayoutStringContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutStringContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public LayoutStringContentSelection(ILayoutNodeStateView stateView, string propertyName, int start, int end)
            : base(stateView, propertyName, start, end)
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
            ILayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

            ILayoutTemplateSet TemplateSet = ControllerView.TemplateSet;
            IList<IFocusFrameSelectorList> SelectorStack = StateView.GetSelectorStack();
            ILayoutTextValueFrame Frame = (ILayoutTextValueFrame)TemplateSet.PropertyToFrame(StateView.State, PropertyName, SelectorStack);
            Debug.Assert(Frame != null);

            string Text = NodeTreeHelper.GetString(StateView.State.Node, PropertyName);
            Debug.Assert(Text != null);

            Debug.Assert(Start <= End);
            Debug.Assert(End <= Text.Length);

            Frame.Print(ControllerView.PrintContext, Text.Substring(Start, End - Start), Point.Origin);
        }
        #endregion
    }
}
