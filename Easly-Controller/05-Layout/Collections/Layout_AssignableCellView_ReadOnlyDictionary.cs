#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Read-only dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface ILayoutAssignableCellViewReadOnlyDictionary<TKey> : IFocusAssignableCellViewReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, ILayoutAssignableCellView>, IEqualComparable
    {
        new ILayoutAssignableCellView this[TKey key] { get; }
        new int Count { get; }
        new bool ContainsKey(TKey key);
        new IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class LayoutAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, ILayoutAssignableCellView>, ILayoutAssignableCellViewReadOnlyDictionary<TKey>
    {
        public LayoutAssignableCellViewReadOnlyDictionary(ILayoutAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        IFrameAssignableCellView IReadOnlyDictionary<TKey, IFrameAssignableCellView>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFrameAssignableCellView>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<TKey, IFrameAssignableCellView>.TryGetValue(TKey key, out IFrameAssignableCellView value)
        {
            bool Result = TryGetValue(key, out ILayoutAssignableCellView Value);
            value = Value;
            return Result;
        }

        IEnumerable<IFrameAssignableCellView> IReadOnlyDictionary<TKey, IFrameAssignableCellView>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFrameAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        IFocusAssignableCellView IFocusAssignableCellViewReadOnlyDictionary<TKey>.this[TKey key] { get { return this[key]; } }

        IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> IFocusAssignableCellViewReadOnlyDictionary<TKey>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFocusAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFocusAssignableCellView>>();
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }

        IFocusAssignableCellView IReadOnlyDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value)
        {
            bool Result = TryGetValue(key, out ILayoutAssignableCellView Value);
            value = Value;
            return Result;
        }

        IEnumerable<IFocusAssignableCellView> IReadOnlyDictionary<TKey, IFocusAssignableCellView>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFocusAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFocusAssignableCellView>>();
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutAssignableCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsAssignableCellViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!comparer.IsTrue(AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                ILayoutAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as ILayoutAssignableCellView;

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
