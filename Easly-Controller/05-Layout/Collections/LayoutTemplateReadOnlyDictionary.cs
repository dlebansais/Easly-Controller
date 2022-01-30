namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutTemplateReadOnlyDictionary : FocusTemplateReadOnlyDictionary, ICollection<KeyValuePair<System.Type, ILayoutTemplate>>, IEnumerable<KeyValuePair<System.Type, ILayoutTemplate>>, IDictionary<System.Type, ILayoutTemplate>, IReadOnlyCollection<KeyValuePair<System.Type, ILayoutTemplate>>, IReadOnlyDictionary<System.Type, ILayoutTemplate>
    {
        /// <inheritdoc/>
        public LayoutTemplateReadOnlyDictionary(LayoutTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region System.Type, ILayoutTemplate
        void ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.Add(KeyValuePair<System.Type, ILayoutTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.Contains(KeyValuePair<System.Type, ILayoutTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.CopyTo(KeyValuePair<System.Type, ILayoutTemplate>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<System.Type, IFrameTemplate> Entry in this) array[i++] = new KeyValuePair<System.Type, ILayoutTemplate>((System.Type)Entry.Key, (ILayoutTemplate)Entry.Value); }
        bool ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.Remove(KeyValuePair<System.Type, ILayoutTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<System.Type, ILayoutTemplate>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<System.Type, ILayoutTemplate>> IEnumerable<KeyValuePair<System.Type, ILayoutTemplate>>.GetEnumerator() { IEnumerator<KeyValuePair<System.Type, IFrameTemplate>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<System.Type, ILayoutTemplate>((System.Type)iterator.Current.Key, (ILayoutTemplate)iterator.Current.Value); } }

        ILayoutTemplate IDictionary<System.Type, ILayoutTemplate>.this[System.Type key] { get { return (ILayoutTemplate)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<System.Type> IDictionary<System.Type, ILayoutTemplate>.Keys { get { List<System.Type> Result = new(); foreach (KeyValuePair<System.Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<System.Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutTemplate> IDictionary<System.Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<System.Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<System.Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<System.Type, ILayoutTemplate>.Add(System.Type key, ILayoutTemplate value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<System.Type, ILayoutTemplate>.ContainsKey(System.Type key) { return ContainsKey(key); }
        bool IDictionary<System.Type, ILayoutTemplate>.Remove(System.Type key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<System.Type, ILayoutTemplate>.TryGetValue(System.Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }

        ILayoutTemplate IReadOnlyDictionary<System.Type, ILayoutTemplate>.this[System.Type key] { get { return (ILayoutTemplate)this[key]; } }
        IEnumerable<System.Type> IReadOnlyDictionary<System.Type, ILayoutTemplate>.Keys { get { List<System.Type> Result = new(); foreach (KeyValuePair<System.Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<System.Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutTemplate> IReadOnlyDictionary<System.Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<System.Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<System.Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<System.Type, ILayoutTemplate>.ContainsKey(System.Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<System.Type, ILayoutTemplate>.TryGetValue(System.Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }
        #endregion
    }
}
