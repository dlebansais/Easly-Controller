using BaseNode;
using BaseNodeHelper;
using EaslyController.Constants;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IFocusControllerView : IFrameControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new IFocusController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new IFocusStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new IFocusBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        new IFocusTemplateSet TemplateSet { get; }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        IFocusFocusableCellView FocusedCellView { get; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MinFocusMove { get; }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MaxFocusMove { get; }

        /// <summary>
        /// Position of the caret in a string property with the focus, -1 otherwise.
        /// </summary>
        int CaretPosition { get; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        CaretModes CaretMode { get; }

        /// <summary>
        /// Current text if the focus is on a string property. Null otherwise.
        /// </summary>
        string FocusedText { get; }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// Checks if the template associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is complex.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        bool IsTemplateComplex(IFocusNodeStateView stateView, string propertyName);

        /// <summary>
        /// Checks if the collection associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is has at least one item.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName);

        /// <summary>
        /// Checks if the enum or boolean associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has value <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="defaultValue">Expected default value.</param>
        bool DiscreteHasDefaultValue(IFocusNodeStateView stateView, string propertyName, int defaultValue);

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        bool IsFirstItem(IFocusNodeStateView stateView);

        /// <summary>
        /// Checks if the <paramref name="blockStateView"/> block state belongs to a replicated block.
        /// </summary>
        /// <param name="blockStateView">The block state view.</param>
        bool IsInReplicatedBlock(IFocusBlockStateView blockStateView);

        /// <summary>
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state matches the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern);

        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void MoveFocus(int direction);

        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <returns>True if the position was changed. False otherwise.</returns>
        bool SetCaretPosition(int position);

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        void SetCaretMode(CaretModes mode);

        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        void SetUserVisible(bool isUserVisible);
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class FocusControllerView : FrameControllerView, IFocusControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        public static IFocusControllerView Create(IFocusController controller, IFocusTemplateSet templateSet)
        {
            FocusControllerView View = new FocusControllerView(controller, templateSet);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="FocusControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        protected FocusControllerView(IFocusController controller, IFocusTemplateSet templateSet)
            : base(controller, templateSet)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new IFocusController Controller { get { return (IFocusController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new IFocusStateViewDictionary StateViewTable { get { return (IFocusStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new IFocusBlockStateViewDictionary BlockStateViewTable { get { return (IFocusBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public new IFocusTemplateSet TemplateSet { get { return (IFocusTemplateSet)base.TemplateSet; } }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        public IFocusFocusableCellView FocusedCellView { get; private set; }
        protected IFocusFocusableCellViewList FocusChain { get; private set; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MinFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(FocusedCellView);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return -FocusIndex;
            }
        }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MaxFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(FocusedCellView);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return FocusChain.Count - FocusIndex - 1;
            }
        }

        /// <summary>
        /// Position of the caret in a string property with the focus, -1 otherwise.
        /// </summary>
        public int CaretPosition { get; private set; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        public CaretModes CaretMode { get; private set; }

        /// <summary>
        /// Current text if the focus is on a string property. Null otherwise.
        /// </summary>
        public string FocusedText
        {
            get
            {
                if (FocusedCellView is IFocusTextFocusableCellView AsText)
                {
                    INode Node = AsText.StateView.State.Node;
                    string PropertyName = AsText.PropertyName;
                    Debug.Assert(PropertyName == "Text");

                    return NodeTreeHelper.GetText(Node);
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        public bool IsUserVisible { get { return FocusedCellView.StateView.IsUserVisible; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks if the template associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is complex.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        public virtual bool IsTemplateComplex(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusPlaceholderInner ParentInner = State.InnerTable[propertyName] as IFocusPlaceholderInner;
            Debug.Assert(ParentInner != null);

            NodeTreeHelperChild.GetChildNode(stateView.State.Node, propertyName, out INode ChildNode);
            Debug.Assert(ChildNode != null);

            Type NodeType = ChildNode.GetType();
            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            Debug.Assert(TemplateSet.NodeTemplateTable.ContainsKey(InterfaceType));

            IFocusNodeTemplate ParentTemplate = TemplateSet.NodeTemplateTable[InterfaceType] as IFocusNodeTemplate;
            Debug.Assert(ParentTemplate != null);

            return ParentTemplate.IsComplex;
        }

        /// <summary>
        /// Checks if the collection associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is has at least one item.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        public virtual bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            switch (State.InnerTable[propertyName])
            {
                case IFocusListInner AsListInner:
                    return AsListInner.Count > 0;

                case IFocusBlockListInner AsBlockListInner:
                    return AsBlockListInner.Count > 0;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
        }

        /// <summary>
        /// Checks if the enum or boolean associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has value <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="defaultValue">Expected default value.</param>
        public virtual bool DiscreteHasDefaultValue(IFocusNodeStateView stateView, string propertyName, int defaultValue)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));

            switch (State.ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.Boolean:
                case ValuePropertyType.Enum:
                    return NodeTreeHelper.GetEnumValue(State.Node, propertyName) == defaultValue;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
        }

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        public virtual bool IsFirstItem(IFocusNodeStateView stateView)
        {
            IFocusPlaceholderNodeState State = stateView.State as IFocusPlaceholderNodeState;
            Debug.Assert(State != null);
            Debug.Assert(State.ParentInner != null);

            IFocusPlaceholderNodeStateReadOnlyList StateList;
            int Index;
            switch (State.ParentInner)
            {
                case IFocusListInner AsListInner:
                    StateList = AsListInner.StateList;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    Debug.Assert(AsBlockListInner.BlockStateList.Count > 0);
                    StateList = AsBlockListInner.BlockStateList[0].StateList;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(stateView));
            }

            Index = StateList.IndexOf(State);
            Debug.Assert(Index >= 0 && Index < StateList.Count);

            return Index == 0;
        }

        /// <summary>
        /// Checks if the <paramref name="blockStateView"/> block state belongs to a replicated block.
        /// </summary>
        /// <param name="blockStateView">The block state view.</param>
        public virtual bool IsInReplicatedBlock(IFocusBlockStateView blockStateView)
        {
            IFocusBlockState BlockState = blockStateView.BlockState;
            Debug.Assert(BlockState != null);

            return BlockState.ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated;
        }

        /// <summary>
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state matches the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        public virtual bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            switch (State.InnerTable[propertyName])
            {
                case IFocusPlaceholderInner AsPlaceholderInner:
                    Debug.Assert(AsPlaceholderInner.InterfaceType == typeof(IIdentifier));
                    IFocusPlaceholderNodeState ChildState = AsPlaceholderInner.ChildState as IFocusPlaceholderNodeState;
                    Debug.Assert(ChildState != null);
                    return NodeTreeHelper.GetText(ChildState.Node) == textPattern;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
        }

        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void MoveFocus(int direction)
        {
            int FocusIndex = FocusChain.IndexOf(FocusedCellView);
            Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

            FocusIndex += direction;
            if (FocusIndex < 0)
                FocusIndex = 0;
            else if (FocusIndex >= FocusChain.Count)
                FocusIndex = FocusChain.Count - 1;

            Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);
            FocusedCellView = FocusChain[FocusIndex];
        }

        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <returns>True if the position was changed. False otherwise.</returns>
        public virtual bool SetCaretPosition(int position)
        {
            if (FocusedCellView is IFocusTextFocusableCellView AsText)
            {
                INode Node = AsText.StateView.State.Node;
                string PropertyName = AsText.PropertyName;
                Debug.Assert(PropertyName == "Text");

                return SetTextCaretPosition(Node, position);
            }

            return false;
        }

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        public virtual void SetCaretMode(CaretModes mode)
        {
            CaretMode = mode;
        }

        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        public virtual void SetUserVisible(bool isUserVisible)
        {
            Debug.Assert(FocusedCellView != null);

            IFocusNodeStateView StateView = FocusedCellView.StateView;
            Debug.Assert(StateView != null);

            if (isUserVisible)
            {
                foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in StateViewTable)
                    Entry.Value.SetIsUserVisible(false);

                StateView.SetIsUserVisible(isUserVisible: true);
            }
            else
                StateView.SetIsUserVisible(isUserVisible: false);

            Refresh(StateView.State);
        }
        #endregion

        #region Implementation
        protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            IFocusCellViewTreeContext Context = (IFocusCellViewTreeContext)CreateCellViewTreeContext(stateView);

            IFocusNodeStateView CurrentStateView = (IFocusNodeStateView)stateView;
            List<IFocusFrameSelectorList> SelectorStack = new List<IFocusFrameSelectorList>();

            for(;;)
            {
                IFocusInner<IFocusBrowsingChildIndex> ParentInner = CurrentStateView.State.ParentInner;
                IFocusNodeState ParentState = CurrentStateView.State.ParentState;
                if (ParentInner == null)
                {
                    Debug.Assert(ParentState == null);
                    break;
                }

                Debug.Assert(ParentState != null);

                string PropertyName = ParentInner.PropertyName;

                CurrentStateView = StateViewTable[ParentState];
                IFocusNodeTemplate Template = CurrentStateView.Template as IFocusNodeTemplate;
                Debug.Assert(Template != null);

                if (Template.FrameSelectorForProperty(PropertyName, out IFocusNodeFrameWithSelector Frame))
                    if (Frame != null)
                        if (Frame.Selectors.Count > 0)
                            SelectorStack.Insert(0, Frame.Selectors);
            }

            foreach (IFocusFrameSelectorList Selectors in SelectorStack)
                Context.AddSelectors(Selectors);

            return Context;
        }

        protected override void Refresh(IFrameNodeState state)
        {
            base.Refresh(state);

            UpdateFocusChain(state);
        }

        protected virtual void UpdateFocusChain(IFrameNodeState state)
        {
            IFocusFocusableCellViewList NewFocusChain = CreateFocusChain();
            IFocusNodeState RootState = Controller.RootState;
            IFocusNodeStateView RootStateView = StateViewTable[RootState];

            RootStateView.UpdateFocusChain(NewFocusChain);

            // Ensured by all templates having at least one preferred (hence focusable) frame.
            Debug.Assert(NewFocusChain.Count > 0);

            // First run, initialize the focus to the first focusable cell.
            if (FocusedCellView == null)
            {
                Debug.Assert(FocusChain == null);
                FocusedCellView = NewFocusChain[0];
                ResetCaretPosition();
            }

            // If the focus may have forcibly changed.
            else if (!NewFocusChain.Contains(FocusedCellView))
            {
                IFocusNodeStateView StateView = FocusedCellView.StateView;
                IFocusFrame Frame = FocusedCellView.Frame;

                // Find all focusable cells belonging to the old state.
                List<IFocusFocusableCellView> SameStateViewList = new List<IFocusFocusableCellView>();
                foreach (IFocusFocusableCellView CellView in NewFocusChain)
                    if (CellView.StateView == StateView)
                        SameStateViewList.Add(CellView);

                // If none, try with the new state or its children (only children might have focusable cells), or its parent (in case of a removal or unassign).
                if (SameStateViewList.Count == 0)
                {
                    IFocusNodeState CurrentState = (IFocusNodeState)state;
                    StateView = null;

                    while (CurrentState != null && !RecursiveFocusableCellViewSearch(NewFocusChain, StateViewTable[CurrentState], SameStateViewList, out StateView))
                        CurrentState = CurrentState.ParentState;

                    Debug.Assert(SameStateViewList.Count > 0);
                    Debug.Assert(StateView != null);
                }

                // Now that we have found candidates, try to select the original frame.
                bool IsFrameChanged = true;
                foreach (IFocusFocusableCellView CellView in SameStateViewList)
                    if (CellView.Frame == Frame)
                    {
                        IsFrameChanged = false;
                        FocusedCellView = CellView;
                        break;
                    }

                // If the frame has changed, use a preferred frame.
                if (IsFrameChanged)
                {
                    bool IsFrameSet = false;
                    IFocusNodeTemplate Template = StateView.Template as IFocusNodeTemplate;
                    Debug.Assert(Template != null);

                    Template.GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
                    Debug.Assert(FirstPreferredFrame != null);
                    Debug.Assert(LastPreferredFrame != null);

                    foreach (IFocusFocusableCellView CellView in SameStateViewList)
                        if (CellView.Frame == FirstPreferredFrame || CellView.Frame == LastPreferredFrame)
                        {
                            IsFrameSet = true;
                            FocusedCellView = CellView;
                            break;
                        }

                    // If none of the preferred frames are visible, use the first focusable cell.
                    if (!IsFrameSet)
                        FocusedCellView = SameStateViewList[0];
                }

                ResetCaretPosition();
            }

            FocusChain = NewFocusChain;

            Debug.Assert(FocusedCellView != null);
            Debug.Assert(FocusChain.Contains(FocusedCellView));
        }

        protected virtual bool RecursiveFocusableCellViewSearch(IFocusFocusableCellViewList focusChain, IFocusNodeStateView stateView, List<IFocusFocusableCellView> cellViewList, out IFocusNodeStateView selectedStateView)
        {
            foreach (IFocusFocusableCellView CellView in focusChain)
                if (CellView.StateView == stateView)
                    cellViewList.Add(CellView);

            if (cellViewList.Count > 0)
            {
                selectedStateView = stateView;
                return true;
            }

            foreach (KeyValuePair<string, IFocusInner<IFocusBrowsingChildIndex>> Entry in stateView.State.InnerTable)
                if (Entry.Value is IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                    if (RecursiveFocusableCellViewSearch(focusChain, ChildStateView, cellViewList, out selectedStateView))
                        return true;
                }
                else if (Entry.Value is IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    if (AsOptionalInner.IsAssigned)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[AsOptionalInner.ChildState];
                        if (RecursiveFocusableCellViewSearch(focusChain, ChildStateView, cellViewList, out selectedStateView))
                            return true;
                    }
                }
                else if (Entry.Value is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    foreach (IFocusNodeState ChildState in AsListInner.StateList)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                        if (RecursiveFocusableCellViewSearch(focusChain, ChildStateView, cellViewList, out selectedStateView))
                            return true;
                    }
                }
                else if (Entry.Value is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    foreach (IFocusBlockState BlockState in AsBlockListInner.BlockStateList)
                    {
                        IFocusNodeStateView PatternStateView = StateViewTable[BlockState.PatternState];
                        if (RecursiveFocusableCellViewSearch(focusChain, PatternStateView, cellViewList, out selectedStateView))
                            return true;

                        IFocusNodeStateView SourceStateView = StateViewTable[BlockState.SourceState];
                        if (RecursiveFocusableCellViewSearch(focusChain, SourceStateView, cellViewList, out selectedStateView))
                            return true;

                        foreach (IFocusNodeState ChildState in BlockState.StateList)
                        {
                            IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                            if (RecursiveFocusableCellViewSearch(focusChain, ChildStateView, cellViewList, out selectedStateView))
                                return true;
                        }
                    }
                }
                else
                    Debug.Assert(false);

            selectedStateView = null;
            return false;
        }

        protected virtual void ResetCaretPosition()
        {
            if (FocusedCellView is IFocusTextFocusableCellView AsText)
                CaretPosition = 0;
            else
                CaretPosition = -1;
        }

        protected virtual bool SetTextCaretPosition(INode node, int position)
        {
            string Text = NodeTreeHelper.GetText(node);
            int OldPosition = CaretPosition;

            if (position < 0)
                CaretPosition = 0;
            else if (position >= Text.Length)
                CaretPosition = Text.Length;
            else
                CaretPosition = position;

            if (CaretPosition != OldPosition)
                return true;
            else
                return false;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusControllerView AsControllerView))
                return false;

            if (!base.IsEqual(comparer, AsControllerView))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                NodeStateDetachedHandler = OnNodeStateRemoved,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockListInnerDetachedHandler = OnBlockListInnerRemoved,
                BlockStateAttachedHandler = OnBlockStateCreated,
                BlockStateDetachedHandler = OnBlockStateRemoved,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPlaceholderNodeStateView(this, (IFocusPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusOptionalNodeStateView(this, (IFocusOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPatternStateView(this, (IFocusPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusSourceStateView(this, (IFocusSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateView(this, (IFocusBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        protected override IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCellViewTreeContext(this, (IFocusNodeStateView)stateView);
        }

        /// <summary>
        /// Creates a IxxxFocusableCellViewList object.
        /// </summary>
        protected virtual IFocusFocusableCellViewList CreateFocusChain()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusFocusableCellViewList();
        }
        #endregion
    }
}
