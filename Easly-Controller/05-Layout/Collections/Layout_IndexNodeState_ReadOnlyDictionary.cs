#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface ILayoutIndexNodeStateReadOnlyDictionary : IFocusIndexNodeStateReadOnlyDictionary, IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>
    {
        new ILayoutNodeState this[ILayoutIndex key] { get; }
        new int Count { get; }
        new bool ContainsKey(ILayoutIndex key);
        new IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class LayoutIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>, ILayoutIndexNodeStateReadOnlyDictionary
    {
        public LayoutIndexNodeStateReadOnlyDictionary(ILayoutIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }
        IReadOnlyNodeState IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(ILayoutIndex)key]; } }
        IEnumerable<IReadOnlyIndex> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        IEnumerable<IReadOnlyNodeState> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        bool IWriteableIndexNodeStateReadOnlyDictionary.ContainsKey(IWriteableIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }
        IWriteableNodeState IWriteableIndexNodeStateReadOnlyDictionary.this[IWriteableIndex key] { get { return this[(ILayoutIndex)key]; } }
        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(ILayoutIndex)key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IWriteableIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).GetEnumerator(); }
        #endregion

        #region Frame
        bool IFrameIndexNodeStateReadOnlyDictionary.ContainsKey(IFrameIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }
        IFrameNodeState IFrameIndexNodeStateReadOnlyDictionary.this[IFrameIndex key] { get { return this[(ILayoutIndex)key]; } }
        IFrameNodeState IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return this[(ILayoutIndex)key]; } }
        IEnumerable<IFrameIndex> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Keys { get { return new List<IFrameIndex>(Keys); } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Values { get { return new List<IFrameNodeState>(Values); } }

        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameIndex, IFrameNodeState>> NewList = new List<KeyValuePair<IFrameIndex, IFrameNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameIndex, IFrameNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IFrameIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).GetEnumerator(); }
        #endregion

        #region Focus
        bool IFocusIndexNodeStateReadOnlyDictionary.ContainsKey(IFocusIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey((ILayoutIndex)key); }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }
        IFocusNodeState IFocusIndexNodeStateReadOnlyDictionary.this[IFocusIndex key] { get { return this[(ILayoutIndex)key]; } }
        IFocusNodeState IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return this[(ILayoutIndex)key]; } }
        IEnumerable<IFocusIndex> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Keys { get { return new List<IFocusIndex>(Keys); } }
        IEnumerable<IFocusNodeState> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Values { get { return new List<IFocusNodeState>(Values); } }

        IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFocusIndex, IFocusNodeState>> NewList = new List<KeyValuePair<IFocusIndex, IFocusNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFocusIndex, IFocusNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> IFocusIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).GetEnumerator(); }
        #endregion
    }
}
