using System.Collections.Generic;

namespace EaslyController.Focus
{
    /// <summary>
    /// Dictionary of ..., IxxxNodeSemantic
    /// </summary>
    public interface IFocusNodeSemanticDictionary<TKey> : IDictionary<TKey, IFocusNodeSemantic>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxNodeSemantic
    /// </summary>
    public class FocusNodeSemanticDictionary<TKey> : Dictionary<TKey, IFocusNodeSemantic>, IFocusNodeSemanticDictionary<TKey>
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusNodeSemanticDictionary{TKey}"/> object.
        /// </summary>
        public FocusNodeSemanticDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="FocusNodeSemanticDictionary{TKey}"/> object.
        /// </summary>
        public FocusNodeSemanticDictionary(IDictionary<TKey, IFocusNodeSemantic> dictionary)
            : base(dictionary)
        {
        }
    }
}
