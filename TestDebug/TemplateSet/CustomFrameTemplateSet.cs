using EaslyController.Frame;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CustomFrameTemplateSet
    {
        #region Init
        static CustomFrameTemplateSet()
        {
            IFrameTemplateReadOnlyDictionary FrameCustomNodeTemplates = LoadTemplate(FrameTemplateListString);
            IFrameTemplateReadOnlyDictionary FrameCustomBlockTemplates = LoadTemplate(FrameBlockTemplateString);
            FrameTemplateSet = new FrameTemplateSet(FrameCustomNodeTemplates, FrameCustomBlockTemplates);
        }

        private static IFrameTemplateReadOnlyDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                IFrameTemplateList Templates = XamlReader.Parse(s) as IFrameTemplateList;

                FrameTemplateDictionary TemplateDictionary = new FrameTemplateDictionary();
                foreach (IFrameTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FrameFrame.Root);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                IFrameTemplateReadOnlyDictionary Result = new FrameTemplateReadOnlyDictionary(TemplateDictionary);
                return Result;
            }
        }

        public static IFrameTemplateSet FrameTemplateSet { get; private set; }

        private CustomFrameTemplateSet()
        {
        }
        #endregion

        #region Node Templates
        static string FrameTemplateListString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameNodeTemplate NodeType=""{xaml:Type IAssertion}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""Tag"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttachment}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame Text=""else"">
                </FrameKeywordFrame>
                <FrameKeywordFrame>as</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""AttachTypeBlocks"" />
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClass}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""CopySpecification"">
                    <FrameKeywordFrame>any</FrameKeywordFrame>
                    <FrameKeywordFrame>reference</FrameKeywordFrame>
                    <FrameKeywordFrame>value</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""Cloneable"">
                    <FrameKeywordFrame>cloneable</FrameKeywordFrame>
                    <FrameKeywordFrame>single</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""Comparable"">
                    <FrameKeywordFrame>comparable</FrameKeywordFrame>
                    <FrameKeywordFrame>incomparable</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""IsAbstract"">
                    <FrameKeywordFrame>instanceable</FrameKeywordFrame>
                    <FrameKeywordFrame>abstract</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame>class</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityName""/>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>import</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>generic</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""GenericBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""GenericBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ExportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>typedef</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""TypedefBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""TypedefBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>inheritance</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InheritanceBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InheritanceBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>discrete</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""DiscreteBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""DiscreteBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>replicate</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassReplicateBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassReplicateBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>feature</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""FeatureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""FeatureBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>conversion</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ConversionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ConversionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>invariant</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""PatternBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>parameter</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                        <FrameKeywordFrame>closed</FrameKeywordFrame>
                        <FrameKeywordFrame>open</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                    <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""CommandBody"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConditional}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame Text=""else"">
                </FrameKeywordFrame>
                <FrameKeywordFrame>if</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""BooleanExpression""/>
                <FrameKeywordFrame>then</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConstraint}"">
        <FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParentType"" />
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>rename</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IContinuation}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>execute</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""Instructions"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>cleanup</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""CleanupBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""CleanupBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDiscrete}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""NumericValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEntityDeclaration}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>:</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityType"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""DefaultValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExceptionHandler}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>catch</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ExceptionIdentifier"" />
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExport}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExportChange}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGeneric}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>=</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""DefaultValue"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>conform to</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ConstraintBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ConstraintBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGlobalReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalListFrame PropertyName=""Patterns"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IImport}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Type"">
                    <FrameKeywordFrame>latest</FrameKeywordFrame>
                    <FrameKeywordFrame>strict</FrameKeywordFrame>
                    <FrameKeywordFrame>stable</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""LibraryIdentifier"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>rename</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInheritance}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Conformance"">
                    <FrameKeywordFrame>conformant</FrameKeywordFrame>
                    <FrameKeywordFrame>non-conformant</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""ParentType"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>rename</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RenameBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>forget</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ForgetBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ForgetBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>keep</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""KeepBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""KeepBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>discontinue</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""DiscontinueBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""DiscontinueBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>export</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExportChangeBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExportChangeBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameDiscreteFrame PropertyName=""ForgetIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>forget indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""KeepIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>keep indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""DiscontinueIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>discontinue indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame Text=""end"">
                </FrameKeywordFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ILibrary}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>library</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityName""/>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>import</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>class</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IName}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPattern}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQualifiedName}"">
        <FrameHorizontalListFrame PropertyName=""Path"" />
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>parameter</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                        <FrameKeywordFrame>closed</FrameKeywordFrame>
                        <FrameKeywordFrame>open</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                    <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>result</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ResultBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ResultBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>modified</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""QueryBody"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>variant</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""Variant"" />
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>return</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ResultBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ResultBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRange}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"">
            </FrameSymbolFrame>
            <FramePlaceholderFrame PropertyName=""LeftExpression"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>to</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRename}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""SourceIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DestinationIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRoot}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>libraries</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""LibraryBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""LibraryBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>classes</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>replicates</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""Replicates"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalListFrame PropertyName=""Replicates"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IScope}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>local</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>do</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InstructionBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ITypedef}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>is</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DefinedType"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ParameterBlocks""/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IWith}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>case</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""RangeBlocks""/>
                <FrameInsertFrame CollectionName=""RangeBlocks""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>deferred</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPositionalArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>local</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""BodyInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>exception</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>extern</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>precursor</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAgentExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>agent</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""BaseType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Delegated"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssertionTagExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>tag</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""TagIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IBinaryConditionalExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Conditional"">
                <FrameKeywordFrame>and</FrameKeywordFrame>
                <FrameKeywordFrame>or</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IBinaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClassConstantExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FramePlaceholderFrame PropertyName=""ConstantIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICloneOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Type"">
                <FrameKeywordFrame>shallow</FrameKeywordFrame>
                <FrameKeywordFrame>deep</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameKeywordFrame>clone of</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEntityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>entity</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEqualityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Comparison"">
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameKeywordFrame></FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameDiscreteFrame PropertyName=""Equality"">
                <FrameKeywordFrame>phys</FrameKeywordFrame>
                <FrameKeywordFrame>deep</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameKeywordFrame Text="" ""/>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""IndexedExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInitializedObjectExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameInsertFrame CollectionName=""AssignmentBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordExpression}"">
        <FrameDiscreteFrame PropertyName=""Value"">
            <FrameKeywordFrame>True</FrameKeywordFrame>
            <FrameKeywordFrame>False</FrameKeywordFrame>
            <FrameKeywordFrame>Current</FrameKeywordFrame>
            <FrameKeywordFrame>Value</FrameKeywordFrame>
            <FrameKeywordFrame>Result</FrameKeywordFrame>
            <FrameKeywordFrame>Retry</FrameKeywordFrame>
            <FrameKeywordFrame>Exception</FrameKeywordFrame>
        </FrameDiscreteFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestCharacterExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>'</FrameKeywordFrame>
            <FrameCharacterFrame PropertyName=""Text""/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestNumberExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameNumberFrame PropertyName=""Text""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestStringExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>""</FrameKeywordFrame>
            <FrameTextValueFrame PropertyName=""Text""/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type INewExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>new</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Object"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IOldExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>old</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorIndexExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPreprocessorExpression}"">
        <FrameDiscreteFrame PropertyName=""Value"">
            <FrameKeywordFrame>DateAndTime</FrameKeywordFrame>
            <FrameKeywordFrame>CompilationDiscreteIdentifier</FrameKeywordFrame>
            <FrameKeywordFrame>ClassPath</FrameKeywordFrame>
            <FrameKeywordFrame>CompilerVersion</FrameKeywordFrame>
            <FrameKeywordFrame>ConformanceToStandard</FrameKeywordFrame>
            <FrameKeywordFrame>DiscreteClassIdentifier</FrameKeywordFrame>
            <FrameKeywordFrame>Counter</FrameKeywordFrame>
            <FrameKeywordFrame>Debugging</FrameKeywordFrame>
            <FrameKeywordFrame>RandomInteger</FrameKeywordFrame>
        </FrameDiscreteFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IResultOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>result of</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IUnaryNotExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>not</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IUnaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttributeFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConstantFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ConstantValue""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICreationFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>creation</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IFunctionFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>once per</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""Once"">
                        <FrameKeywordFrame>normal</FrameKeywordFrame>
                        <FrameKeywordFrame>object</FrameKeywordFrame>
                        <FrameKeywordFrame>processor</FrameKeywordFrame>
                        <FrameKeywordFrame>process</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                </FrameHorizontalPanelFrame>
                <FrameKeywordFrame>function</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexerFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame>indexer</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>modify</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame>
                        <FrameHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameOptionalFrame PropertyName=""GetterBody"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameOptionalFrame PropertyName=""SetterBody"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IProcedureFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>procedure</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPropertyFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>is</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>modify</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame>
                        <FrameHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FrameOptionalFrame PropertyName=""GetterBody""  />
                <FrameOptionalFrame PropertyName=""SetterBody"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIdentifier}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAsLongAsInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>as long as</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ContinueCondition""/>
                <FrameInsertFrame CollectionName=""ContinuationBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ContinuationBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalBlockListFrame PropertyName=""DestinationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttachmentInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>attach</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameKeywordFrame>to</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""EntityNameBlocks"" />
                <FrameInsertFrame CollectionName=""AttachmentBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""AttachmentBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICheckInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>check</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Command"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICreateInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>create</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityIdentifier"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>with</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""CreationRoutineIdentifier"" />
                <FrameHorizontalPanelFrame>
                    <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                    <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                    <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
                </FrameHorizontalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>same processor as</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""Processor"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDebugInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>debug</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IForLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameKeywordFrame>loop</FrameKeywordFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>local</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>init</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InitInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>while</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""WhileCondition""/>
                    <FrameInsertFrame CollectionName=""LoopInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>iterate</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""IterationInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>invariant</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>variant</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""Variant"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIfThenElseInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalBlockListFrame PropertyName=""ConditionalBlocks"" />
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>else</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""ElseInstructions"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Destination"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInspectInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>inspect</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameInsertFrame CollectionName=""WithBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""WithBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Destination"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IOverLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>over</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""OverList"" />
                <FrameKeywordFrame>for each</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""IndexerBlocks"" />
                <FrameDiscreteFrame PropertyName=""Iteration"">
                    <FrameKeywordFrame>Single</FrameKeywordFrame>
                    <FrameKeywordFrame>Nested</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""LoopInstructions"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>exit if</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""ExitEntityName"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>invariant</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>from</FrameKeywordFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>from</FrameKeywordFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRaiseEventInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>raise</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""QueryIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Event"">
                <FrameKeywordFrame>once</FrameKeywordFrame>
                <FrameKeywordFrame>forever</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IReleaseInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>release</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityName""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IThrowInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>throw</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""ExceptionType"" />
            <FrameKeywordFrame>with</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""CreationRoutine"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""AnchorKind"">
                <FrameKeywordFrame>declaration</FrameKeywordFrame>
                <FrameKeywordFrame>creation</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FramePlaceholderFrame PropertyName=""AnchoredName"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IFunctionType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>function</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGenericType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""TypeArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexerType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>indexer</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""EntityType""/>
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameDiscreteFrame PropertyName=""IndexerKind"">
                    <FrameKeywordFrame>read-only</FrameKeywordFrame>
                    <FrameKeywordFrame>write-only</FrameKeywordFrame>
                    <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""Anchor"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IProcedureType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>procedure</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPropertyType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>is</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""EntityType""/>
                </FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""PropertyKind"">
                    <FrameKeywordFrame>read-only</FrameKeywordFrame>
                    <FrameKeywordFrame>write-only</FrameKeywordFrame>
                    <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ISimpleType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ITupleType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>tuple</FrameKeywordFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParameterIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPositionalTypeArgument}"">
        <FramePlaceholderFrame PropertyName=""Source"" />
    </FrameNodeTemplate>
</FrameTemplateList>";
        #endregion

        #region Block Templates
        static string FrameBlockTemplateString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IArgument,Argument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAssertion,Assertion}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAssignmentArgument,AssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAttachment,Attachment}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IClass,Class}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IClassReplicate,ClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverload,CommandOverload}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverloadType,CommandOverloadType}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IConditional,Conditional}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IConstraint,Constraint}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IContinuation,Continuation}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IDiscrete,Discrete}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IEntityDeclaration,EntityDeclaration}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExceptionHandler,ExceptionHandler}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExport,Export}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExportChange,ExportChange}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IFeature,Feature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IGeneric,Generic}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IIdentifier,Identifier}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IImport,Import}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IInheritance,Inheritance}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IInstruction,Instruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ILibrary,Library}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IName,Name}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IObjectType,ObjectType}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IPattern,Pattern}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQualifiedName,QualifiedName}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverload,QueryOverload}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverloadType,QueryOverloadType}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IRange,Range}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IRename,Rename}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ITypeArgument,TypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ITypedef,Typedef}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IWith,With}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameVerticalPanelFrame>
    </FrameBlockTemplate>
</FrameTemplateList>
";
        #endregion
    }
}
