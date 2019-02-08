namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameAssignableCellViewDictionary<TKey> : IDictionary<TKey, IFrameAssignableCellView>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        IFrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly();
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewDictionary<TKey>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IFrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new FrameAssignableCellViewReadOnlyDictionary<TKey>(this);
        }
    }
}
