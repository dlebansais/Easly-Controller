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
        new int Count { get; }
        new IFocusFrame this[int index] { get; set; }
        new IEnumerator<IFocusFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    public class FocusFrameList : Collection<IFocusFrame>, IFocusFrameList
    {
        #region Frame
        public new IFrameFrame this[int index] { get { return base[index]; } set { base[index] = (IFocusFrame)value; } }
        public void Add(IFrameFrame item) { base.Add((IFocusFrame)item); }
        public void Insert(int index, IFrameFrame item) { base.Insert(index, (IFocusFrame)item); }
        public bool Remove(IFrameFrame item) { return base.Remove((IFocusFrame)item); }
        public void CopyTo(IFrameFrame[] array, int index) { base.CopyTo((IFocusFrame[])array, index); }
        bool ICollection<IFrameFrame>.IsReadOnly { get { return ((ICollection<IFocusFrame>)this).IsReadOnly; } }
        public bool Contains(IFrameFrame value) { return base.Contains((IFocusFrame)value); }
        public int IndexOf(IFrameFrame value) { return base.IndexOf((IFocusFrame)value); }
        public new IEnumerator<IFrameFrame> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
