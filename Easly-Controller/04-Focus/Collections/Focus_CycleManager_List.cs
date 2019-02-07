#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxCycleManager
    /// </summary>
    public interface IFocusCycleManagerList : IList<IFocusCycleManager>, IReadOnlyList<IFocusCycleManager>
    {
        new IFocusCycleManager this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxCycleManager
    /// </summary>
    internal class FocusCycleManagerList : Collection<IFocusCycleManager>, IFocusCycleManagerList
    {
    }
}
