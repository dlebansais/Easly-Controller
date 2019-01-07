using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IWriteableBlockStateView : IReadOnlyBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IWriteableControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new IWriteableBlockState BlockState { get; }
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class WriteableBlockStateView : ReadOnlyBlockStateView, IWriteableBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public WriteableBlockStateView(IWriteableControllerView controllerView, IWriteableBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IWriteableControllerView ControllerView { get { return (IWriteableControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IWriteableBlockState BlockState { get { return (IWriteableBlockState)base.BlockState; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableBlockStateView AsBlockStateView))
                return false;

            if (!base.IsEqual(comparer, AsBlockStateView))
                return false;

            return true;
        }
        #endregion
    }
}
