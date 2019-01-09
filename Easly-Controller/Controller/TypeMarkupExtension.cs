using BaseNode;
using System;
using System.Reflection;
using System.Windows.Markup;

namespace EaslyController.Xaml
{
    [ContentProperty("TypeName")]
    public class Type : MarkupExtension
    {
        public Type(string typeName)
        {
            TypeName = typeName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //TODO; properly parse short and generic type names.

            Assembly EaslyAssembly = typeof(INode).Assembly;
            System.Type Type = EaslyAssembly.GetType("BaseNode." + TypeName);
            return Type;
        }

        public string TypeName { get; set; }
    }
}
