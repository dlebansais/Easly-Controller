﻿using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IReadOnlyBrowsingNewBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        IIdentifier SourceNode { get; }
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public class ReadOnlyBrowsingNewBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public ReadOnlyBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(node, propertyName, blockIndex)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(patternNode != null);
            Debug.Assert(sourceNode != null);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(parentNode, propertyName, blockIndex, 0, node));
            Debug.Assert(NodeTreeHelperBlockList.IsBlockPatternNode(parentNode, propertyName, blockIndex, patternNode));
            Debug.Assert(NodeTreeHelperBlockList.IsBlockSourceNode(parentNode, propertyName, blockIndex, sourceNode));

            PatternNode = patternNode;
            SourceNode = sourceNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        public IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        public IIdentifier SourceNode { get; }
        #endregion
    }
}
