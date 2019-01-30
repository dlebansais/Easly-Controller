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
        new int Count { get; }
        new IFocusFocusableCellView this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxFocusableCellView
    /// </summary>
    public class FocusFocusableCellViewList : Collection<IFocusFocusableCellView>, IFocusFocusableCellViewList
    {
    }
}
