namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IReadOnlyRootNodeIndex : IReadOnlyNodeIndex, IEqualComparable
    {
    }

    /// <inheritdoc/>
    public class ReadOnlyRootNodeIndex : IReadOnlyRootNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyRootNodeIndex"/> object.
        /// </summary>
        public static ReadOnlyRootNodeIndex Empty { get; } = new ReadOnlyRootNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRootNodeIndex"/> class.
        /// </summary>
        protected ReadOnlyRootNodeIndex()
        {
            Node = Node.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public ReadOnlyRootNodeIndex(Node node)
        {
            if (!NodeTreeDiagnostic.IsValid(node, throwOnInvalid: false))
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
        /// <inheritdoc/>
        public Node Node { get; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
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
