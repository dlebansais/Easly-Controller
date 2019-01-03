using System.Collections.Generic;

namespace EaslyController.Frame
{
    /// <summary>
    /// Dictionary of ..., IxxxCellView
    /// </summary>
    public interface IFrameCellViewDictionary<TKey> : IDictionary<TKey, IFrameCellView>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxCellView
    /// </summary>
    public class FrameCellViewDictionary<TKey> : Dictionary<TKey, IFrameCellView>, IFrameCellViewDictionary<TKey>
    {
    }
}
