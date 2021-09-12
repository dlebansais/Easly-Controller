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
            FrameTemplateReadOnlyDictionary FrameCustomNodeTemplates = NodeTemplateDictionary.ToReadOnly();
            BlockTemplateDictionary = LoadTemplate(FrameBlockTemplateString);
            FrameTemplateReadOnlyDictionary FrameCustomBlockTemplates = BlockTemplateDictionary.ToReadOnly();
            FrameTemplateSet = new FrameTemplateSet(FrameCustomNodeTemplates, FrameCustomBlockTemplates);
        }

        private static FrameTemplateDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                Templates = (FrameTemplateList)XamlReader.Parse(s);

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
        public static FrameTemplateDictionary NodeTemplateDictionary { get; private set; }
        public static FrameTemplateDictionary BlockTemplateDictionary { get; private set; }
        public static IFrameTemplateSet FrameTemplateSet { get; private set; }
        public static FrameTemplateList Templates { get; private set; } = null!;
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
    <FrameNodeTemplate NodeType=""{xaml:Type cov:Leaf}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type cov:Tree}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type cov:Main}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type cov:Root}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Assertion}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""Tag"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Attachment}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Class}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""PatternBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CommandOverload}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CommandOverloadType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Conditional}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Constraint}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Continuation}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Discrete}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""NumericValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:EntityDeclaration}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ExceptionHandler}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Export}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ExportChange}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Generic}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:GlobalReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalListFrame PropertyName=""Patterns"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Import}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Inheritance}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Library}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Name}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Pattern}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:QualifiedName}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalListFrame PropertyName=""Path"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:QueryOverload}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:QueryOverloadType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Range}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Rename}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""SourceIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DestinationIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Root}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Scope}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Typedef}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>is</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DefinedType"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalBlockListFrame PropertyName=""ParameterBlocks""/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:With}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:DeferredBody}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PositionalArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:EffectiveBody}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ExternBody}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PrecursorBody}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AgentExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AssertionTagExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>tag</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""TagIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:BinaryConditionalExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:BinaryOperatorExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ClassConstantExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FramePlaceholderFrame PropertyName=""ConstantIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CloneOfExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:EntityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>entity</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:EqualityExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IndexQueryExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:InitializedObjectExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameInsertFrame CollectionName=""AssignmentBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:KeywordEntityExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:KeywordExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ManifestCharacterExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
            <FrameCharacterFrame PropertyName=""Text""/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ManifestNumberExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameNumberFrame PropertyName=""Text""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ManifestStringExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
            <FrameTextValueFrame PropertyName=""Text""/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:NewExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>new</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Object"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:OldExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>old</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PrecursorExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PreprocessorExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:QueryExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ResultOfExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:UnaryNotExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:UnaryOperatorExpression}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AttributeFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ConstantFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CreationFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:FunctionFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IndexerFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ProcedureFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PropertyFeature}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:Identifier}"">
        <FrameVerticalPanelFrame>
            <FrameCommentFrame/>
            <FrameTextValueFrame PropertyName=""Text""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AsLongAsInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameHorizontalBlockListFrame PropertyName=""DestinationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AttachmentInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CheckInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>check</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CommandInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:CreateInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:DebugInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ForLoopInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IfThenElseInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IndexAssignmentInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:InspectInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:KeywordAssignmentInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:OverLoopInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexAssignmentInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PrecursorInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:RaiseEventInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ReleaseInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>release</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityName""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ThrowInstruction}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AnchoredType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:FunctionType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>function</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:GenericType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:IndexerType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:KeywordAnchoredType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:ProcedureType}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FrameKeywordFrame>procedure</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PropertyType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:SimpleType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:TupleType}"">
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
    <FrameNodeTemplate NodeType=""{xaml:Type easly:AssignmentTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameCommentFrame/>
            <FramePlaceholderFrame PropertyName=""ParameterIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type easly:PositionalTypeArgument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,cov:Leaf}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,cov:Tree}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,cov:Main}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Argument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Assertion}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:AssignmentArgument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Attachment}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Class}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ClassReplicate}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:CommandOverload}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:CommandOverloadType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Conditional}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Constraint}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Continuation}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Discrete}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:EntityDeclaration}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ExceptionHandler}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Export}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ExportChange}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Feature}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Generic}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Identifier}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Import}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Inheritance}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Instruction}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Library}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Name}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ObjectType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Pattern}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QualifiedName}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QueryOverload}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QueryOverloadType}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Range}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Rename}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:TypeArgument}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Typedef}"">
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
    <FrameBlockTemplate NodeType=""{xaml:Type easly:Block,easly:With}"">
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
