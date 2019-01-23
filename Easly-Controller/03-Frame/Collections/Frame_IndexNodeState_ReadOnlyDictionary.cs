using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFrameIndexNodeStateReadOnlyDictionary : IWriteableIndexNodeStateReadOnlyDictionary, IReadOnlyDictionary<IFrameIndex, IFrameNodeState>
    {
        new IFrameNodeState this[IFrameIndex key] { get; }
        new IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> GetEnumerator();
        new bool ContainsKey(IFrameIndex key);
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class FrameIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IFrameIndex, IFrameNodeState>, IFrameIndexNodeStateReadOnlyDictionary
    {
        public FrameIndexNodeStateReadOnlyDictionary(IFrameIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IFrameIndex)key); }
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value)
        {
            bool Result = TryGetValue((IFrameIndex)key, out IFrameNodeState Value);
            value = Value;
            return Result;
        }
        IReadOnlyNodeState IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IFrameIndex)key]; } }
        IEnumerable<IReadOnlyIndex> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        IEnumerable<IReadOnlyNodeState> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameIndex, IFrameNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        bool IWriteableIndexNodeStateReadOnlyDictionary.ContainsKey(IWriteableIndex key) { return ContainsKey((IFrameIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((IFrameIndex)key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value)
        {
            bool Result = TryGetValue((IFrameIndex)key, out IFrameNodeState Value);
            value = Value;
            return Result;
        }
        IWriteableNodeState IWriteableIndexNodeStateReadOnlyDictionary.this[IWriteableIndex key] { get { return this[(IFrameIndex)key]; } }
        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(IFrameIndex)key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameIndex, IFrameNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IWriteableIndexNodeStateReadOnlyDictionary.GetEnumerator() { return ((IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).GetEnumerator(); }
        #endregion
    }
}
