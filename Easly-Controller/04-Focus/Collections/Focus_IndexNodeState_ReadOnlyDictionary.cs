using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFocusIndexNodeStateReadOnlyDictionary : IFrameIndexNodeStateReadOnlyDictionary, IReadOnlyDictionary<IFocusIndex, IFocusNodeState>
    {
        new IFocusNodeState this[IFocusIndex key] { get; }
        new IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> GetEnumerator();
        new bool ContainsKey(IFocusIndex key);
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class FocusIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IFocusIndex, IFocusNodeState>, IFocusIndexNodeStateReadOnlyDictionary
    {
        public FocusIndexNodeStateReadOnlyDictionary(IFocusIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IFocusIndex)key); }
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        IReadOnlyNodeState IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IFocusIndex)key]; } }
        IEnumerable<IReadOnlyIndex> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        IEnumerable<IReadOnlyNodeState> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        bool IWriteableIndexNodeStateReadOnlyDictionary.ContainsKey(IWriteableIndex key) { return ContainsKey((IFocusIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((IFocusIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        IWriteableNodeState IWriteableIndexNodeStateReadOnlyDictionary.this[IWriteableIndex key] { get { return this[(IFocusIndex)key]; } }
        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(IFocusIndex)key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IWriteableIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).GetEnumerator(); }
        #endregion

        #region Frame
        bool IFrameIndexNodeStateReadOnlyDictionary.ContainsKey(IFrameIndex key) { return ContainsKey((IFocusIndex)key); }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey((IFocusIndex)key); }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        IFrameNodeState IFrameIndexNodeStateReadOnlyDictionary.this[IFrameIndex key] { get { return this[(IFocusIndex)key]; } }
        IFrameNodeState IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return this[(IFocusIndex)key]; } }
        IEnumerable<IFrameIndex> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Keys { get { return new List<IFrameIndex>(Keys); } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Values { get { return new List<IFrameNodeState>(Values); } }

        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameIndex, IFrameNodeState>> NewList = new List<KeyValuePair<IFrameIndex, IFrameNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameIndex, IFrameNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IFrameIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).GetEnumerator(); }
        #endregion
    }
}
