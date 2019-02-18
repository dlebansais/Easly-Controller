#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    public interface IFocusFocusList : IList<IFocusFocus>, IReadOnlyList<IFocusFocus>
    {
        new IFocusFocus this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    internal class FocusFocusList : Collection<IFocusFocus>, IFocusFocusList
    {
    }
}
