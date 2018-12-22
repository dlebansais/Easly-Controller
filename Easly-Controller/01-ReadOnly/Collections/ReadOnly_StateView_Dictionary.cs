using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public interface IReadOnlyStateViewDictionary : IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>
    {
        bool IsEqual(IReadOnlyStateViewDictionary other);
    }

    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public class ReadOnlyStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IReadOnlyStateViewDictionary
    {
        #region Debugging
        public virtual bool IsEqual(IReadOnlyStateViewDictionary other)
        {
            if (Count != other.Count)
                return false;

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this)
            {
                IReadOnlyNodeState Key = Entry.Key;
                IReadOnlyNodeStateView Value = Entry.Value;

                if (!other.ContainsKey(Key))
                    return false;

                if (!Value.IsEqual(other[Key]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
