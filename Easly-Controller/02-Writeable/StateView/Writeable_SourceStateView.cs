using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IWriteableSourceStateView : IReadOnlySourceStateView, IWriteablePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IWriteableSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class WriteableSourceStateView : ReadOnlySourceStateView, IWriteableSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public WriteableSourceStateView(IWriteableControllerView controllerView, IWriteableSourceState state)
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
        /// The pattern state.
        /// </summary>
        public new IWriteableSourceState State { get { return (IWriteableSourceState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableSourceStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableSourceStateView AsSourceStateView))
                return false;

            if (!base.IsEqual(comparer, AsSourceStateView))
                return false;

            return true;
        }
        #endregion
    }
}
