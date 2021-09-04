namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class WriteableBlockStateView : ReadOnlyBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public WriteableBlockStateView(WriteableControllerView controllerView, IWriteableBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new WriteableControllerView ControllerView { get { return (WriteableControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IWriteableBlockState BlockState { get { return (IWriteableBlockState)base.BlockState; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
