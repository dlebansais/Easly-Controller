namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

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
        /// <param name="propertyName">Name of the property pointing to the collection to check.</param>
        bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName);

        /// <summary>
        /// Checks if the optional node associated to <paramref name="propertyName"/> is assigned.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the node to check.</param>
        bool IsOptionalNodeAssigned(IFocusNodeStateView stateView, string propertyName);

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

        /// <summary>
        /// Checks if a new item can be inserted at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to insert the new item upon return.</param>
        /// <param name="index">Index of the new item to insert upon return.</param>
        /// <returns>True if a new item can be inserted at the focus.</returns>
        bool IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        bool IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        bool IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        bool IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsItemMoveable(int direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsBlockMoveable(int direction, out IFocusBlockListInner inner, out int blockIndex);

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        bool IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition);

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        bool IsItemSimplifiable(out IFocusInner inner, out IFocusInsertionChildIndex index);

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to insert upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        bool IsIdentifierSplittable(out IFocusListInner inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex);

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        bool IsReplicationModifiable(out IFocusBlockListInner inner, out int blockIndex, out ReplicationStatus replication);
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
        /// Initializes a new instance of the <see cref="FocusControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        private protected FocusControllerView(IFocusController controller, IFocusTemplateSet templateSet)
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
        /// <summary></summary>
        private protected IFocusFocusableCellViewList FocusChain { get; private set; }

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

                    return NodeTreeHelper.GetString(Node, PropertyName);
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        public bool IsUserVisible
        {
            get
            {
                IFocusNodeStateView StateView = GetFirstNonSimpleStateView(FocusedCellView.StateView);
                return StateView.IsUserVisible;
            }
        }
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
        /// <param name="propertyName">Name of the property pointing to the collection to check.</param>
        public virtual bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool HasItems = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusListInner AsListInner:
                    HasItems = AsListInner.Count > 0;
                    if (HasItems)
                    {
                        IFocusPlaceholderNodeState NodeState = AsListInner.StateList[0];
                        Debug.Assert(StateViewTable.ContainsKey(NodeState));
                        IFocusNodeStateView StateView = StateViewTable[NodeState];
                        IFocusTemplate Template = StateView.Template;
                    }

                    IsHandled = true;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    HasItems = AsBlockListInner.Count > 0;
                    if (HasItems)
                    {
                        IFocusBlockState BlockState = AsBlockListInner.BlockStateList[0];
                        Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
                        IFocusBlockStateView BlockStateView = BlockStateViewTable[BlockState];
                        IFocusTemplate Template = BlockStateView.Template;
                        IFocusCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
                    }

                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return HasItems;
        }

        /// <summary>
        /// Checks if the optional node associated to <paramref name="propertyName"/> is assigned.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the node to check.</param>
        public virtual bool IsOptionalNodeAssigned(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusOptionalInner OptionalInner = State.InnerTable[propertyName] as IFocusOptionalInner;
            Debug.Assert(OptionalInner != null);

            return OptionalInner.IsAssigned;
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

            bool IsHandled = false;
            bool Result = false;

            switch (State.ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.Boolean:
                case ValuePropertyType.Enum:
                    Result = NodeTreeHelper.GetEnumValue(State.Node, propertyName) == defaultValue;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return Result;
        }

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        public virtual bool IsFirstItem(IFocusNodeStateView stateView)
        {
            Debug.Assert(stateView != null);

            IFocusNodeState State = stateView.State;
            Debug.Assert(State != null);

            IFocusInner ParentInner = State.ParentInner;
            Debug.Assert(ParentInner != null);

            IFocusPlaceholderNodeState PlaceholderNodeState;
            IFocusPlaceholderNodeStateReadOnlyList StateList;
            int Index;
            bool Result;

            switch (ParentInner)
            {
                case IFocusListInner AsListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    StateList = AsListInner.StateList;
                    Index = StateList.IndexOf(PlaceholderNodeState);
                    Debug.Assert(Index >= 0 && Index < StateList.Count);
                    Result = Index == 0;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    Result = false;
                    for (int BlockIndex = 0; BlockIndex < AsBlockListInner.BlockStateList.Count; BlockIndex++)
                    {
                        StateList = AsBlockListInner.BlockStateList[BlockIndex].StateList;
                        Index = StateList.IndexOf(PlaceholderNodeState);
                        if (Index >= 0)
                        {
                            Debug.Assert(Index < StateList.Count);
                            Result = BlockIndex == 0 && Index == 0;
                        }
                    }
                    break;

                default:
                    Result = true;
                    break;
            }

            return Result;
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

            bool IsHandled = false;
            bool Result = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusPlaceholderInner AsPlaceholderInner:
                    Debug.Assert(AsPlaceholderInner.InterfaceType == typeof(IIdentifier));
                    IFocusPlaceholderNodeState ChildState = AsPlaceholderInner.ChildState as IFocusPlaceholderNodeState;
                    Debug.Assert(ChildState != null);
                    Result = NodeTreeHelper.GetString(ChildState.Node, nameof(IIdentifier.Text)) == textPattern;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            return Result;
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

            ResetCaretPosition();
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

                return SetTextCaretPosition(Node, PropertyName, position);
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

            IFocusNodeStateView StateView = GetFirstNonSimpleStateView(FocusedCellView.StateView);

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

        /// <summary></summary>
        private protected virtual IFocusNodeStateView GetFirstNonSimpleStateView(IFocusNodeStateView stateView)
        {
            Debug.Assert(stateView != null);
            IFocusNodeStateView CurrentStateView = stateView;

            while (CurrentStateView.Template.IsSimple && CurrentStateView.State.ParentState != null)
                CurrentStateView = StateViewTable[CurrentStateView.State.ParentState];

            return CurrentStateView;
        }

        /// <summary>
        /// Checks if a new item can be inserted at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to insert the new item upon return.</param>
        /// <param name="index">Index of the new item to insert upon return.</param>
        /// <returns>True if a new item can be inserted at the focus.</returns>
        public virtual bool IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;
            bool IsHandled = false;
            bool Result = false;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
            {
                Type InsertType = AsInsertFrame.InsertType;
                Debug.Assert(InsertType != null);

                INode NewItem = BuildNewInsertableItem(InsertType);

                IFocusCollectionInner CollectionInner = null;
                AsInsertFrame.CollectionNameToInner(ref State, ref CollectionInner);
                Debug.Assert(CollectionInner != null);

                if (CollectionInner is IFocusBlockListInner AsBlockListInner)
                {
                    inner = AsBlockListInner;

                    if (AsBlockListInner.Count == 0)
                    {
                        IPattern NewPattern = NodeHelper.CreateEmptyPattern();
                        IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
                        index = CreateNewBlockNodeIndex(State.Node, CollectionInner.PropertyName, NewItem, 0, NewPattern, NewSource);
                    }
                    else
                        index = CreateExistingBlockNodeIndex(State.Node, CollectionInner.PropertyName, NewItem, 0, 0);

                    Result = true;
                    IsHandled = true;
                }
                else if (CollectionInner is IFocusListInner AsListInner)
                {
                    inner = AsListInner;
                    index = CreateListNodeIndex(State.Node, AsListInner.PropertyName, NewItem, 0);

                    Result = true;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
            else if (FocusedCellView.Frame is IFocusTextValueFrame AsTextValueFrame)
            {
                if (CaretPosition == 0)
                    if (State.ParentInner is IFocusListInner AsListInner)
                    {
                        Type InsertType = NodeTreeHelper.InterfaceTypeToNodeType(AsListInner.InterfaceType);
                        INode NewItem = BuildNewInsertableItem(InsertType);

                        inner = AsListInner;
                        index = CreateListNodeIndex(inner.Owner.Node, inner.PropertyName, NewItem, 0);

                        Result = true;
                    }
            }

            return Result;
        }

        /// <summary></summary>
        private protected virtual INode BuildNewInsertableItem(Type insertType)
        {
            return NodeHelper.CreateEmptyNode(insertType);
        }

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        public virtual bool IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;
            bool IsRemoveable;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsRemoveable = false;
            else
            {
                IsRemoveable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusCollectionInner AsCollectionInner)
                    {
                        inner = AsCollectionInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsRemoveable(inner, index))
                            IsRemoveable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsRemoveable;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        public virtual bool IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;
            bool IsSplittable;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsSplittable = false;
            else
            {
                IsSplittable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsSplittable(inner, index))
                            IsSplittable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsSplittable;
        }

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        public virtual bool IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;
            bool IsMergeable;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsMergeable = false;
            else
            {
                IsMergeable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMergeable(inner, index))
                            IsMergeable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsMergeable;
        }

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsItemMoveable(int direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;
            bool IsMoveable;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsMoveable = false;
            else
            {
                IsMoveable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusCollectionInner AsCollectionInner)
                    {
                        inner = AsCollectionInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMoveable(inner, index, direction))
                            IsMoveable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsMoveable;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsBlockMoveable(int direction, out IFocusBlockListInner inner, out int blockIndex)
        {
            inner = null;
            blockIndex = -1;
            bool IsBlockMoveable;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsBlockMoveable = false;
            else
            {
                IsBlockMoveable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ParentIndex != null);
                        blockIndex = ParentIndex.BlockIndex;

                        if (Controller.IsBlockMoveable(inner, blockIndex, direction))
                            IsBlockMoveable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsBlockMoveable;
        }

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        public virtual bool IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition)
        {
            state = null;
            cyclePosition = -1;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState CurrentState = FocusedCellView.StateView.State;

            // Search recursively for a collection parent.
            while (CurrentState != null)
            {
                if (CurrentState is IFocusCyclableNodeState AsCyclableNodeState && Controller.IsMemberOfCycle(AsCyclableNodeState, out IFocusCycleManager CycleManager))
                {
                    CycleManager.AddNodeToCycle(AsCyclableNodeState);

                    IFocusInsertionChildNodeIndexList CycleIndexList = AsCyclableNodeState.CycleIndexList;
                    Debug.Assert(CycleIndexList.Count >= 2);
                    int CurrentPosition = AsCyclableNodeState.CycleCurrentPosition;
                    Debug.Assert(CurrentPosition >= 0 && CurrentPosition < CycleIndexList.Count);

                    state = AsCyclableNodeState;
                    cyclePosition = CurrentPosition;

                    return true;
                }

                CurrentState = CurrentState.ParentState;
            }

            return false;
        }

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        public virtual bool IsItemSimplifiable(out IFocusInner inner, out IFocusInsertionChildIndex index)
        {
            inner = null;
            index = null;
            bool IsSimplifiable = false;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState CurrentState = FocusedCellView.StateView.State;

            // Search recursively for a simplifiable node.
            while (CurrentState != null)
            {
                if (NodeHelper.GetSimplifiedNode(CurrentState.Node, out INode SimplifiedNode))
                {
                    if (SimplifiedNode != null)
                    {
                        Type InterfaceType = CurrentState.ParentInner.InterfaceType;
                        if (InterfaceType.IsAssignableFrom(SimplifiedNode.GetType()))
                        {
                            IFocusBrowsingChildIndex ParentIndex = CurrentState.ParentIndex as IFocusBrowsingChildIndex;
                            Debug.Assert(ParentIndex != null);

                            inner = CurrentState.ParentInner;
                            index = ((IFocusBrowsingInsertableIndex)ParentIndex).ToInsertionIndex(inner.Owner.Node, SimplifiedNode) as IFocusInsertionChildIndex;
                            IsSimplifiable = true;
                        }
                    }

                    break;
                }

                CurrentState = CurrentState.ParentState;
            }

            return IsSimplifiable;
        }

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to insert upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        public virtual bool IsIdentifierSplittable(out IFocusListInner inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex)
        {
            inner = null;
            replaceIndex = null;
            insertIndex = null;
            bool IsSplittable = false;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState IdentifierState = FocusedCellView.StateView.State;
            if (IdentifierState.Node is IIdentifier AsIdentifier)
            {
                IFocusNodeState ParentState = IdentifierState.ParentState;
                if (ParentState.Node is IQualifiedName)
                {
                    string Text = AsIdentifier.Text;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= Text.Length);

                    inner = IdentifierState.ParentInner as IFocusListInner;
                    Debug.Assert(inner != null);

                    IFocusBrowsingListNodeIndex CurrentIndex = IdentifierState.ParentIndex as IFocusBrowsingListNodeIndex;
                    Debug.Assert(CurrentIndex != null);

                    IIdentifier FirstPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(0, CaretPosition));
                    IIdentifier SecondPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(CaretPosition));

                    replaceIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, FirstPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(replaceIndex != null);

                    insertIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, SecondPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(insertIndex != null);

                    IsSplittable = true;
                }
            }

            return IsSplittable;
        }

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        public virtual bool IsReplicationModifiable(out IFocusBlockListInner inner, out int blockIndex, out ReplicationStatus replication)
        {
            inner = null;
            blockIndex = -1;
            replication = ReplicationStatus.Normal;
            bool IsModifiable = false;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            // Search recursively for a collection parent, up to 3 levels up.
            for (int i = 0; i < 3 && State != null; i++)
            {
                if (State is IFocusPatternState AsPatternState)
                {
                    IFocusBlockState ParentBlock = AsPatternState.ParentBlockState;
                    IFocusBlockListInner BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }
                else if (State is IFocusSourceState AsSourceState)
                {
                    IFocusBlockState ParentBlock = AsSourceState.ParentBlockState;
                    IFocusBlockListInner BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }
                else if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                {
                    inner = AsBlockListInner;
                    IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ParentIndex != null);
                    blockIndex = ParentIndex.BlockIndex;
                    replication = inner.BlockStateList[blockIndex].ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }

                State = State.ParentState;
            }

            return IsModifiable;
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            IFocusBlockState BlockState = ((IFocusInsertBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            IFocusPlaceholderNodeState ChildState = ((IFocusInsertBlockOperation)operation).ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == ((IFocusInsertBlockOperation)operation).BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            IFocusNodeState RemovedState = ((IFocusRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockViewOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IFocusNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            IFocusNodeState ChildState = ((IFocusInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFocusBrowsingCollectionNodeIndex BrowsingIndex = ((IFocusInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            IFocusPlaceholderNodeState RemovedState = ((IFocusRemoveNodeOperation)operation).RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            IFocusNodeState NewChildState = ((IFocusReplaceOperation)operation).NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            IFocusBrowsingChildIndex OldBrowsingIndex = ((IFocusReplaceOperation)operation).OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            IFocusBrowsingChildIndex NewBrowsingIndex = ((IFocusReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateChanged(IWriteableChangeNodeOperation operation)
        {
            base.OnStateChanged(operation);

            IFocusNodeState State = ((IFocusChangeNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            IFocusBlockState BlockState = ((IFocusChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            IFocusPlaceholderNodeState State = ((IFocusMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            IFocusBlockState BlockState = ((IFocusMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            IFocusBlockState BlockState = ((IFocusSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            IFocusBlockState BlockState = ((IFocusMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            IFocusNodeState RefreshState = ((IFocusGenericRefreshOperation)operation).RefreshState;
            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));
        }

        /// <summary></summary>
        private protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            IFocusCellViewTreeContext Context = (IFocusCellViewTreeContext)CreateCellViewTreeContext(stateView);

            IFocusNodeStateView CurrentStateView = (IFocusNodeStateView)stateView;
            List<IFocusFrameSelectorList> SelectorStack = new List<IFocusFrameSelectorList>();

            for (;;)
            {
                IFocusInner ParentInner = CurrentStateView.State.ParentInner;
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

        /// <summary></summary>
        private protected override void Refresh(IFrameNodeState state)
        {
            base.Refresh(state);

            UpdateFocusChain((IFocusNodeState)state);
        }

        /// <summary></summary>
        private protected virtual void UpdateFocusChain(IFocusNodeState state)
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
            else if (!NewFocusChain.Contains(FocusedCellView)) // If the focus may have forcibly changed.
                RecoverFocus(state, NewFocusChain);

            FocusChain = NewFocusChain;

            Debug.Assert(FocusedCellView != null);
            Debug.Assert(FocusChain.Contains(FocusedCellView));
        }

        /// <summary></summary>
        private protected virtual void RecoverFocus(IFocusNodeState state, IFocusFocusableCellViewList newFocusChain)
        {
            IFocusNodeState CurrentState = state;
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView = null;
            List<IFocusFocusableCellView> SameStateFocusableList = new List<IFocusFocusableCellView>();

            // Get the state that should have the focus and all its children.
            while (CurrentState != null && !GetFocusedStateAndChildren(newFocusChain, CurrentState, out MainStateView, out StateViewList, out SameStateFocusableList))
                CurrentState = CurrentState.ParentState;

            Debug.Assert(SameStateFocusableList.Count > 0);

            // Now that we have found candidates, try to select the original frame.
            IFocusFrame Frame = FocusedCellView.Frame;
            bool IsFrameChanged = true;
            foreach (IFocusFocusableCellView CellView in SameStateFocusableList)
                if (CellView.Frame == Frame)
                {
                    IsFrameChanged = false;
                    FocusedCellView = CellView;
                    break;
                }

            // If the frame has changed, use a preferred frame.
            if (IsFrameChanged)
                FindPreferredFrame(MainStateView, SameStateFocusableList);

            ResetCaretPosition();
        }

        /// <summary></summary>
        private protected virtual void FindPreferredFrame(IFocusNodeStateView mainStateView, List<IFocusFocusableCellView> sameStateFocusableList)
        {
            bool IsFrameSet = false;
            IFocusNodeTemplate Template = mainStateView.Template as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            Template.GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
            Debug.Assert(FirstPreferredFrame != null);
            Debug.Assert(LastPreferredFrame != null);

            foreach (IFocusFocusableCellView CellView in sameStateFocusableList)
                if (CellView.Frame == FirstPreferredFrame || CellView.Frame == LastPreferredFrame)
                {
                    IsFrameSet = true;
                    FocusedCellView = CellView;
                    break;
                }

            // If none of the preferred frames are visible, use the first focusable cell.
            if (!IsFrameSet)
                FocusedCellView = sameStateFocusableList[0];
        }

        /// <summary></summary>
        private protected virtual bool GetFocusedStateAndChildren(IFocusFocusableCellViewList newFocusChain, IFocusNodeState state, out IFocusNodeStateView mainStateView, out List<IFocusNodeStateView> stateViewList, out List<IFocusFocusableCellView> sameStateFocusableList)
        {
            mainStateView = StateViewTable[state];

            stateViewList = new List<IFocusNodeStateView>();
            sameStateFocusableList = new List<IFocusFocusableCellView>();
            GetChildrenStateView(mainStateView, stateViewList);

            // Find all focusable cells belonging to these states.
            foreach (IFocusFocusableCellView CellView in newFocusChain)
            {
                foreach (IFocusNodeStateView StateView in stateViewList)
                    if (CellView.StateView == StateView && !sameStateFocusableList.Contains(CellView))
                    {
                        sameStateFocusableList.Add(CellView);
                        break;
                    }
            }

            bool Found = sameStateFocusableList.Count > 0;

            // If it doesn't work, try the parent state, down to the root (in case of a removal or unassign).
            return Found;
        }

        /// <summary></summary>
        private protected virtual void GetChildrenStateView(IFocusNodeStateView stateView, List<IFocusNodeStateView> stateViewList)
        {
            stateViewList.Add(stateView);

            foreach (KeyValuePair<string, IFocusInner> Entry in stateView.State.InnerTable)
            {
                bool IsHandled = false;

                if (Entry.Value is IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    Debug.Assert(StateViewTable.ContainsKey(AsPlaceholderInner.ChildState));
                    IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                    GetChildrenStateView(ChildStateView, stateViewList);

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    if (AsOptionalInner.IsAssigned)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[AsOptionalInner.ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    foreach (IFocusNodeState ChildState in AsListInner.StateList)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    foreach (IFocusBlockState BlockState in AsBlockListInner.BlockStateList)
                    {
                        IFocusNodeStateView PatternStateView = StateViewTable[BlockState.PatternState];
                        GetChildrenStateView(PatternStateView, stateViewList);

                        IFocusNodeStateView SourceStateView = StateViewTable[BlockState.SourceState];
                        GetChildrenStateView(SourceStateView, stateViewList);

                        foreach (IFocusNodeState ChildState in BlockState.StateList)
                        {
                            IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                            GetChildrenStateView(ChildStateView, stateViewList);
                        }
                    }

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        /// <summary></summary>
        private protected virtual void ResetCaretPosition()
        {
            if (FocusedCellView is IFocusTextFocusableCellView AsText)
                CaretPosition = 0;
            else
                CaretPosition = -1;
        }

        /// <summary></summary>
        private protected virtual bool SetTextCaretPosition(INode node, string propertyName, int position)
        {
            string Text = NodeTreeHelper.GetString(node, propertyName);
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

        /// <summary></summary>
        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((IFocusBlockCellView)blockCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusBlockCellView)blockCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
        }

        /// <summary></summary>
        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((IFocusContainerCellView)containerCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ChildStateView == (IFocusNodeStateView)childStateView);
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

            if (!comparer.IsSameType(other, out FocusControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
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
        private protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPlaceholderNodeStateView(this, (IFocusPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusOptionalNodeStateView(this, (IFocusOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPatternStateView(this, (IFocusPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusSourceStateView(this, (IFocusSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateView(this, (IFocusBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        private protected override IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCellViewTreeContext(this, (IFocusNodeStateView)stateView);
        }

        /// <summary>
        /// Creates a IxxxFocusableCellViewList object.
        /// </summary>
        private protected virtual IFocusFocusableCellViewList CreateFocusChain()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusFocusableCellViewList();
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(INode parentNode, string propertyName, INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }
        #endregion
    }
}
