namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class ReadOnlyRootNodeIndex : IReadOnlyNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public ReadOnlyRootNodeIndex(Node node)
        {
            if (!NodeTreeDiagnostic.IsValid(node, assertValid: false))
            {
                Debug.WriteLine($"Invalid node {node}");
                Exception InnerException = null;
                throw new ArgumentException(nameof(node), InnerException);
            }

            Debug.Assert(node != null);

            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed root node.
        /// </summary>
        public Node Node { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyRootNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsRootNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
