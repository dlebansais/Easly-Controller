﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IWriteableIndexCollection : IReadOnlyIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IWriteableIndexCollection<out IIndex> : IReadOnlyIndexCollection<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal class WriteableIndexCollection<IIndex> : ReadOnlyIndexCollection<IIndex>, IWriteableIndexCollection<IIndex>, IWriteableIndexCollection
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="WriteableIndexCollection{IIndex}"/> object.
        /// </summary>
        public static new WriteableIndexCollection<IIndex> Empty { get; } = new WriteableIndexCollection<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableIndexCollection{IIndex}"/> class.
        /// </summary>
        protected WriteableIndexCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public WriteableIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion
    }
}
