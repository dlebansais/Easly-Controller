namespace EaslyController.Focus
{
    using BaseNode;
    using BaseNodeHelper;
    using Easly;
    using System.Diagnostics;

    /// <summary>
    /// Objects common to a body cycle.
    /// </summary>
    internal class CycleFeatureInfo
    {
        #region Init
        public CycleFeatureInfo()
        {
            Documentation = NodeHelper.CreateEmptyDocumentation();
            ExportIdentifier = NodeHelper.CreateEmptyIdentifier();
            Export = ExportStatus.Exported;
            EntityName = NodeHelper.CreateEmptyName();
            EntityType = NodeHelper.CreateDefaultObjectType();
            EnsureBlocks = BlockListHelper.CreateEmptyBlockList<Assertion>();
            ConstantValue = NodeHelper.CreateDefaultExpression();
            CommandOverloadBlocks = BlockListHelper.CreateSimpleBlockList(NodeHelper.CreateEmptyCommandOverload());
            Once = OnceChoice.Normal;
            QueryOverloadBlocks = BlockListHelper.CreateSimpleBlockList(NodeHelper.CreateEmptyQueryOverload());
            PropertyKind = UtilityType.ReadWrite;
            ModifiedQueryBlocks = BlockListHelper.CreateEmptyBlockList<Identifier>();
            GetterBody = OptionalReferenceHelper.CreateReference(NodeHelper.CreateDefaultBody());
            SetterBody = OptionalReferenceHelper.CreateReference(NodeHelper.CreateDefaultBody());
            IndexParameterBlocks = BlockListHelper.CreateSimpleBlockList(NodeHelper.CreateEmptyEntityDeclaration());
            ParameterEnd = ParameterEndStatus.Closed;
        }
        #endregion

        #region Properties
        public Document Documentation { get; private set; }
        public Identifier ExportIdentifier { get; private set; }
        public ExportStatus Export { get; private set; }
        public Name EntityName { get; private set; }
        public ObjectType EntityType { get; private set; }
        public IBlockList<Assertion> EnsureBlocks { get; private set; }
        public Expression ConstantValue { get; private set; }
        public IBlockList<CommandOverload> CommandOverloadBlocks { get; private set; }
        public OnceChoice Once { get; private set; }
        public IBlockList<QueryOverload> QueryOverloadBlocks { get; private set; }
        public UtilityType PropertyKind { get; private set; }
        public IBlockList<Identifier> ModifiedQueryBlocks { get; private set; }
        public IOptionalReference<Body> GetterBody { get; private set; }
        public IOptionalReference<Body> SetterBody { get; private set; }
        public IBlockList<EntityDeclaration> IndexParameterBlocks { get; private set; }
        public ParameterEndStatus ParameterEnd { get; private set; }
        #endregion

        #region Client Interface
        public void Update(Feature feature)
        {
            switch (feature)
            {
                case AttributeFeature AsAttribute:
                    Documentation = AsAttribute.Documentation;
                    ExportIdentifier = AsAttribute.ExportIdentifier;
                    Export = AsAttribute.Export;
                    EntityName = AsAttribute.EntityName;
                    EntityType = AsAttribute.EntityType;
                    EnsureBlocks = AsAttribute.EnsureBlocks;
                    break;

                case ConstantFeature AsConstant:
                    Documentation = AsConstant.Documentation;
                    ExportIdentifier = AsConstant.ExportIdentifier;
                    Export = AsConstant.Export;
                    EntityName = AsConstant.EntityName;
                    EntityType = AsConstant.EntityType;
                    ConstantValue = AsConstant.ConstantValue;
                    break;

                case CreationFeature AsCreation:
                    Documentation = AsCreation.Documentation;
                    ExportIdentifier = AsCreation.ExportIdentifier;
                    Export = AsCreation.Export;
                    EntityName = AsCreation.EntityName;
                    CommandOverloadBlocks = AsCreation.OverloadBlocks;
                    break;

                case FunctionFeature AsFunction:
                    Documentation = AsFunction.Documentation;
                    ExportIdentifier = AsFunction.ExportIdentifier;
                    Export = AsFunction.Export;
                    EntityName = AsFunction.EntityName;
                    Once = AsFunction.Once;
                    QueryOverloadBlocks = AsFunction.OverloadBlocks;
                    break;

                case ProcedureFeature AsProcedure:
                    Documentation = AsProcedure.Documentation;
                    ExportIdentifier = AsProcedure.ExportIdentifier;
                    Export = AsProcedure.Export;
                    EntityName = AsProcedure.EntityName;
                    CommandOverloadBlocks = AsProcedure.OverloadBlocks;
                    break;

                case PropertyFeature AsProperty:
                    Documentation = AsProperty.Documentation;
                    ExportIdentifier = AsProperty.ExportIdentifier;
                    Export = AsProperty.Export;
                    EntityName = AsProperty.EntityName;
                    EntityType = AsProperty.EntityType;
                    PropertyKind = AsProperty.PropertyKind;
                    ModifiedQueryBlocks = AsProperty.ModifiedQueryBlocks;
                    GetterBody = AsProperty.GetterBody;
                    SetterBody = AsProperty.SetterBody;
                    break;

                case IndexerFeature AsIndexer:
                    Documentation = AsIndexer.Documentation;
                    ExportIdentifier = AsIndexer.ExportIdentifier;
                    Export = AsIndexer.Export;
                    EntityType = AsIndexer.EntityType;
                    IndexParameterBlocks = AsIndexer.IndexParameterBlocks;
                    ParameterEnd = AsIndexer.ParameterEnd;
                    ModifiedQueryBlocks = AsIndexer.ModifiedQueryBlocks;
                    GetterBody = AsIndexer.GetterBody;
                    SetterBody = AsIndexer.SetterBody;
                    break;
            }
        }
        #endregion
    }
}
