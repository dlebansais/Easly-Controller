﻿using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IWriteablePlaceholderNodeStateView : IReadOnlyPlaceholderNodeStateView, IWriteableNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IWriteablePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class WriteablePlaceholderNodeStateView : ReadOnlyPlaceholderNodeStateView, IWriteablePlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public WriteablePlaceholderNodeStateView(IWriteableControllerView controllerView, IWriteablePlaceholderNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IWriteableControllerView ControllerView { get { return (IWriteableControllerView)base.ControllerView; } }

        /// <summary>
        /// The child node.
        /// </summary>
        public new IWriteablePlaceholderNodeState State { get { return (IWriteablePlaceholderNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteablePlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteablePlaceholderNodeStateView AsPlaceholderNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return false;

            return true;
        }
        #endregion
    }
}
