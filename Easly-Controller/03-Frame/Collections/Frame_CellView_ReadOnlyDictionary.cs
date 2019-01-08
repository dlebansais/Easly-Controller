using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IFrameCellViewReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IFrameCellView>, IEqualComparable
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class FrameCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameCellView>, IFrameCellViewReadOnlyDictionary<TKey>
    {
        public FrameCellViewReadOnlyDictionary(IFrameCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellViewReadOnlyDictionary<TKey> AsCellViewReadOnlyDictionary))
                return false;

            if (Count != AsCellViewReadOnlyDictionary.Count)
                return false;

            foreach (KeyValuePair<TKey, IFrameCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!AsCellViewReadOnlyDictionary.ContainsKey(Entry.Key))
                    return false;

                Debug.Assert(Entry.Value != null);
                Debug.Assert(AsCellViewReadOnlyDictionary[Entry.Key] != null);

                if (!comparer.VerifyEqual(Entry.Value, AsCellViewReadOnlyDictionary[Entry.Key] as IFrameCellView))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
