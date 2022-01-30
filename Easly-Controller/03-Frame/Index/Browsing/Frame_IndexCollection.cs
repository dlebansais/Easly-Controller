namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IFrameIndexCollection : IWriteableIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFrameIndexCollection<out IIndex> : IWriteableIndexCollection<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal class FrameIndexCollection<IIndex> : WriteableIndexCollection<IIndex>, IFrameIndexCollection<IIndex>, IFrameIndexCollection
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameIndexCollection{IIndex}"/> object.
        /// </summary>
        public static new FrameIndexCollection<IIndex> Empty { get; } = new FrameIndexCollection<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameIndexCollection{IIndex}"/> class.
        /// </summary>
        protected FrameIndexCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public FrameIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion
    }
}
