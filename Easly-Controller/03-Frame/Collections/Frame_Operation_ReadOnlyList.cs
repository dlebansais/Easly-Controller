#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public interface IFrameOperationReadOnlyList : IWriteableOperationReadOnlyList, IReadOnlyList<IFrameOperation>
    {
        new int Count { get; }
        new IFrameOperation this[int index] { get; }
        bool Contains(IFrameOperation value);
        int IndexOf(IFrameOperation value);
        new IEnumerator<IFrameOperation> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public class FrameOperationReadOnlyList : ReadOnlyCollection<IFrameOperation>, IFrameOperationReadOnlyList
    {
        public FrameOperationReadOnlyList(IFrameOperationList list)
            : base(list)
        {
        }

        #region Writeable
        public new IWriteableOperation this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableOperation value) { return base.Contains((IFrameOperation)value); }
        public int IndexOf(IWriteableOperation value) { return base.IndexOf((IFrameOperation)value); }
        public new IEnumerator<IWriteableOperation> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
