﻿namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewReadOnlyDictionary(ReadOnlyBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
