namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBlockStateView : WriteableBlockStateView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameBlockStateView"/> object.
        /// </summary>
        public static new FrameBlockStateView Empty { get; } = new FrameBlockStateView(FrameControllerView.Empty, FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>.Empty, FrameTemplate.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        /// <param name="template">The frame template.</param>
        protected FrameBlockStateView(FrameControllerView controllerView, IFrameBlockState blockState, IFrameTemplate template)
            : base(controllerView, blockState)
        {
            Template = template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FrameBlockStateView(FrameControllerView controllerView, IFrameBlockState blockState)
            : base(controllerView, blockState)
        {
            Type BlockType = BlockState.ParentInner.BlockType;
            Template = ControllerView.TemplateSet.BlockTypeToTemplate(BlockType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new FrameControllerView ControllerView { get { return (FrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public IFrameCellViewCollection EmbeddingCellView { get; private set; }

        /// <summary>
        /// True if the block view contain at least one visible cell view.
        /// </summary>
        public virtual bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(RootCellView != null);
                return RootCellView.HasVisibleCellView;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.BlockStateView == this);

            IFrameBlockTemplate BlockTemplate = Template as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            SetRootCellView(BlockTemplate.BuildBlockCells(context, null));

            Debug.Assert(EmbeddingCellView != null);
        }

        private protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        public virtual void AssignEmbeddingCellView(IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView != null);
            Debug.Assert(EmbeddingCellView == null);

            EmbeddingCellView = embeddingCellView;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to delete cells.</param>
        public virtual void ClearRootCellView(IFrameNodeStateView stateView)
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            EmbeddingCellView = null;
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
        public virtual bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed)
        {
            Debug.Assert(handler != null);

            Debug.Assert(RootCellView != null);
            return RootCellView.EnumerateVisibleCellViews(handler, out cellView, reversed);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Template, AsBlockStateView.Template))
                return comparer.Failed();

            if (!IsRootCellViewEqual(comparer, AsBlockStateView))
                return comparer.Failed();

            if (!IsEmbeddingCellViewEqual(comparer, AsBlockStateView))
                return comparer.Failed();

            return true;
        }

        private protected virtual bool IsRootCellViewEqual(CompareEqual comparer, FrameBlockStateView other)
        {
            if (!comparer.IsTrue((RootCellView == null && other.RootCellView == null) || (RootCellView != null && other.RootCellView != null)))
                return comparer.Failed();

            if (RootCellView != null)
            {
                if (!comparer.VerifyEqual(RootCellView, other.RootCellView))
                    return comparer.Failed();
            }

            return true;
        }

        private protected virtual bool IsEmbeddingCellViewEqual(CompareEqual comparer, FrameBlockStateView other)
        {
            if (!comparer.IsTrue((EmbeddingCellView == null && other.EmbeddingCellView == null) || (EmbeddingCellView != null && other.EmbeddingCellView != null)))
                return comparer.Failed();

            if (EmbeddingCellView != null)
            {
                if (!comparer.VerifyEqual(EmbeddingCellView, other.EmbeddingCellView))
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

            if (IsValid)
            {
                FrameAssignableCellViewDictionary<string> EmptyCellViewTable = CreateCellViewTable();
                FrameAssignableCellViewReadOnlyDictionary<string> ExpectedCellViewTable = EmptyCellViewTable.ToReadOnly();
                FrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
                IsValid &= RootCellView.IsCellViewTreeValid(ExpectedCellViewTable, ActualCellViewTable);
                IsValid &= ActualCellViewTable.Count == 0;
            }

            return IsValid;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected virtual FrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockStateView));
            return new FrameAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
