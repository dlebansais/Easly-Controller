#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public interface IFocusTemplateDictionary : IFrameTemplateDictionary, IDictionary<Type, IFocusTemplate>
    {
        new int Count { get; }
        new Dictionary<Type, IFocusTemplate>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public class FocusTemplateDictionary : Dictionary<Type, IFocusTemplate>, IFocusTemplateDictionary
    {
        public FocusTemplateDictionary()
        {
        }

        public FocusTemplateDictionary(IDictionary<Type, IFocusTemplate> dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        void IDictionary<Type, IFrameTemplate>.Add(Type key, IFrameTemplate value) { Add((Type)key, (IFocusTemplate)value); }
        bool IDictionary<Type, IFrameTemplate>.Remove(Type key) { return Remove((Type)key); }
        bool IDictionary<Type, IFrameTemplate>.TryGetValue(Type key, out IFrameTemplate value)
        {
            bool Result = TryGetValue((Type)key, out IFocusTemplate Value);
            value = Value;
            return Result;
        }

        void ICollection<KeyValuePair<Type, IFrameTemplate>>.CopyTo(KeyValuePair<Type, IFrameTemplate>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<Type, IFocusTemplate> Entry in this)
                array[arrayIndex++] = new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value);
        }

        bool IDictionary<Type, IFrameTemplate>.ContainsKey(Type key) { return ContainsKey((Type)key); }
        void ICollection<KeyValuePair<Type, IFrameTemplate>>.Add(KeyValuePair<Type, IFrameTemplate> item) { Add((Type)item.Key, (IFocusTemplate)item.Value); }
        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.Contains(KeyValuePair<Type, IFrameTemplate> item) { return ContainsKey((Type)item.Key) && this[(Type)item.Key] == item.Value; }
        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.Remove(KeyValuePair<Type, IFrameTemplate> item) { return Remove((Type)item.Key); }
        IFrameTemplate IDictionary<Type, IFrameTemplate>.this[Type key] { get { return this[(Type)key]; } set { this[(Type)key] = (IFocusTemplate)value; } }
        ICollection<Type> IDictionary<Type, IFrameTemplate>.Keys { get { return new List<Type>(Keys); } }
        ICollection<IFrameTemplate> IDictionary<Type, IFrameTemplate>.Values { get { return new List<IFrameTemplate>(Values); } }

        IEnumerator<KeyValuePair<Type, IFrameTemplate>> IEnumerable<KeyValuePair<Type, IFrameTemplate>>.GetEnumerator()
        {
            List<KeyValuePair<Type, IFrameTemplate>> NewList = new List<KeyValuePair<Type, IFrameTemplate>>();
            IEnumerator<KeyValuePair<Type, IFocusTemplate>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<Type, IFocusTemplate> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<Type, IFrameTemplate>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        bool ICollection<KeyValuePair<Type, IFrameTemplate>>.IsReadOnly { get { return ((ICollection<KeyValuePair<Type, IFocusTemplate>>)this).IsReadOnly; } }
        #endregion
    }
}
