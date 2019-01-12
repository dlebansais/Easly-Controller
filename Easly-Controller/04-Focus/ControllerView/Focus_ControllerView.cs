﻿using BaseNodeHelper;
using EaslyController.Constants;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using System;
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
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has match the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern);
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

            Type InterfaceType = ParentInner.InterfaceType;
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
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has match the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        public virtual bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));

            switch (State.ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.String:
                    Debug.Assert(propertyName == "Text");
                    return NodeTreeHelper.GetText(State.Node) == textPattern;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
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
        #endregion
    }
}