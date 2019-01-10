using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface IFocusCellViewCollection : IFrameCellViewCollection, IFocusCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        new IFocusCellViewList CellViewList { get; }
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public abstract class FocusCellViewCollection : FrameCellViewCollection, IFocusCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameCellViewCollection"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FocusCellViewCollection(IFocusNodeStateView stateView, IFocusCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new IFocusCellViewList CellViewList { get { return (IFocusCellViewList)base.CellViewList; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusCellViewCollection AsCellViewCollection))
                return false;

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return false;

            return true;
        }
        #endregion
    }
}
