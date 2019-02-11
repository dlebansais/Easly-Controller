#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Read-only dictionary of ..., IxxxTemplate
    /// </summary>
    public interface ILayoutTemplateReadOnlyDictionary : IFocusTemplateReadOnlyDictionary, IReadOnlyDictionary<Type, ILayoutTemplate>
    {
        new ILayoutTemplate this[Type key] { get; }
        new int Count { get; }
        new bool ContainsKey(Type key);
        new IEnumerator<KeyValuePair<Type, ILayoutTemplate>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxTemplate
    /// </summary>
    public class LayoutTemplateReadOnlyDictionary : ReadOnlyDictionary<Type, ILayoutTemplate>, ILayoutTemplateReadOnlyDictionary
    {
        public LayoutTemplateReadOnlyDictionary(ILayoutTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        IFrameTemplate IReadOnlyDictionary<Type, IFrameTemplate>.this[Type key] { get { return this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFrameTemplate>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<Type, IFrameTemplate>.TryGetValue(Type key, out IFrameTemplate value)
        {
            bool Result = TryGetValue(key, out ILayoutTemplate Value);
            value = Value;
            return Result;
        }

        IEnumerable<IFrameTemplate> IReadOnlyDictionary<Type, IFrameTemplate>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<Type, IFrameTemplate>> IEnumerable<KeyValuePair<Type, IFrameTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFrameTemplate>> NewList = new List<KeyValuePair<Type, IFrameTemplate>>();
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in Dictionary)
                NewList.Add(new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        IFocusTemplate IFocusTemplateReadOnlyDictionary.this[Type key] { get { return this[key]; } }

        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IFocusTemplateReadOnlyDictionary.GetEnumerator()
        {
            List<KeyValuePair<Type, IFocusTemplate>> NewList = new List<KeyValuePair<Type, IFocusTemplate>>();
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in Dictionary)
                NewList.Add(new KeyValuePair<Type, IFocusTemplate>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }

        IFocusTemplate IReadOnlyDictionary<Type, IFocusTemplate>.this[Type key] { get { return this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFocusTemplate>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value)
        {
            bool Result = TryGetValue(key, out ILayoutTemplate Value);
            value = Value;
            return Result;
        }

        IEnumerable<IFocusTemplate> IReadOnlyDictionary<Type, IFocusTemplate>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IEnumerable<KeyValuePair<Type, IFocusTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFocusTemplate>> NewList = new List<KeyValuePair<Type, IFocusTemplate>>();
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in Dictionary)
                NewList.Add(new KeyValuePair<Type, IFocusTemplate>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
