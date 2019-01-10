using EaslyController.Frame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
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
        public new IFrameTemplate this[Type key] { get { return base[key]; } }
        public new IEnumerable<Type> Keys { get { return base.Keys; } }
        public new IEnumerable<IFrameTemplate> Values { get { return base.Values; } }

        public new IEnumerator<KeyValuePair<Type, IFrameTemplate>> GetEnumerator()
        {
            List<KeyValuePair<Type, IFrameTemplate>> NewList = new List<KeyValuePair<Type, IFrameTemplate>>();
            foreach (KeyValuePair<Type, IFocusTemplate> Entry in Dictionary)
                NewList.Add(new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(Type key, out IFrameTemplate value) { bool Result = TryGetValue(key, out IFocusTemplate Value); value = Value; return Result; }
        #endregion
    }
}
