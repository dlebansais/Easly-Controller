#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <summary>
    /// Read-only dictionary of ..., IxxxTemplate
    /// </summary>
    public interface IFocusTemplateReadOnlyDictionary : IFrameTemplateReadOnlyDictionary, IReadOnlyDictionary<Type, IFocusTemplate>
    {
        new int Count { get; }
        new IFocusTemplate this[Type key] { get; }
        new IEnumerator<KeyValuePair<Type, IFocusTemplate>> GetEnumerator();
        new bool ContainsKey(Type key);
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxTemplate
    /// </summary>
    public class FocusTemplateReadOnlyDictionary : ReadOnlyDictionary<Type, IFocusTemplate>, IFocusTemplateReadOnlyDictionary
    {
        public FocusTemplateReadOnlyDictionary(IFocusTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        IFrameTemplate IReadOnlyDictionary<Type, IFrameTemplate>.this[Type key] { get { return this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFrameTemplate>.Keys { get { return Keys; } }
        IEnumerable<IFrameTemplate> IReadOnlyDictionary<Type, IFrameTemplate>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<Type, IFrameTemplate>> IEnumerable<KeyValuePair<Type, IFrameTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFrameTemplate>> NewList = new List<KeyValuePair<Type, IFrameTemplate>>();
            foreach (KeyValuePair<Type, IFocusTemplate> Entry in Dictionary)
                NewList.Add(new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }

        bool IReadOnlyDictionary<Type, IFrameTemplate>.TryGetValue(Type key, out IFrameTemplate value)
        {
            bool Result = TryGetValue(key, out IFocusTemplate Value);
            value = Value;
            return Result;
        }
        #endregion
    }
}
