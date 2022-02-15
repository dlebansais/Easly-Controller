namespace EaslyController.Xaml
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Markup;
    using Contracts;

    /// <summary>
    /// Markup extension to declare types in Xaml.
    ///
    /// Xaml syntax:
    ///   xmlns:xaml="clr-namespace:EaslyController.Xaml;assembly=Easly-Controller"
    /// For Node:
    ///   MyProperty="{xaml:Type Node}"
    /// For IBlock{Identifier}:
    ///   MyProperty="{xaml:Type IBlock,Identifier}"
    /// </summary>
    [ContentProperty("TypeName")]
    public class Type : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// Creates a type with no generic argument.
        /// </summary>
        /// <param name="typeName">Type name, without namespace.</param>
        public Type(string typeName)
        {
            Contract.RequireNotNull(typeName, out string TypeName);

            this.TypeName = TypeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// Creates a type with one generic argument.
        /// </summary>
        /// <param name="typeName">Type name, without namespace.</param>
        /// <param name="arg1">Type argument name.</param>
        public Type(string typeName, string arg1)
        {
            Contract.RequireNotNull(typeName, out string TypeName);
            Contract.RequireNotNull(arg1, out string Arg1);

            this.TypeName = TypeName;
            this.Arg1 = Arg1;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Contract.RequireNotNull(serviceProvider, out IServiceProvider ServiceProvider);

            Debug.Assert(TypeName != null);

            IXamlTypeResolver XamlTypeResolver = ServiceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;

            System.Type Type = null;

            if (Arg1 == null)
                Type = XamlTypeResolver.Resolve(TypeName);
            else
            {
                System.Type Arg1Type;
                System.Type GenericDefinitionType;

                Arg1Type = XamlTypeResolver.Resolve(Arg1);

                GenericDefinitionType = XamlTypeResolver.Resolve(TypeName);
                Assembly GenericDefinitionAssembly = GenericDefinitionType.Assembly;
                GenericDefinitionType = GenericDefinitionAssembly.GetType(ToFullNameWithArguments(GenericDefinitionType.Name, 1));

                System.Type[] GenericArguments = new System.Type[] { Arg1Type };
                Type = GenericDefinitionType.MakeGenericType(GenericArguments);
            }

            return Type;
        }

        private string ToFullNameWithArguments(string name, int argCount)
        {
            return $"BaseNode.{name}`{argCount}";
        }

        /// <summary>
        /// Type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Type argument name #1. Can be null if no generic argument.
        /// </summary>
        public string Arg1 { get; set; }
    }
}
