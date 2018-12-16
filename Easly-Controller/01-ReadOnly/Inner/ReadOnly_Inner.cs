using BaseNode;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IReadOnlyInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        void CloneChildren(INode parentNode);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IReadOnlyInner<out IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        void CloneChildren(INode parentNode);

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public abstract class ReadOnlyInner<IIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyInner(IReadOnlyNodeState owner, string propertyName)
        {
            Debug.Assert(owner != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Owner = owner;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public abstract Type InterfaceType { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public abstract IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public abstract void CloneChildren(INode parentNode);
        #endregion
    }
}
