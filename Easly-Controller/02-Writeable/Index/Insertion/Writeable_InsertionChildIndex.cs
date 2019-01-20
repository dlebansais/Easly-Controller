﻿using BaseNode;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for an inserted child node.
    /// </summary>
    public interface IWriteableInsertionChildIndex : IWriteableChildIndex, IWriteableNodeIndex
    {
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        IWriteableBrowsingChildIndex ToBrowsingIndex();
    }
}
