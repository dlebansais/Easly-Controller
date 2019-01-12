using System.Collections.Generic;

namespace EaslyController.Frame
{
    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    public interface IFrameAssignableCellViewDictionary<TKey> : IDictionary<TKey, IFrameAssignableCellView>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    public class FrameAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewDictionary<TKey>
    {
    }
}
