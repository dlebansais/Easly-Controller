namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface IFrameCellViewCollection : IFrameCellView, IFrameAssignableCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        IFrameCellViewList CellViewList { get; }

        /// <summary>
        /// Inserts a new cell view in the collection.
        /// </summary>
        /// <param name="index">Index where to insert the cell view.</param>
        /// <param name="cellView">The cell view to insert.</param>
        void Insert(int index, IFrameCellView cellView);

        /// <summary>
        /// Removes a cell view from the collection.
        /// </summary>
        /// <param name="index">Index where to remove the cell view.</param>
        void Remove(int index);

        /// <summary>
        /// Replaces a cell view with another in the collection.
        /// </summary>
        /// <param name="oldCellView">The cell view to replace.</param>
        /// <param name="newCellView">The new cell view.</param>
        void Replace(IFrameCellView oldCellView, IFrameCellView newCellView);

        /// <summary>
        /// Replaces a cell view with another in the collection.
        /// </summary>
        /// <param name="index">Index where to replace the cell view.</param>
        /// <param name="cellView">The new cell view.</param>
        void Replace(int index, IFrameCellView cellView);

        /// <summary>
        /// Moves a cell view around in the collection.
        /// </summary>
        /// <param name="cellView">The cell view to move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IFrameCellView cellView, int direction);

        /// <summary>
        /// Moves a cell view around in the collection.
        /// </summary>
        /// <param name="index">Index of the cell view to move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(int index, int direction);
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public abstract class FrameCellViewCollection : FrameCellView, IFrameCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FrameCellViewCollection(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView)
        {
            CellViewList = cellViewList;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public IFrameCellViewList CellViewList { get; }

        /// <summary>
        /// True if the cell is assigned to a property in a cell view table.
        /// </summary>
        public bool IsAssignedToTable { get; private set; }

        /// <summary>
        /// True if the collection contain at least one visible cell view.
        /// </summary>
        public override bool HasVisibleCellView
        {
            get
            {
                foreach (IFrameCellView CellView in CellViewList)
                    if (CellView.HasVisibleCellView)
                        return true;

                return false;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Indicates that the cell view is assigned to a property in a cell view table.
        /// </summary>
        public virtual void AssignToCellViewTable()
        {
            Debug.Assert(!IsAssignedToTable);

            IsAssignedToTable = true;
        }

        /// <summary>
        /// Inserts a new cell view in the collection.
        /// </summary>
        /// <param name="index">Index where to insert the cell view.</param>
        /// <param name="cellView">The cell view to insert.</param>
        public virtual void Insert(int index, IFrameCellView cellView)
        {
            Debug.Assert(index >= 0 && index <= CellViewList.Count);
            Debug.Assert(cellView != null);

            CellViewList.Insert(index, cellView);

            foreach (IFrameCellView Item in CellViewList)
                if (Item is IFrameContainerCellView AsContainerCellView)
                    Debug.Assert(AsContainerCellView.ParentCellView == this);
        }

        /// <summary>
        /// Removes a cell view from the collection.
        /// </summary>
        /// <param name="index">Index where to remove the cell view.</param>
        public virtual void Remove(int index)
        {
            Debug.Assert(index >= 0 && index <= CellViewList.Count);

            CellViewList.RemoveAt(index);
        }

        /// <summary>
        /// Replaces a cell view with another in the collection.
        /// </summary>
        /// <param name="oldCellView">The cell view to replace.</param>
        /// <param name="newCellView">The new cell view.</param>
        public virtual void Replace(IFrameCellView oldCellView, IFrameCellView newCellView)
        {
            Debug.Assert(CellViewList.Contains(oldCellView));
            Debug.Assert(!CellViewList.Contains(newCellView));

            int Index = CellViewList.IndexOf(oldCellView);
            Replace(Index, newCellView);
        }

        /// <summary>
        /// Replaces a cell view with another in the collection.
        /// </summary>
        /// <param name="index">Index where to replace the cell view.</param>
        /// <param name="cellView">The new cell view.</param>
        public virtual void Replace(int index, IFrameCellView cellView)
        {
            Debug.Assert(index >= 0 && index < CellViewList.Count);
            Debug.Assert(cellView != null);

            CellViewList[index] = cellView;

            foreach (IFrameCellView Item in CellViewList)
                if (Item is IFrameContainerCellView AsContainerCellView)
                    Debug.Assert(AsContainerCellView.ParentCellView == this);
        }

        /// <summary>
        /// Moves a cell view around in the collection.
        /// </summary>
        /// <param name="cellView">The cell view to move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IFrameCellView cellView, int direction)
        {
            Debug.Assert(CellViewList.Contains(cellView));

            int Index = CellViewList.IndexOf(cellView);
            Move(Index, direction);
        }

        /// <summary>
        /// Moves a cell view around in the collection.
        /// </summary>
        /// <param name="index">Index of the cell view to move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(int index, int direction)
        {
            Debug.Assert(index >= 0 && index < CellViewList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < CellViewList.Count);

            IFrameCellView CellView = CellViewList[index];
            CellViewList.RemoveAt(index);
            CellViewList.Insert(index + direction, CellView);
        }

        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
            foreach (IFrameCellView Item in CellViewList)
                Item.ClearCellTree();

            CellViewList.Clear();
        }

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public override void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            foreach (IFrameCellView Item in CellViewList)
                Item.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellViewCollection AsCellViewCollection))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return comparer.Failed();

            if (!comparer.VerifyEqual(CellViewList, AsCellViewCollection.CellViewList))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public override bool IsCellViewTreeValid(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            foreach (IFrameCellView CellView in CellViewList)
                if (!CellView.IsCellViewTreeValid(expectedCellViewTable, actualCellViewTable))
                    return false;

            if (!IsCellViewProperlyAssigned(expectedCellViewTable, actualCellViewTable))
                return false;

            return true;
        }

        /// <summary></summary>
        private protected virtual bool IsCellViewProperlyAssigned(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            string PropertyName = null;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in expectedCellViewTable)
                if (Entry.Value == this)
                {
                    PropertyName = Entry.Key;
                    break;
                }

            if (IsAssignedToTable != (PropertyName != null))
                return false;

            if (PropertyName != null)
            {
                bool IsActual = false;
                foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in actualCellViewTable)
                    if (Entry.Value == this)
                    {
                        IsActual = true;
                        break;
                    }
                if (IsActual)
                    return false;

                actualCellViewTable.Add(PropertyName, this);
            }

            return true;
        }
        #endregion
    }
}
