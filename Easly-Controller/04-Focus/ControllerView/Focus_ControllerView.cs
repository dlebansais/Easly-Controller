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
        bool IsNewItemInsertable(out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusInsertionCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        bool IsItemRemoveable(out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        bool IsItemSplittable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        bool IsItemMergeable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsItemMoveable(int direction, out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsBlockMoveable(int direction, out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out int blockIndex);

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        bool IsItemCyclableThrough(out IFocusNodeState state, out int cyclePosition);

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        bool IsItemSimplifiable(out IFocusInner<IFocusBrowsingChildIndex> inner, out IFocusInsertionChildIndex index);

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to replace upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        bool IsIdentifierSplittable(out IFocusListInner<IFocusBrowsingListNodeIndex> inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex);

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        bool IsReplicationModifiable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out int blockIndex, out ReplicationStatus replication);
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
        /// <summary></summary>
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
                    Index = StateList.IndexOf(State);
                    Debug.Assert(Index >= 0 && Index < StateList.Count);
                    return Index == 0;

                case IFocusBlockListInner AsBlockListInner:
                    for (int BlockIndex = 0; BlockIndex < AsBlockListInner.BlockStateList.Count; BlockIndex++)
                    {
                        StateList = AsBlockListInner.BlockStateList[BlockIndex].StateList;
                        Index = StateList.IndexOf(State);
                        if (Index >= 0)
                        {
                            Debug.Assert(Index < StateList.Count);
                            return BlockIndex == 0 && Index == 0;
                        }
                    }
                    throw new ArgumentOutOfRangeException(nameof(stateView));

                default:
                    throw new ArgumentOutOfRangeException(nameof(stateView));
            }
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
        protected virtual IFocusNodeStateView GetFirstNonSimpleStateView(IFocusNodeStateView stateView)
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
        public virtual bool IsNewItemInsertable(out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
            {
                INode NewItem = BuildNewInsertableItem(AsInsertFrame.InsertType);

                IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> CollectionInner = null;
                AsInsertFrame.CollectionNameToInner(ref State, ref CollectionInner);
                Debug.Assert(CollectionInner != null);

                if (CollectionInner is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
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

                    return true;
                }

                else if (CollectionInner is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    inner = AsListInner;
                    index = CreateListNodeIndex(State.Node, AsListInner.PropertyName, NewItem, 0);

                    return true;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(CollectionInner));
            }

            else if ((FocusedCellView.Frame is IFocusTextValueFrame AsTextValueFrame) && CaretPosition == 0)
            {
                if (State.ParentInner is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    Type InsertType = NodeTreeHelper.InterfaceTypeToNodeType(AsListInner.InterfaceType);
                    INode NewItem = BuildNewInsertableItem(InsertType);

                    inner = AsListInner;
                    index = CreateListNodeIndex(inner.Owner.Node, inner.PropertyName, NewItem, 0);

                    return true;
                }
            }

            return false;
        }

        /// <summary></summary>
        protected virtual INode BuildNewInsertableItem(Type insertType)
        {
            return NodeHelper.CreateEmptyNode(insertType);
        }

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        public virtual bool IsItemRemoveable(out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                return false;

            else
            {
                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> AsCollectionInner)
                    {
                        inner = AsCollectionInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsRemoveable(inner, index))
                            return true;
                        else
                            return false;
                    }

                    State = State.ParentState;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        public virtual bool IsItemSplittable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                return false;

            else
            {
                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsSplittable(inner, index))
                            return true;
                        else
                            return false;
                    }

                    State = State.ParentState;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        public virtual bool IsItemMergeable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                return false;

            else
            {
                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMergeable(inner, index))
                            return true;
                        else
                            return false;
                    }

                    State = State.ParentState;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsItemMoveable(int direction, out IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                return false;

            else
            {
                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> AsCollectionInner)
                    {
                        inner = AsCollectionInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMoveable(inner, index, direction))
                            return true;
                        else
                            return false;
                    }

                    State = State.ParentState;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsBlockMoveable(int direction, out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out int blockIndex)
        {
            inner = null;
            blockIndex = -1;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            if (FocusedCellView.Frame is IFocusInsertFrame AsInsertFrame)
                return false;

            else
            {
                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ParentIndex != null);
                        blockIndex = ParentIndex.BlockIndex;

                        if (Controller.IsBlockMoveable(inner, blockIndex, direction))
                            return true;
                        else
                            return false;
                    }

                    State = State.ParentState;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        public virtual bool IsItemCyclableThrough(out IFocusNodeState state, out int cyclePosition)
        {
            state = null;
            cyclePosition = -1;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState CurrentState = FocusedCellView.StateView.State;

            // Search recursively for a collection parent.
            while (CurrentState != null)
            {
                if ((CurrentState.Node is IBody) || (CurrentState.Node is IFeature))
                {
                    Controller.AddNodeToCycle(CurrentState);

                    IFocusInsertionChildIndexList CycleIndexList = CurrentState.CycleIndexList;
                    Debug.Assert(CycleIndexList.Count >= 2);
                    int CurrentPosition = CurrentState.CycleCurrentPosition;
                    Debug.Assert(CurrentPosition >= 0 && CurrentPosition < CycleIndexList.Count);

                    state = CurrentState;
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
        public virtual bool IsItemSimplifiable(out IFocusInner<IFocusBrowsingChildIndex> inner, out IFocusInsertionChildIndex index)
        {
            inner = null;
            index = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState CurrentState = FocusedCellView.StateView.State;

            // Search recursively for a simplifiable node.
            while (CurrentState != null)
            {
                if (NodeHelper.GetSimplifiedNode(CurrentState.Node, out INode SimplifiedNode))
                {
                    if (SimplifiedNode == null)
                        return false;

                    Type InterfaceType = CurrentState.ParentInner.InterfaceType;
                    if (!InterfaceType.IsAssignableFrom(SimplifiedNode.GetType()))
                        return false;

                    IFocusBrowsingChildIndex ParentIndex = CurrentState.ParentIndex as IFocusBrowsingChildIndex;
                    Debug.Assert(ParentIndex != null);

                    inner = CurrentState.ParentInner;
                    index = ParentIndex.ToInsertionIndex(inner.Owner.Node, SimplifiedNode) as IFocusInsertionChildIndex;
                    return true;
                }

                CurrentState = CurrentState.ParentState;
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to replace upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        public virtual bool IsIdentifierSplittable(out IFocusListInner<IFocusBrowsingListNodeIndex> inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex)
        {
            inner = null;
            replaceIndex = null;
            insertIndex = null;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState IdentifierState = FocusedCellView.StateView.State;
            if (IdentifierState.Node is IIdentifier AsIdentifier)
            {
                IFocusNodeState ParentState = IdentifierState.ParentState;
                if (ParentState.Node is IQualifiedName)
                {
                    string Text = AsIdentifier.Text;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= Text.Length);

                    inner = IdentifierState.ParentInner as IFocusListInner<IFocusBrowsingListNodeIndex>;
                    Debug.Assert(inner != null);

                    IFocusBrowsingListNodeIndex CurrentIndex = IdentifierState.ParentIndex as IFocusBrowsingListNodeIndex;
                    Debug.Assert(CurrentIndex != null);

                    IIdentifier FirstPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(0, CaretPosition));
                    IIdentifier SecondPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(CaretPosition));

                    replaceIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, FirstPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(replaceIndex != null);

                    insertIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, SecondPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(replaceIndex != null);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        public virtual bool IsReplicationModifiable(out IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, out int blockIndex, out ReplicationStatus replication)
        {
            inner = null;
            blockIndex = -1;
            replication = ReplicationStatus.Normal;

            Debug.Assert(FocusedCellView != null);

            IFocusNodeState State = FocusedCellView.StateView.State;

            // Search recursively for a collection parent, up to 3 levels up.
            for (int i = 0; i < 3 && State != null; i++)
            {
                if (State is IFocusPatternState AsPatternState)
                {
                    IFocusBlockState ParentBlock = AsPatternState.ParentBlockState;
                    IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    return true;
                }

                else if (State is IFocusSourceState AsSourceState)
                {
                    IFocusBlockState ParentBlock = AsSourceState.ParentBlockState;
                    IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    return true;
                }

                else if (State.ParentInner is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    inner = AsBlockListInner;
                    IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ParentIndex != null);
                    blockIndex = ParentIndex.BlockIndex;
                    replication = inner.BlockStateList[blockIndex].ChildBlock.Replication;
                    return true;
                }

                State = State.ParentState;
            }

            return false;
        }
        #endregion

        #region Implementation
        /// <summary></summary>
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

        /// <summary></summary>
        protected override void Refresh(IFrameNodeState state)
        {
            base.Refresh(state);

            UpdateFocusChain((IFocusNodeState)state);
        }

        /// <summary></summary>
        protected virtual void UpdateFocusChain(IFocusNodeState state)
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
                RecoverFocus(state, NewFocusChain);

            FocusChain = NewFocusChain;

            Debug.Assert(FocusedCellView != null);
            Debug.Assert(FocusChain.Contains(FocusedCellView));
        }

        /// <summary></summary>
        protected virtual void RecoverFocus(IFocusNodeState state, IFocusFocusableCellViewList newFocusChain)
        {
            IFocusNodeState CurrentState = state;
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView;
            List<IFocusFocusableCellView> SameStateFocusableList = new List<IFocusFocusableCellView>();

            for (;;) 
            {
                MainStateView = StateViewTable[CurrentState];

                // Get the state that should have the focus and all its children.
                StateViewList.Clear();
                GetChildrenStateView(MainStateView, StateViewList);

                // Find all focusable cells belonging to these states.
                foreach (IFocusFocusableCellView CellView in newFocusChain)
                    foreach (IFocusNodeStateView StateView in StateViewList)
                        if (CellView.StateView == StateView)
                        {
                            SameStateFocusableList.Add(CellView);
                            break;
                        }

                if (SameStateFocusableList.Count > 0)
                    break;

                // If it doesn't work, try the parent state, down to the root (in case of a removal or unassign).
                if (CurrentState == null)
                    CurrentState = state;
                else
                    CurrentState = CurrentState.ParentState;
                if (CurrentState == null)
                    break;
            }

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
            {
                bool IsFrameSet = false;
                IFocusNodeTemplate Template = MainStateView.Template as IFocusNodeTemplate;
                Debug.Assert(Template != null);

                Template.GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
                Debug.Assert(FirstPreferredFrame != null);
                Debug.Assert(LastPreferredFrame != null);

                foreach (IFocusFocusableCellView CellView in SameStateFocusableList)
                    if (CellView.Frame == FirstPreferredFrame || CellView.Frame == LastPreferredFrame)
                    {
                        IsFrameSet = true;
                        FocusedCellView = CellView;
                        break;
                    }

                // If none of the preferred frames are visible, use the first focusable cell.
                if (!IsFrameSet)
                    FocusedCellView = SameStateFocusableList[0];
            }

            ResetCaretPosition();
        }

        /// <summary></summary>
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

        /// <summary></summary>
        protected virtual void GetChildrenStateView(IFocusNodeStateView stateView, List<IFocusNodeStateView> stateViewList)
        {
            stateViewList.Add(stateView);

            foreach (KeyValuePair<string, IFocusInner<IFocusBrowsingChildIndex>> Entry in stateView.State.InnerTable)
                if (Entry.Value is IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    Debug.Assert(StateViewTable.ContainsKey(AsPlaceholderInner.ChildState));
                    IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                    GetChildrenStateView(ChildStateView, stateViewList);
                }
                else if (Entry.Value is IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    if (AsOptionalInner.IsAssigned)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[AsOptionalInner.ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);
                    }
                }
                else if (Entry.Value is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    foreach (IFocusNodeState ChildState in AsListInner.StateList)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);
                    }
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
                }
                else
                    Debug.Assert(false);
        }

        /// <summary></summary>
        protected virtual void ResetCaretPosition()
        {
            if (FocusedCellView is IFocusTextFocusableCellView AsText)
                CaretPosition = 0;
            else
                CaretPosition = -1;
        }

        /// <summary></summary>
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

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(INode parentNode, string propertyName, INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }
        #endregion
    }
}
