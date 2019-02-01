namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    internal interface IFocusIndexCollection : IFrameIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFocusIndexCollection<out IIndex> : IFrameIndexCollection<IIndex>
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
    }
}
