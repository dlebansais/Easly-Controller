namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IFocusIndexCollection : IFrameIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface IFocusIndexCollection<out IIndex> : IFrameIndexCollection<IIndex>
        where IIndex : IFocusBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal class FocusIndexCollection<IIndex> : FrameIndexCollection<IIndex>, IFocusIndexCollection<IIndex>, IFocusIndexCollection
        where IIndex : IFocusBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public FocusIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndexCollection"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusIndexCollection<IIndex> AsIndexCollection))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsIndexCollection))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
