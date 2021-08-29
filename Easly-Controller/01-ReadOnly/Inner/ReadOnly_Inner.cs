namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IReadOnlyInner : IEqualComparable
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
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    public abstract class ReadOnlyInner<TIndex> :  IReadOnlyInner, IEqualComparable
        where TIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{TIndex}"/> class.
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
        public abstract void CloneChildren(Node parentNode);

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public abstract void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// DEtach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public abstract void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyInner{TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyInner<TIndex> AsInner))
                return comparer.Failed();

            if (!comparer.VerifyEqual(Owner, AsInner.Owner))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInner.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
