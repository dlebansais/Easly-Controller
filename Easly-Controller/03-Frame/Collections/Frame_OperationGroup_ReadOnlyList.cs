﻿using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public interface IFrameOperationGroupReadOnlyList : IWriteableOperationGroupReadOnlyList, IReadOnlyList<IFrameOperationGroup>
    {
        new int Count { get; }
        new IFrameOperationGroup this[int index] { get; }
        bool Contains(IFrameOperationGroup value);
        int IndexOf(IFrameOperationGroup value);
        new IEnumerator<IFrameOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public class FrameOperationGroupReadOnlyList : ReadOnlyCollection<IFrameOperationGroup>, IFrameOperationGroupReadOnlyList
    {
        public FrameOperationGroupReadOnlyList(IFrameOperationGroupList list)
            : base(list)
        {
        }

        #region Writeable
        public new IWriteableOperationGroup this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableOperationGroup value) { return base.Contains((IFrameOperationGroup)value); }
        public int IndexOf(IWriteableOperationGroup value) { return base.IndexOf((IFrameOperationGroup)value); }
        public new IEnumerator<IWriteableOperationGroup> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
