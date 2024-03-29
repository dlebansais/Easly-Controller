﻿namespace EaslyController.Writeable
{
    using BaseNode;

    /// <summary>
    /// Index for an inserted child.
    /// </summary>
    public interface IWriteableInsertionChildIndex : IWriteableChildIndex, IEqualComparable
    {
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        IWriteableBrowsingChildIndex ToBrowsingIndex();
    }
}
