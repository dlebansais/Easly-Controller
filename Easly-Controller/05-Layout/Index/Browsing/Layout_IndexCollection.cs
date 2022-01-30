namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface ILayoutIndexCollection : IFocusIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface ILayoutIndexCollection<out IIndex> : IFocusIndexCollection<IIndex>
        where IIndex : ILayoutBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal class LayoutIndexCollection<IIndex> : FocusIndexCollection<IIndex>, ILayoutIndexCollection<IIndex>, ILayoutIndexCollection
        where IIndex : ILayoutBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutIndexCollection{IIndex}"/> object.
        /// </summary>
        public static new LayoutIndexCollection<IIndex> Empty { get; } = new LayoutIndexCollection<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutIndexCollection{IIndex}"/> class.
        /// </summary>
        protected LayoutIndexCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public LayoutIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion
    }
}
