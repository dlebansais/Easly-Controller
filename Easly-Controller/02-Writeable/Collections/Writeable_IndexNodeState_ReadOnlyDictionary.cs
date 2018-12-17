using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IWriteableIndexNodeStateReadOnlyDictionary : IReadOnlyIndexNodeStateReadOnlyDictionary, IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>
    {
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class WriteableIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>, IWriteableIndexNodeStateReadOnlyDictionary
    {
        public WriteableIndexNodeStateReadOnlyDictionary(IWriteableIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IWriteableIndex)key); }
        bool IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue((IWriteableIndex)key, out IWriteableNodeState Value); value = Value; return Result; }
        IReadOnlyNodeState IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IWriteableIndex)key]; } }
        IEnumerable<IReadOnlyIndex> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        IEnumerable<IReadOnlyNodeState> IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
    }
}
