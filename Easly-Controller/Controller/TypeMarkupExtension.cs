using System;
using System.Windows.Markup;

namespace EaslyController.Xaml
{
    [ContentProperty("TypeName")]
    public class Type : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //TODO; properly parse short and generic type names.
            return System.Type.GetType(TypeName);
        }

        public string TypeName { get; set; }
    }
}
