namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusTemplateReadOnlyDictionary : FrameTemplateReadOnlyDictionary, ICollection<KeyValuePair<System.Type, IFocusTemplate>>, IEnumerable<KeyValuePair<System.Type, IFocusTemplate>>, IDictionary<System.Type, IFocusTemplate>, IReadOnlyCollection<KeyValuePair<System.Type, IFocusTemplate>>, IReadOnlyDictionary<System.Type, IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateReadOnlyDictionary(FocusTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region System.Type, IFocusTemplate
        void ICollection<KeyValuePair<System.Type, IFocusTemplate>>.Add(KeyValuePair<System.Type, IFocusTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<System.Type, IFocusTemplate>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<System.Type, IFocusTemplate>>.Contains(KeyValuePair<System.Type, IFocusTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<System.Type, IFocusTemplate>>.CopyTo(KeyValuePair<System.Type, IFocusTemplate>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<System.Type, IFrameTemplate> Entry in this) array[i++] = new KeyValuePair<System.Type, IFocusTemplate>((System.Type)Entry.Key, (IFocusTemplate)Entry.Value); }
        bool ICollection<KeyValuePair<System.Type, IFocusTemplate>>.Remove(KeyValuePair<System.Type, IFocusTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<System.Type, IFocusTemplate>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<System.Type, IFocusTemplate>> IEnumerable<KeyValuePair<System.Type, IFocusTemplate>>.GetEnumerator() { IEnumerator<KeyValuePair<System.Type, IFrameTemplate>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<System.Type, IFocusTemplate>((System.Type)iterator.Current.Key, (IFocusTemplate)iterator.Current.Value); } }

        IFocusTemplate IDictionary<System.Type, IFocusTemplate>.this[System.Type key] { get { return (IFocusTemplate)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<System.Type> IDictionary<System.Type, IFocusTemplate>.Keys { get { List<System.Type> Result = new(); foreach (KeyValuePair<System.Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<System.Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusTemplate> IDictionary<System.Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<System.Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<System.Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<System.Type, IFocusTemplate>.Add(System.Type key, IFocusTemplate value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<System.Type, IFocusTemplate>.ContainsKey(System.Type key) { return ContainsKey(key); }
        bool IDictionary<System.Type, IFocusTemplate>.Remove(System.Type key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<System.Type, IFocusTemplate>.TryGetValue(System.Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }

        IFocusTemplate IReadOnlyDictionary<System.Type, IFocusTemplate>.this[System.Type key] { get { return (IFocusTemplate)this[key]; } }
        IEnumerable<System.Type> IReadOnlyDictionary<System.Type, IFocusTemplate>.Keys { get { List<System.Type> Result = new(); foreach (KeyValuePair<System.Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<System.Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusTemplate> IReadOnlyDictionary<System.Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<System.Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<System.Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<System.Type, IFocusTemplate>.ContainsKey(System.Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<System.Type, IFocusTemplate>.TryGetValue(System.Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }
        #endregion
    }
}
