namespace EaslyController.Xaml
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Markup;
    using BaseNode;

    /// <summary>
    /// Markup extension to declare types in Xaml.
    ///
    /// Xaml syntax:
    ///   xmlns:xaml="clr-namespace:EaslyController.Xaml;assembly=Easly-Controller"
    /// For INode:
    ///   MyProperty="{xaml:Type INode}"
    /// For IBlock{IIdentifier,Identifier}:
    ///   MyProperty="{xaml:Type IBlock,IIdentifier,Identifier}"
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
            Debug.Assert(typeName != null);

            TypeName = typeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// Creates a type with one generic argument.
        /// </summary>
        /// <param name="typeName">Type name, without namespace.</param>
        /// <param name="arg1">Type argument name.</param>
        public Type(string typeName, string arg1)
        {
            Debug.Assert(typeName != null);
            Debug.Assert(arg1 != null);

            TypeName = typeName;
            Arg1 = arg1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// Creates a type with two generic arguments.
        /// </summary>
        /// <param name="typeName">Type name, without namespace.</param>
        /// <param name="arg1">Type argument name #1.</param>
        /// <param name="arg2">Type argument name #2.</param>
        public Type(string typeName, string arg1, string arg2)
        {
            Debug.Assert(typeName != null);
            Debug.Assert(arg1 != null);
            Debug.Assert(arg2 != null);

            TypeName = typeName;
            Arg1 = arg1;
            Arg2 = arg2;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Debug.Assert(serviceProvider != null);
            Debug.Assert(TypeName != null);
            Debug.Assert((Arg1 == null && Arg2 == null) || (Arg1 != null && Arg2 == null) || (Arg1 != null && Arg2 != null));

            IXamlTypeResolver XamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;

            System.Type Type = null;

            if (Arg1 == null && Arg2 == null)
                Type = XamlTypeResolver.Resolve(TypeName);

            else if (Arg1 != null && Arg2 == null)
            {
                System.Type Arg1Type;
                System.Type GenericDefinitionType;

                Arg1Type = XamlTypeResolver.Resolve(Arg1);
                GenericDefinitionType = XamlTypeResolver.Resolve($"{TypeName}`1");

                System.Type[] GenericArguments = new System.Type[] { Arg1Type };
                Type = GenericDefinitionType.MakeGenericType(GenericArguments);
            }
            else if (Arg1 != null && Arg1 != null)
            {
                System.Type Arg1Type;
                System.Type Arg2Type;
                System.Type GenericDefinitionType;

                Arg1Type = XamlTypeResolver.Resolve(Arg1);
                Arg2Type = XamlTypeResolver.Resolve(Arg2);

                GenericDefinitionType = XamlTypeResolver.Resolve(TypeName);
                Assembly GenericDefinitionAssembly = GenericDefinitionType.Assembly;
                GenericDefinitionType = GenericDefinitionAssembly.GetType(ToFullNameWithArguments(GenericDefinitionType.Name, 2));

                System.Type[] GenericArguments = new System.Type[] { Arg1Type, Arg2Type };
                Type = GenericDefinitionType.MakeGenericType(GenericArguments);
            }

            return Type;
        }

        /// <summary>
        /// Type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Type argument name #1. Can be null if no generic argument.
        /// </summary>
        public string Arg1 { get; set; }

        /// <summary>
        /// Type argument name #2. Can be null if none or just one generic argument.
        /// </summary>
        public string Arg2 { get; set; }

        private string ToFullName(string name)
        {
            return $"BaseNode.{name}";
        }

        private string ToFullNameWithArguments(string name, int argCount)
        {
            return $"BaseNode.{name}`{argCount}";
        }
    }
}
