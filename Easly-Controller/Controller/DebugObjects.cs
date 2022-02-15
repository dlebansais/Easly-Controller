namespace EaslyController
{
    using System;
    using System.Collections.Generic;
    using Contracts;

    /// <summary>
    /// Helper class for debugging.
    /// </summary>
    public class DebugObjects
    {
        /// <summary>
        /// Table of references to various objects.
        /// </summary>
        public static IDictionary<Type, object> ReferenceTable { get; } = new Dictionary<Type, object>();

        /// <summary>
        /// Adds or replaces a reference in <see cref="ReferenceTable"/>.
        /// </summary>
        /// <param name="reference">The added reference.</param>
        public static void AddReference(object reference)
        {
            Contract.RequireNotNull(reference, out object Reference);

            Type t = Reference.GetType();
            if (ReferenceTable.ContainsKey(t))
                ReferenceTable[t] = Reference;
            else
                ReferenceTable.Add(t, Reference);
        }

        /// <summary>
        /// Gets the first reference that supports interface <paramref name="t"/>.
        /// </summary>
        /// <param name="t">The interface type.</param>
        public static object GetReferenceByInterface(Type t)
        {
            // t = BaseNodeHelper.NodeTreeHelper.InterfaceTypeToNodeType(t);

            foreach (KeyValuePair<Type, object> Entry in ReferenceTable)
            {
                if (Entry.Key == t)
                    return Entry.Value;
            }

            return null;
        }
    }
}
