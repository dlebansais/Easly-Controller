namespace EaslyController.Xaml
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using IServiceProvider = System.IServiceProvider;
    using Contracts;
    using NotNullReflection;

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
            Arg1 = string.Empty;
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

            IXamlTypeResolver XamlTypeResolver = (IXamlTypeResolver)ServiceProvider.GetService(typeof(IXamlTypeResolver));

            NotNullReflection.Type Type;

            if (Arg1.Length == 0)
            {
                Type = (NotNullReflection.Type)NotNullReflection.Type.CreateNew(XamlTypeResolver.Resolve(TypeName));
            }
            else
            {
                NotNullReflection.Type GenericDefinitionType;

                GenericDefinitionType = (NotNullReflection.Type)NotNullReflection.Type.CreateNew(XamlTypeResolver.Resolve(TypeName));
                Assembly GenericDefinitionAssembly = GenericDefinitionType.Assembly;
                GenericDefinitionType = GenericDefinitionAssembly.GetType(ToFullNameWithArguments(GenericDefinitionType.Name, 1))!;

                NotNullReflection.Type[] GenericArguments = new NotNullReflection.Type[] { (NotNullReflection.Type)NotNullReflection.Type.CreateNew(XamlTypeResolver.Resolve(Arg1)) };
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
        /// Type argument name #1. Can be <see cref="string.Empty"/> if no generic argument.
        /// </summary>
        public string Arg1 { get; set; }
    }
}
