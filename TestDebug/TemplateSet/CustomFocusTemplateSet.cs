﻿using EaslyController.Focus;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CustomFocusTemplateSet
    {
        #region Init
#if !TRAVIS
        static CustomFocusTemplateSet()
        {
            IFocusTemplateReadOnlyDictionary FocusCustomNodeTemplates = LoadTemplate(FocusTemplateListString);
            IFocusTemplateReadOnlyDictionary FocusCustomBlockTemplates = LoadTemplate(FocusBlockTemplateString);
            FocusTemplateSet = new FocusTemplateSet(FocusCustomNodeTemplates, FocusCustomBlockTemplates);
        }

        private static IFocusTemplateReadOnlyDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                IFocusTemplateList Templates = XamlReader.Parse(s) as IFocusTemplateList;

                FocusTemplateDictionary TemplateDictionary = new FocusTemplateDictionary();
                foreach (IFocusTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FocusFrame.Root);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                IFocusTemplateReadOnlyDictionary Result = new FocusTemplateReadOnlyDictionary(TemplateDictionary);
                return Result;
            }
        }

        private CustomFocusTemplateSet()
        {
        }
#endif
        #endregion

        #region Properties
        public static IFocusTemplateSet FocusTemplateSet { get; private set; }
        #endregion

        #region Node Templates
        static string FocusTemplateListString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusNodeTemplate NodeType=""{xaml:Type IAssertion}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""Tag"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAttachment}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                </FocusKeywordFrame>
                <FocusKeywordFrame>as</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""AttachTypeBlocks"" IsNeverEmpty=""True""/>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IClass}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""CopySpecification"">
                    <FocusKeywordFrame>any</FocusKeywordFrame>
                    <FocusKeywordFrame>reference</FocusKeywordFrame>
                    <FocusKeywordFrame>value</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""Cloneable"">
                    <FocusKeywordFrame>cloneable</FocusKeywordFrame>
                    <FocusKeywordFrame>single</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""Comparable"">
                    <FocusKeywordFrame>comparable</FocusKeywordFrame>
                    <FocusKeywordFrame>incomparable</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""IsAbstract"">
                    <FocusKeywordFrame>instanceable</FocusKeywordFrame>
                    <FocusKeywordFrame>abstract</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame>class</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>import</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ImportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>generic</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""GenericBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""GenericBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ExportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>typedef</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""TypedefBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""TypedefBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>inheritance</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InheritanceBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InheritanceBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>discrete</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""DiscreteBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""DiscreteBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>replicate</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassReplicateBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassReplicateBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>feature</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""FeatureBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""FeatureBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>conversion</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ConversionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ConversionBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>invariant</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IClassReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ReplicateName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""PatternBlocks"" IsNeverEmpty=""True""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandOverload}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>parameter</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                        <FocusKeywordFrame>closed</FocusKeywordFrame>
                        <FocusKeywordFrame>open</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                    <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""CommandBody"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConditional}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                </FocusKeywordFrame>
                <FocusKeywordFrame>if</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""BooleanExpression""/>
                <FocusKeywordFrame>then</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConstraint}"">
        <FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ParentType"" />
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>rename</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RenameBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IContinuation}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>execute</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""Instructions"" />
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>cleanup</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""CleanupBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""CleanupBlocks"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDiscrete}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""NumericValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEntityDeclaration}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>:</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityType"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""DefaultValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExceptionHandler}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>catch</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ExceptionIdentifier"" />
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExport}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" IsNeverEmpty=""True""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExportChange}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGeneric}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>=</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""DefaultValue"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>conform to</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ConstraintBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ConstraintBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGlobalReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ReplicateName""/>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalListFrame PropertyName=""Patterns"" IsNeverEmpty=""True""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IImport}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Type"">
                    <FocusKeywordFrame>latest</FocusKeywordFrame>
                    <FocusKeywordFrame>strict</FocusKeywordFrame>
                    <FocusKeywordFrame>stable</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""LibraryIdentifier"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>rename</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RenameBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IInheritance}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Conformance"">
                    <FocusKeywordFrame>conformant</FocusKeywordFrame>
                    <FocusKeywordFrame>non-conformant</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""ParentType"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>rename</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RenameBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>forget</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ForgetBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ForgetBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>keep</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""KeepBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""KeepBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>discontinue</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""DiscontinueBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""DiscontinueBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>export</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExportChangeBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExportChangeBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusDiscreteFrame PropertyName=""ForgetIndexer"">
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>forget indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""KeepIndexer"">
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>keep indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""DiscontinueIndexer"">
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>discontinue indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame Text=""end"">
                </FocusKeywordFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ILibrary}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>library</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>import</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ImportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>class</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IName}"">
        <FocusTextValueFrame PropertyName=""Text""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPattern}"">
        <FocusTextValueFrame PropertyName=""Text""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQualifiedName}"">
        <FocusHorizontalListFrame PropertyName=""Path"" IsNeverEmpty=""True""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryOverload}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>parameter</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                        <FocusKeywordFrame>closed</FocusKeywordFrame>
                        <FocusKeywordFrame>open</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                    <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>result</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ResultBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ResultBlocks"" IsNeverEmpty=""True""/>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>modified</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""QueryBody"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>variant</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""Variant"" />
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>return</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ResultBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ResultBlocks"" IsNeverEmpty=""True""/>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRange}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"">
            </FocusSymbolFrame>
            <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>to</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRename}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""SourceIdentifier"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DestinationIdentifier"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRoot}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>libraries</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""LibraryBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""LibraryBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>classes</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>replicates</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""Replicates"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalListFrame PropertyName=""Replicates"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IScope}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>local</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>do</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InstructionBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ITypedef}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>is</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DefinedType"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalBlockListFrame PropertyName=""ParameterBlocks"" IsNeverEmpty=""True""/>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IWith}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>case</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""RangeBlocks"" IsNeverEmpty=""True""/>
                <FocusInsertFrame CollectionName=""RangeBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions""/>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDeferredBody}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>require</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RequireBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>throw</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>getter</FocusKeywordFrame>
                    <FocusKeywordFrame>deferred</FocusKeywordFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>ensure</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPositionalArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEffectiveBody}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>require</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RequireBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>throw</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>local</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>getter</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>exception</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>ensure</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExternBody}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>require</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RequireBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>throw</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>getter</FocusKeywordFrame>
                    <FocusKeywordFrame>extern</FocusKeywordFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>ensure</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorBody}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>require</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RequireBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>throw</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>getter</FocusKeywordFrame>
                    <FocusKeywordFrame>precursor</FocusKeywordFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>ensure</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAgentExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>agent</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""BaseType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Delegated"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssertionTagExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>tag</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""TagIdentifier"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IBinaryConditionalExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Conditional"">
                <FocusKeywordFrame>and</FocusKeywordFrame>
                <FocusKeywordFrame>or</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IBinaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Operator"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IClassConstantExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FocusPlaceholderFrame PropertyName=""ConstantIdentifier"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICloneOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Type"">
                <FocusKeywordFrame>shallow</FocusKeywordFrame>
                <FocusKeywordFrame>deep</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusKeywordFrame>clone of</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEntityExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>entity</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEqualityExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Comparison"">
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusKeywordFrame>!=</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusDiscreteFrame PropertyName=""Equality"">
                <FocusKeywordFrame>phys</FocusKeywordFrame>
                <FocusKeywordFrame>deep</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusKeywordFrame Text="" ""/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexQueryExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""IndexedExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" IsNeverEmpty=""True""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IInitializedObjectExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusInsertFrame CollectionName=""AssignmentBlocks"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordExpression}"">
        <FocusDiscreteFrame PropertyName=""Value"">
            <FocusKeywordFrame>True</FocusKeywordFrame>
            <FocusKeywordFrame>False</FocusKeywordFrame>
            <FocusKeywordFrame>Current</FocusKeywordFrame>
            <FocusKeywordFrame>Value</FocusKeywordFrame>
            <FocusKeywordFrame>Result</FocusKeywordFrame>
            <FocusKeywordFrame>Retry</FocusKeywordFrame>
            <FocusKeywordFrame>Exception</FocusKeywordFrame>
        </FocusDiscreteFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestCharacterExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>'</FocusKeywordFrame>
            <FocusCharacterFrame PropertyName=""Text""/>
            <FocusKeywordFrame>'</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestNumberExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusNumberFrame PropertyName=""Text""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestStringExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>""</FocusKeywordFrame>
            <FocusTextValueFrame PropertyName=""Text""/>
            <FocusKeywordFrame>""</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type INewExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>new</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Object"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IOldExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>old</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorIndexExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" IsNeverEmpty=""True""/>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPreprocessorExpression}"">
        <FocusDiscreteFrame PropertyName=""Value"">
            <FocusKeywordFrame>DateAndTime</FocusKeywordFrame>
            <FocusKeywordFrame>CompilationDiscreteIdentifier</FocusKeywordFrame>
            <FocusKeywordFrame>ClassPath</FocusKeywordFrame>
            <FocusKeywordFrame>CompilerVersion</FocusKeywordFrame>
            <FocusKeywordFrame>ConformanceToStandard</FocusKeywordFrame>
            <FocusKeywordFrame>DiscreteClassIdentifier</FocusKeywordFrame>
            <FocusKeywordFrame>Counter</FocusKeywordFrame>
            <FocusKeywordFrame>Debugging</FocusKeywordFrame>
            <FocusKeywordFrame>RandomInteger</FocusKeywordFrame>
        </FocusDiscreteFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Query"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IResultOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>result of</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IUnaryNotExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>not</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IUnaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Operator"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAttributeFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConstantFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ConstantValue""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICreationFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>creation</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks"" IsNeverEmpty=""True""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IFunctionFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>once per</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""Once"">
                        <FocusKeywordFrame>normal</FocusKeywordFrame>
                        <FocusKeywordFrame>object</FocusKeywordFrame>
                        <FocusKeywordFrame>processor</FocusKeywordFrame>
                        <FocusKeywordFrame>process</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                </FocusHorizontalPanelFrame>
                <FocusKeywordFrame>function</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks"" IsNeverEmpty=""True""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexerFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame>indexer</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""GetterBody"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""SetterBody"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IProcedureFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>procedure</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks"" IsNeverEmpty=""True""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPropertyFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>is</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                <FocusOptionalFrame PropertyName=""GetterBody""  />
                <FocusOptionalFrame PropertyName=""SetterBody"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIdentifier}"">
        <FocusTextValueFrame PropertyName=""Text""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAsLongAsInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>as long as</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ContinueCondition""/>
                <FocusInsertFrame CollectionName=""ContinuationBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ContinuationBlocks"" IsNeverEmpty=""True""/>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalBlockListFrame PropertyName=""DestinationBlocks"" IsNeverEmpty=""True""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAttachmentInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>attach</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusKeywordFrame>to</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""EntityNameBlocks"" IsNeverEmpty=""True""/>
                <FocusInsertFrame CollectionName=""AttachmentBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""AttachmentBlocks"" IsNeverEmpty=""True""/>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICheckInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>check</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Command"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICreateInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>create</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityIdentifier"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>with</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""CreationRoutineIdentifier"" />
                <FocusHorizontalPanelFrame>
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                    <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
                </FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>same processor as</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Processor"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDebugInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>debug</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IForLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusKeywordFrame>loop</FocusKeywordFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>local</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>init</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InitInstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>while</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""WhileCondition""/>
                    <FocusInsertFrame CollectionName=""LoopInstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" />
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>iterate</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IterationInstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>invariant</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>variant</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Variant"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIfThenElseInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalBlockListFrame PropertyName=""ConditionalBlocks"" IsNeverEmpty=""True""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>else</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""ElseInstructions"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Destination"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IInspectInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>inspect</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusInsertFrame CollectionName=""WithBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""WithBlocks"" IsNeverEmpty=""True""/>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Destination"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IOverLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>over</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""OverList"" />
                <FocusKeywordFrame>for each</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""IndexerBlocks"" IsNeverEmpty=""True""/>
                <FocusDiscreteFrame PropertyName=""Iteration"">
                    <FocusKeywordFrame>Single</FocusKeywordFrame>
                    <FocusKeywordFrame>Nested</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""LoopInstructions"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>exit if</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""ExitEntityName"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>invariant</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorIndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>from</FocusKeywordFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" IsNeverEmpty=""True""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>from</FocusKeywordFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRaiseEventInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>raise</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""QueryIdentifier"" />
            <FocusDiscreteFrame PropertyName=""Event"">
                <FocusKeywordFrame>once</FocusKeywordFrame>
                <FocusKeywordFrame>forever</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IReleaseInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>release</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IThrowInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>throw</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""ExceptionType"" />
            <FocusKeywordFrame>with</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""CreationRoutine"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAnchoredType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""AnchorKind"">
                <FocusKeywordFrame>declaration</FocusKeywordFrame>
                <FocusKeywordFrame>creation</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusPlaceholderFrame PropertyName=""AnchoredName"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IFunctionType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>function</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks"" IsNeverEmpty=""True""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGenericType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""TypeArgumentBlocks"" IsNeverEmpty=""True""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexerType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>indexer</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""EntityType""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" IsNeverEmpty=""True""/>
                </FocusVerticalPanelFrame>
                <FocusDiscreteFrame PropertyName=""IndexerKind"">
                    <FocusKeywordFrame>read-only</FocusKeywordFrame>
                    <FocusKeywordFrame>write-only</FocusKeywordFrame>
                    <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordAnchoredType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""Anchor"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IProcedureType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>procedure</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks"" IsNeverEmpty=""True""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPropertyType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>is</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""EntityType""/>
                </FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""PropertyKind"">
                    <FocusKeywordFrame>read-only</FocusKeywordFrame>
                    <FocusKeywordFrame>write-only</FocusKeywordFrame>
                    <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ISimpleType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ITupleType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>tuple</FocusKeywordFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" IsNeverEmpty=""True""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentTypeArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ParameterIdentifier"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPositionalTypeArgument}"">
        <FocusPlaceholderFrame PropertyName=""Source"" />
    </FocusNodeTemplate>
</FocusTemplateList>";
        #endregion

        #region Block Templates
        static string FocusBlockTemplateString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IArgument,Argument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAssertion,Assertion}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAssignmentArgument,AssignmentArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAttachment,Attachment}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IClass,Class}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IClassReplicate,ClassReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverload,CommandOverload}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverloadType,CommandOverloadType}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IConditional,Conditional}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IConstraint,Constraint}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IContinuation,Continuation}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IDiscrete,Discrete}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IEntityDeclaration,EntityDeclaration}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExceptionHandler,ExceptionHandler}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExport,Export}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExportChange,ExportChange}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IFeature,Feature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IGeneric,Generic}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IIdentifier,Identifier}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IImport,Import}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IInheritance,Inheritance}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IInstruction,Instruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ILibrary,Library}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IName,Name}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IObjectType,ObjectType}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IPattern,Pattern}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQualifiedName,QualifiedName}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverload,QueryOverload}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverloadType,QueryOverloadType}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IRange,Range}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IRename,Rename}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ITypeArgument,TypeArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ITypedef,Typedef}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IWith,With}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
</FocusTemplateList>
";
        #endregion
    }
}