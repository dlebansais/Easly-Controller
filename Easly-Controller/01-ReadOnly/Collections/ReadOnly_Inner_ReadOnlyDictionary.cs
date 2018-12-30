using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IReadOnlyInnerReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IEqualComparable
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class ReadOnlyInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerReadOnlyDictionary<TKey>
    {
        public ReadOnlyInnerReadOnlyDictionary(IReadOnlyInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyInnerReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return false;

            if (Count != AsInnerReadOnlyDictionary.Count)
                return false;

            foreach (KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in this)
            {
                if (!AsInnerReadOnlyDictionary.ContainsKey(Entry.Key))
                    return false;

                if (!comparer.VerifyEqual(Entry.Value, AsInnerReadOnlyDictionary[Entry.Key] as IReadOnlyInner))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
