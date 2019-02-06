#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxFocusableCellView
    /// </summary>
    public interface IFocusFocusableCellViewList : IList<IFocusFocusableCellView>, IReadOnlyList<IFocusFocusableCellView>
    {
        new IFocusFocusableCellView this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxFocusableCellView
    /// </summary>
    internal class FocusFocusableCellViewList : Collection<IFocusFocusableCellView>, IFocusFocusableCellViewList
    {
    }
}
