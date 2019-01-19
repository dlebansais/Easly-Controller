using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IFrameAssignableCellViewReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IFrameAssignableCellView>, IEqualComparable
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class FrameAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewReadOnlyDictionary<TKey>
    {
        public FrameAssignableCellViewReadOnlyDictionary(IFrameAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameAssignableCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return false;

            if (Count != AsAssignableCellViewReadOnlyDictionary.Count)
                return false;

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key))
                    return false;

                IFrameAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as IFrameAssignableCellView;

                if (Entry.Value != null && OtherValue != null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, OtherValue))
                        return false;
                }
                else if (Entry.Value != null || OtherValue != null)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
