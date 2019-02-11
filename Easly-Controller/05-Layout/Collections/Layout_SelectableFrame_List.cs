#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    public interface ILayoutSelectableFrameList : IFocusSelectableFrameList, IList<ILayoutSelectableFrame>, IReadOnlyList<ILayoutSelectableFrame>
    {
        new ILayoutSelectableFrame this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutSelectableFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    internal class LayoutSelectableFrameList : Collection<ILayoutSelectableFrame>, ILayoutSelectableFrameList
    {
        #region Focus
        IFocusSelectableFrame IFocusSelectableFrameList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutSelectableFrame)value; } }
        IFocusSelectableFrame IList<IFocusSelectableFrame>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutSelectableFrame)value; } }
        int IList<IFocusSelectableFrame>.IndexOf(IFocusSelectableFrame value) { return IndexOf((ILayoutSelectableFrame)value); }
        void IList<IFocusSelectableFrame>.Insert(int index, IFocusSelectableFrame item) { Insert(index, (ILayoutSelectableFrame)item); }
        void ICollection<IFocusSelectableFrame>.Add(IFocusSelectableFrame item) { Add((ILayoutSelectableFrame)item); }
        bool ICollection<IFocusSelectableFrame>.Contains(IFocusSelectableFrame value) { return Contains((ILayoutSelectableFrame)value); }
        void ICollection<IFocusSelectableFrame>.CopyTo(IFocusSelectableFrame[] array, int index) { CopyTo((ILayoutSelectableFrame[])array, index); }
        bool ICollection<IFocusSelectableFrame>.IsReadOnly { get { return ((ICollection<ILayoutSelectableFrame>)this).IsReadOnly; } }
        bool ICollection<IFocusSelectableFrame>.Remove(IFocusSelectableFrame item) { return Remove((ILayoutSelectableFrame)item); }
        IEnumerator<IFocusSelectableFrame> IEnumerable<IFocusSelectableFrame>.GetEnumerator() { return GetEnumerator(); }
        IFocusSelectableFrame IReadOnlyList<IFocusSelectableFrame>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
