namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateList : List<IReadOnlyBlockState>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyBlockStateReadOnlyList(this);
        }
    }
}
