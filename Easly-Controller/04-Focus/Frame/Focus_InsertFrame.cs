namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;
    using BaseNodeHelper;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for bringing the focus to an insertion point.
    /// </summary>
    public interface IFocusInsertFrame : IFrameInsertFrame, IFocusStaticFrame
    {
        /// <summary>
        /// Type to use when creating a new item in the associated block list or list. Only when it's an abstract type.
        /// (Set in Xaml)
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Type to use when inserting a new item in the collection.
        /// </summary>
        Type InsertType { get; }

        /// <summary>
        /// Returns the inner for the collection associated to this frame, for a given state.
        /// </summary>
        /// <param name="state">The state, modified if <see cref="IFrameInsertFrame.CollectionName"/> points to a different state.</param>
        /// <param name="inner">The inner associated to the collection in <paramref name="state"/>.</param>
        void CollectionNameToInner(ref IFocusNodeState state, ref IFocusCollectionInner inner);
    }

    /// <summary>
    /// Frame for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class FocusInsertFrame : FrameInsertFrame, IFocusInsertFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsPreferred { get; set; }

        /// <summary>
        /// Type to use when creating a new item in the associated block list or list. Only when it's an abstract type.
        /// (Set in Xaml)
        /// </summary>
        public Type ItemType { get; set; }

        /// <summary>
        /// Type to use when inserting a new item in the collection.
        /// </summary>
        public Type InsertType { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);
            IsValid &= Visibility == null || Visibility.IsValid(nodeType);

            Debug.Assert(InsertType != null);

            IsValid &= !InsertType.IsInterface;
            IsValid &= !InsertType.IsAbstract;

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary></summary>
        private protected override void UpdateInterfaceType(Type nodeType)
        {
            base.UpdateInterfaceType(nodeType);

            Debug.Assert(InterfaceType != null);

            Type EstimatedItemType = NodeTreeHelper.InterfaceTypeToNodeType(InterfaceType);
            Debug.Assert(EstimatedItemType != null);

            if (ItemType == null)
                InsertType = EstimatedItemType;
            else
                InsertType = ItemType;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            else
            {
                IFocusEmptyCellView EmptyCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, (IFocusCellViewCollection)parentCellView);
                ValidateEmptyCellView((IFocusCellViewTreeContext)context, EmptyCellView);

                Result = EmptyCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            if (Visibility == null || Visibility.IsVolatile || IsPreferred)
            {
                if (firstPreferredFrame == null || IsPreferred)
                    firstPreferredFrame = this;

                lastPreferredFrame = this;
            }
        }

        /// <summary>
        /// Returns the inner for the collection associated to this frame, for a given state.
        /// </summary>
        /// <param name="state">The state, modified if <see cref="IFrameInsertFrame.CollectionName"/> points to a different state.</param>
        /// <param name="inner">The inner associated to the collection in <paramref name="state"/>.</param>
        public virtual void CollectionNameToInner(ref IFocusNodeState state, ref IFocusCollectionInner inner)
        {
            Debug.Assert(inner == null);

            string[] Split = CollectionName.Split('.');

            for (int i = 0; i < Split.Length; i++)
            {
                string PropertyName = Split[i];

                if (i + 1 < Split.Length)
                {
                    Debug.Assert(state.InnerTable.ContainsKey(PropertyName));
                    bool IsHandled = false;

                    switch (state.InnerTable[PropertyName])
                    {
                        case IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                            state = AsPlaceholderInner.ChildState;
                            IsHandled = true;
                            break;

                        case IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner:
                            Debug.Assert(AsOptionalInner.IsAssigned);
                            state = AsOptionalInner.ChildState;
                            IsHandled = true;
                            break;
                    }

                    Debug.Assert(IsHandled);
                }
                else
                {
                    Debug.Assert(state.InnerTable.ContainsKey(PropertyName));
                    inner = state.InnerTable[PropertyName] as IFocusCollectionInner;
                }
            }

            Debug.Assert(inner != null);
            Debug.Assert(state.InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(state.InnerTable[inner.PropertyName] == inner);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((IFocusVisibleCellView)cellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            Debug.Assert(((IFocusVisibleCellView)cellView).Frame == this);
            IFocusCellViewCollection ParentCellView = ((IFocusVisibleCellView)cellView).ParentCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
            IFocusCellViewCollection ParentCellView = emptyCellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected override IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusFocusableCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this);
        }

        // This class should not need CreateVisibleCellView().

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
