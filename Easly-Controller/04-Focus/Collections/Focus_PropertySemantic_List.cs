using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxPropertySemantic
    /// </summary>
    public interface IFocusPropertySemanticList : IList<IFocusPropertySemantic>, IReadOnlyList<IFocusPropertySemantic>
    {
        new int Count { get; }
        new IFocusPropertySemantic this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxPropertySemantic
    /// </summary>
    public class FocusPropertySemanticList : Collection<IFocusPropertySemantic>, IFocusPropertySemanticList
    {
    }
}
