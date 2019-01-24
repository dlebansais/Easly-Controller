using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of IxxxBlockState, IxxxBlockStateView
    /// </summary>
    public interface IReadOnlyBlockStateViewDictionary : IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>, IEqualComparable
    {
    }

    /// <summary>
    /// Dictionary of IxxxBlockState, IxxxBlockStateView
    /// </summary>
    public class ReadOnlyBlockStateViewDictionary : Dictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>, IReadOnlyBlockStateViewDictionary
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (Count != AsBlockStateViewDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> Entry in this)
            {
                if (!AsBlockStateViewDictionary.ContainsKey(Entry.Key))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsBlockStateViewDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
