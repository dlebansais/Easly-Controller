#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    public interface IFocusFrameList : IFrameFrameList, IList<IFocusFrame>, IReadOnlyList<IFocusFrame>
    {
        new IFocusFrame this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    internal class FocusFrameList : Collection<IFocusFrame>, IFocusFrameList
    {
        #region Frame
        IFrameFrame IFrameFrameList.this[int index] { get { return this[index]; } set { this[index] = (IFocusFrame)value; } }
        IFrameFrame IList<IFrameFrame>.this[int index] { get { return this[index]; } set { this[index] = (IFocusFrame)value; } }
        int IList<IFrameFrame>.IndexOf(IFrameFrame value) { return IndexOf((IFocusFrame)value); }
        void IList<IFrameFrame>.Insert(int index, IFrameFrame item) { Insert(index, (IFocusFrame)item); }
        void ICollection<IFrameFrame>.Add(IFrameFrame item) { Add((IFocusFrame)item); }
        bool ICollection<IFrameFrame>.Contains(IFrameFrame value) { return Contains((IFocusFrame)value); }
        void ICollection<IFrameFrame>.CopyTo(IFrameFrame[] array, int index) { CopyTo((IFocusFrame[])array, index); }
        bool ICollection<IFrameFrame>.IsReadOnly { get { return ((ICollection<IFocusFrame>)this).IsReadOnly; } }
        bool ICollection<IFrameFrame>.Remove(IFrameFrame item) { return Remove((IFocusFrame)item); }
        IEnumerator<IFrameFrame> IEnumerable<IFrameFrame>.GetEnumerator() { return GetEnumerator(); }
        IFrameFrame IReadOnlyList<IFrameFrame>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
