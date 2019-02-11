#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public interface ILayoutTemplateDictionary : IFocusTemplateDictionary, IDictionary<Type, ILayoutTemplate>
    {
        new ILayoutTemplate this[Type key] { get; set; }
        new int Count { get; }
        new Dictionary<Type, ILayoutTemplate>.Enumerator GetEnumerator();
        new bool ContainsKey(Type key);
    }

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public class LayoutTemplateDictionary : Dictionary<Type, ILayoutTemplate>, ILayoutTemplateDictionary
    {
        public LayoutTemplateDictionary()
        {
        }

        public LayoutTemplateDictionary(IDictionary<Type, ILayoutTemplate> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IFrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new LayoutTemplateReadOnlyDictionary(this);
        }

        #region Frame
        IFrameTemplate IDictionary<Type, IFrameTemplate>.this[Type key] { get { return this[key]; } set { this[key] = (ILayoutTemplate)value; } }
        void IDictionary<Type, IFrameTemplate>.Add(Type key, IFrameTemplate value) { Add(key, (ILayoutTemplate)value); }
        bool IDictionary<Type, IFrameTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        ICollection<Type> IDictionary<Type, IFrameTemplate>.Keys { get { return new List<Type>(Keys); } }
        bool IDictionary<Type, IFrameTemplate>.Remove(Type key) { return Remove(key); }

        bool IDictionary<Type, IFrameTemplate>.TryGetValue(Type key, out IFrameTemplate value)
        {
            bool Result = TryGetValue(key, out ILayoutTemplate Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameTemplate> IDictionary<Type, IFrameTemplate>.Values { get { return new List<IFrameTemplate>(Values); } }
        void ICollection<KeyValuePair<Type, IFrameTemplate>>.Add(KeyValuePair<Type, IFrameTemplate> item) { Add(item.Key, (ILayoutTemplate)item.Value); }
        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.Contains(KeyValuePair<Type, IFrameTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }

        void ICollection<KeyValuePair<Type, IFrameTemplate>>.CopyTo(KeyValuePair<Type, IFrameTemplate>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in this)
                array[arrayIndex++] = new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.IsReadOnly { get { return ((ICollection<KeyValuePair<Type, ILayoutTemplate>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.Remove(KeyValuePair<Type, IFrameTemplate> item) { return Remove(item.Key); }

        IEnumerator<KeyValuePair<Type, IFrameTemplate>> IEnumerable<KeyValuePair<Type, IFrameTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFrameTemplate>> NewList = new List<KeyValuePair<Type, IFrameTemplate>>();
            IEnumerator<KeyValuePair<Type, ILayoutTemplate>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<Type, ILayoutTemplate> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        IFocusTemplate IFocusTemplateDictionary.this[Type key] { get { return this[key]; } set { this[key] = (ILayoutTemplate)value; } }

        Dictionary<Type, IFocusTemplate>.Enumerator IFocusTemplateDictionary.GetEnumerator()
        {
            Dictionary<Type, IFocusTemplate> NewDictionary = new Dictionary<Type, IFocusTemplate>();
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in this)
                NewDictionary.Add(Entry.Key, Entry.Value);

            return NewDictionary.GetEnumerator();
        }

        IFocusTemplate IDictionary<Type, IFocusTemplate>.this[Type key] { get { return this[key]; } set { this[key] = (ILayoutTemplate)value; } }
        void IDictionary<Type, IFocusTemplate>.Add(Type key, IFocusTemplate value) { Add(key, (ILayoutTemplate)value); }
        bool IDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        ICollection<Type> IDictionary<Type, IFocusTemplate>.Keys { get { return new List<Type>(Keys); } }
        bool IDictionary<Type, IFocusTemplate>.Remove(Type key) { return Remove(key); }

        bool IDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value)
        {
            bool Result = TryGetValue(key, out ILayoutTemplate Value);
            value = Value;
            return Result;
        }

        ICollection<IFocusTemplate> IDictionary<Type, IFocusTemplate>.Values { get { return new List<IFocusTemplate>(Values); } }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Add(KeyValuePair<Type, IFocusTemplate> item) { Add(item.Key, (ILayoutTemplate)item.Value); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Contains(KeyValuePair<Type, IFocusTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }

        void ICollection<KeyValuePair<Type, IFocusTemplate>>.CopyTo(KeyValuePair<Type, IFocusTemplate>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<Type, ILayoutTemplate> Entry in this)
                array[arrayIndex++] = new KeyValuePair<Type, IFocusTemplate>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.IsReadOnly { get { return ((ICollection<KeyValuePair<Type, ILayoutTemplate>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Remove(KeyValuePair<Type, IFocusTemplate> item) { return Remove(item.Key); }

        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IEnumerable<KeyValuePair<Type, IFocusTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFocusTemplate>> NewList = new List<KeyValuePair<Type, IFocusTemplate>>();
            IEnumerator<KeyValuePair<Type, ILayoutTemplate>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<Type, ILayoutTemplate> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<Type, IFocusTemplate>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
