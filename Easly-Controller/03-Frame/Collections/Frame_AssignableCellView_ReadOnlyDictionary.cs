#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameAssignableCellViewReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IFrameAssignableCellView>, IEqualComparable
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewReadOnlyDictionary<TKey>
    {
        public FrameAssignableCellViewReadOnlyDictionary(IFrameAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameAssignableCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsAssignableCellViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!comparer.IsTrue(AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                IFrameAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as IFrameAssignableCellView;

                if (!comparer.IsTrue((Entry.Value != null && OtherValue != null) || (Entry.Value == null && OtherValue == null)))
                    return comparer.Failed();

                if (Entry.Value != null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, OtherValue))
                        return comparer.Failed();
                }
            }

            return true;
        }
        #endregion
    }
}
