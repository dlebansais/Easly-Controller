using EaslyController.Frame;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CoverageFrameTemplateSet
    {
        #region Init
        static CoverageFrameTemplateSet()
        {
            NodeTemplateDictionary = LoadTemplate(FrameTemplateListString);
            IFrameTemplateReadOnlyDictionary FrameCustomNodeTemplates = NodeTemplateDictionary.ToReadOnly();
            BlockTemplateDictionary = LoadTemplate(FrameBlockTemplateString);
            IFrameTemplateReadOnlyDictionary FrameCustomBlockTemplates = BlockTemplateDictionary.ToReadOnly();
            FrameTemplateSet = new FrameTemplateSet(FrameCustomNodeTemplates, FrameCustomBlockTemplates);
        }

        private static IFrameTemplateDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                Templates = (IFrameTemplateList)XamlReader.Parse(s);

                FrameTemplateDictionary TemplateDictionary = new FrameTemplateDictionary();
                foreach (IFrameTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FrameFrame.FrameRoot);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                return TemplateDictionary;
            }
        }

        private CoverageFrameTemplateSet()
        {
        }
        #endregion

        #region Properties
        public static IFrameTemplateDictionary NodeTemplateDictionary { get; private set; }
        public static IFrameTemplateDictionary BlockTemplateDictionary { get; private set; }
        public static IFrameTemplateSet FrameTemplateSet { get; private set; }
        public static IFrameTemplateList Templates { get; private set; } = null!;
        #endregion

        #region Node Templates
        static string FrameTemplateListString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:cov=""clr-namespace:Coverage;assembly=Test-Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameNodeTemplate NodeType=""{xaml:Type cov:ILeaf}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type cov:ITree}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""Placeholder""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameDiscreteFrame PropertyName=""ValueBoolean"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameDiscreteFrame PropertyName=""ValueEnum"">
                <FrameKeywordFrame>Any</FrameKeywordFrame>
                <FrameKeywordFrame>Reference</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type cov:IMain}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""PlaceholderTree""/>
            <FramePlaceholderFrame PropertyName=""PlaceholderLeaf""/>
            <FrameOptionalFrame PropertyName=""UnassignedOptionalLeaf"" />
            <FrameOptionalFrame PropertyName=""EmptyOptionalLeaf"" />
            <FrameOptionalFrame PropertyName=""AssignedOptionalTree"" />
            <FrameOptionalFrame PropertyName=""AssignedOptionalLeaf"" />
            <FrameInsertFrame CollectionName=""LeafBlocks"" />
            <FrameVerticalBlockListFrame PropertyName=""LeafBlocks"" />
            <FrameHorizontalListFrame PropertyName=""LeafPath"" />
            <FrameDiscreteFrame PropertyName=""ValueBoolean"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameDiscreteFrame PropertyName=""ValueEnum"">
                <FrameKeywordFrame>Any</FrameKeywordFrame>
                <FrameKeywordFrame>Reference</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameTextValueFrame PropertyName=""ValueString""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type cov:IRoot}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalBlockListFrame PropertyName=""MainBlocksH"" />
            <FrameVerticalBlockListFrame PropertyName=""MainBlocksV"" />
            <FrameOptionalFrame PropertyName=""UnassignedOptionalMain"" />
            <FrameTextValueFrame PropertyName=""ValueString""/>
            <FrameHorizontalListFrame PropertyName=""LeafPathH"" />
            <FrameVerticalListFrame PropertyName=""LeafPathV"" />
            <FrameOptionalFrame PropertyName=""UnassignedOptionalLeaf"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAssertion}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""Tag"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAttachment}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IClass}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""PatternBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICommandOverload}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICommandOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IConditional}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IConstraint}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IContinuation}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IDiscrete}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""NumericValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IEntityDeclaration}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>:</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityType"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""DefaultValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IExceptionHandler}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>catch</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ExceptionIdentifier"" />
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IExport}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IExportChange}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IGeneric}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IGlobalReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalListFrame PropertyName=""Patterns"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IImport}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IInheritance}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ILibrary}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IName}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPattern}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IQualifiedName}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalListFrame PropertyName=""Path"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IQueryOverload}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IQueryOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IRange}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IRename}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""SourceIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DestinationIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IRoot}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IScope}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ITypedef}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>is</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DefinedType"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalBlockListFrame PropertyName=""ParameterBlocks""/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IWith}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>case</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""RangeBlocks""/>
                <FrameInsertFrame CollectionName=""RangeBlocks""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
                    <FrameKeywordFrame IsFocusable=""true"">deferred</FrameKeywordFrame>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPositionalArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
                    <FrameKeywordFrame IsFocusable=""true"">extern</FrameKeywordFrame>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAgentExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>agent</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""BaseType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Delegated"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAssertionTagExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>tag</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""TagIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IBinaryConditionalExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
                <FrameKeywordFrame>xor</FrameKeywordFrame>
                <FrameKeywordFrame>⇒</FrameKeywordFrame>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IBinaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IClassConstantExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FramePlaceholderFrame PropertyName=""ConstantIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICloneOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IEntityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>entity</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IEqualityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Comparison"">
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameKeywordFrame>!=</FrameKeywordFrame>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIndexQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IInitializedObjectExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameInsertFrame CollectionName=""AssignmentBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IKeywordEntityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>entity</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""Value"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
                <FrameKeywordFrame>Indexer</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IKeywordExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameDiscreteFrame PropertyName=""Value"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
                <FrameKeywordFrame>Indexer</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IManifestCharacterExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
            <FrameCharacterFrame PropertyName=""Text""/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IManifestNumberExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameNumberFrame PropertyName=""Text""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IManifestStringExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
            <FrameTextValueFrame PropertyName=""Text""/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:INewExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>new</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Object"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IOldExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>old</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPrecursorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPrecursorIndexExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPreprocessorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IResultOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IUnaryNotExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IUnaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAttributeFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IConstantFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICreationFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IFunctionFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIndexerFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IProcedureFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPropertyFeature}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIdentifier}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAsLongAsInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalBlockListFrame PropertyName=""DestinationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAttachmentInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICheckInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>check</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICommandInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ICreateInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IDebugInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IForLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIfThenElseInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IInspectInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IKeywordAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameDiscreteFrame PropertyName=""Destination"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
                <FrameKeywordFrame>Indexer</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IOverLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPrecursorIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPrecursorInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IRaiseEventInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>raise</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""QueryIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Event"">
                <FrameKeywordFrame>once</FrameKeywordFrame>
                <FrameKeywordFrame>forever</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IReleaseInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>release</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityName""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IThrowInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""AnchorKind"">
                <FrameKeywordFrame>declaration</FrameKeywordFrame>
                <FrameKeywordFrame>creation</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FramePlaceholderFrame PropertyName=""AnchoredName"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IFunctionType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>function</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IGenericType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IIndexerType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IKeywordAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""Anchor"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
                <FrameKeywordFrame>Indexer</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IProcedureType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>procedure</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPropertyType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ISimpleType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ITupleType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IAssignmentTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ParameterIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IPositionalTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
</FrameTemplateList>";
        #endregion

        #region Block Templates
        static string FrameBlockTemplateString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:cov=""clr-namespace:Coverage;assembly=Test-Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:ILeaf,cov:Leaf}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:ITree,cov:Tree}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:IMain,cov:Main}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IArgument,easly:Argument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAssertion,easly:Assertion}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAssignmentArgument,easly:AssignmentArgument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAttachment,easly:Attachment}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IClass,easly:Class}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IClassReplicate,easly:ClassReplicate}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ICommandOverload,easly:CommandOverload}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ICommandOverloadType,easly:CommandOverloadType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IConditional,easly:Conditional}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IConstraint,easly:Constraint}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IContinuation,easly:Continuation}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IDiscrete,easly:Discrete}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IEntityDeclaration,easly:EntityDeclaration}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExceptionHandler,easly:ExceptionHandler}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExport,easly:Export}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExportChange,easly:ExportChange}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IFeature,easly:Feature}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IGeneric,easly:Generic}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IIdentifier,easly:Identifier}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IImport,easly:Import}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IInheritance,easly:Inheritance}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IInstruction,easly:Instruction}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ILibrary,easly:Library}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IName,easly:Name}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IObjectType,easly:ObjectType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IPattern,easly:Pattern}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQualifiedName,easly:QualifiedName}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQueryOverload,easly:QueryOverload}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQueryOverloadType,easly:QueryOverloadType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IRange,easly:Range}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IRename,easly:Rename}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ITypeArgument,easly:TypeArgument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ITypedef,easly:Typedef}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IWith,easly:With}"">
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
