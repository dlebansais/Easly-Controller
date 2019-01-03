﻿using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IReadOnlySourceStateView : IReadOnlyPlaceholderNodeStateView
    {
        /// <summary>
        /// The source state.
        /// </summary>
        new IReadOnlySourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class ReadOnlySourceStateView : ReadOnlyPlaceholderNodeStateView, IReadOnlySourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySourceStateView"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        public ReadOnlySourceStateView(IReadOnlySourceState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The source state.
        /// </summary>
        public new IReadOnlySourceState State { get { return (IReadOnlySourceState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlySourceStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlySourceStateView AsSourceStateView))
                return false;

            if (!base.IsEqual(comparer, AsSourceStateView))
                return false;

            return true;
        }
        #endregion
    }
}
