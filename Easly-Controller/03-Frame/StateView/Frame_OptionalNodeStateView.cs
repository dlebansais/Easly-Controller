﻿namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using Easly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public class FrameOptionalNodeStateView : WriteableOptionalNodeStateView, IFrameNodeStateView, IFrameReplaceableStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public FrameOptionalNodeStateView(FrameControllerView controllerView, IFrameOptionalNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new FrameControllerView ControllerView { get { return (FrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                IOptionalReference Optional = State.Optional;
                Debug.Assert(Optional != null);

                Type InterfaceType;
                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out Node ChildNode);
                if (ChildNode != null)
                {
                    Type NodeType = Type.FromGetType(ChildNode);
                    Debug.Assert(!NodeType.IsInterface && !NodeType.IsAbstract);

                    //InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
                    InterfaceType = NodeType;
                }
                else
                {
                    InterfaceType = NodeHelper.GetDefaultItemType(State.ParentInner.InterfaceType);
                    //Debug.Assert(InterfaceType.IsInterface);
                    Debug.Assert(!InterfaceType.IsInterface);
                }

                IFrameTemplate Result = ControllerView.TemplateSet.NodeTypeToTemplate(InterfaceType);
                Debug.Assert(Result != null);

                return Result;
            }
        }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public FrameAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; private set; }
        private FrameAssignableCellViewDictionary<string> _CellViewTable;

        /// <summary>
        /// True if the node view contain at least one visible cell view.
        /// </summary>
        public virtual bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(RootCellView != null);
                return RootCellView.HasVisibleCellView;
            }
        }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public IFrameContainerCellView ParentContainer { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.StateView == this);

            IOptionalReference Optional = State.Optional;
            Debug.Assert(Optional != null);

            InitCellViewTable(true);

            Debug.Assert(RootCellView == null);

            IFrameCellView CellView;

            if (Optional.IsAssigned)
            {
                IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
                Debug.Assert(NodeTemplate != null);

                CellView = NodeTemplate.BuildNodeCells(context);
            }
            else
            {
                IFrameEmptyCellView EmptyCellView = CreateEmptyCellView(this, null);
                ValidateEmptyCellView(context, EmptyCellView);

                CellView = EmptyCellView;
            }

            SetRootCellView(CellView);
            SealCellViewTable();
        }

        private protected virtual void ValidateEmptyCellView(IFrameCellViewTreeContext context, IFrameEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = emptyCellView.ParentCellView;
        }

        private protected virtual void InitCellViewTable(bool initProperties)
        {
            _CellViewTable = CreateCellViewTable();

            if (initProperties)
            {
                foreach (string Key in State.InnerTable.Keys)
                {
                    IFrameInner Value = (IFrameInner)State.InnerTable[Key];

                    _CellViewTable.Add(Value.PropertyName, null);
                }
            }
        }

        private protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        private protected virtual void SealCellViewTable()
        {
            CellViewTable = _CellViewTable.ToReadOnly();
        }

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public virtual void AssignCellViewTable(string propertyName, IFrameAssignableCellView cellView)
        {
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Contract.RequireNotNull(cellView, out IFrameAssignableCellView CellView);

            Debug.Assert(_CellViewTable.ContainsKey(PropertyName));
            Debug.Assert(_CellViewTable[PropertyName] == null);

            _CellViewTable[PropertyName] = CellView;
        }

        /// <summary>
        /// Set the container for this state view.
        /// </summary>
        /// <param name="parentContainer">The cell view where the tree is restarted.</param>
        public virtual void SetContainerCellView(IFrameContainerCellView parentContainer)
        {
            ParentContainer = parentContainer;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        public virtual void ClearRootCellView()
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            _CellViewTable = null;
            CellViewTable = null;
        }

        /// <summary>
        /// Replaces the cell view for the given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="cellView">The new cell view.</param>
        public virtual void ReplaceCellView(string propertyName, IFrameContainerCellView cellView)
        {
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Contract.RequireNotNull(cellView, out IFrameContainerCellView CellView);

            Debug.Assert(_CellViewTable.ContainsKey(PropertyName));
            Debug.Assert(_CellViewTable[PropertyName] != null);

            _CellViewTable[PropertyName] = CellView;
        }

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
        }

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        public virtual bool EnumerateVisibleCellViews(System.Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed)
        {
            Contract.RequireNotNull(handler, out System.Func<IFrameVisibleCellView, bool> Handler);

            Debug.Assert(RootCellView != null);
            return RootCellView.EnumerateVisibleCellViews(Handler, out cellView, reversed);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameOptionalNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameOptionalNodeStateView AsOptionalNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Template, AsOptionalNodeStateView.Template))
                return comparer.Failed();

            if (!comparer.IsTrue((RootCellView == null || AsOptionalNodeStateView.RootCellView != null) && (RootCellView != null || AsOptionalNodeStateView.RootCellView == null)))
                return comparer.Failed();

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsOptionalNodeStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsOptionalNodeStateView.RootCellView))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(CellViewTable, AsOptionalNodeStateView.CellViewTable))
                    return comparer.Failed();
            }

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public virtual bool IsCellViewTreeValid()
        {
            bool IsValid = true;

            IsValid &= RootCellView != null;

            if (IsValid && !(RootCellView is IFrameEmptyCellView))
            {
                FrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
                IsValid &= RootCellView.IsCellViewTreeValid(CellViewTable, ActualCellViewTable);
                IsValid &= AllCellViewsProperlyAssigned(CellViewTable, ActualCellViewTable);
            }

            return IsValid;
        }

        private protected virtual bool AllCellViewsProperlyAssigned(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            bool IsAssigned = true;

            int ActualCount = 0;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in CellViewTable)
                if (Entry.Value != null)
                {
                    ActualCount++;
                    IsAssigned &= actualCellViewTable.ContainsKey(Entry.Key);
                }

            IsAssigned &= actualCellViewTable.Count == ActualCount;

            return IsAssigned;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected virtual FrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameOptionalNodeStateView>());
            return new FrameAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameOptionalNodeStateView>());
            return new FrameEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
