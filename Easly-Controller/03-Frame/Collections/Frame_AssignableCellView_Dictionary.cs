namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameAssignableCellViewDictionary<TKey> : IDictionary<TKey, IFrameAssignableCellView>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewDictionary<TKey>
    {
    }
}
