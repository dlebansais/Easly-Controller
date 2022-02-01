namespace EaslyController.Focus
{
    using BaseNode;
    using BaseNodeHelper;
    using Easly;

    /// <summary>
    /// Objects common to a body cycle.
    /// </summary>
    internal class CycleBodyInfo
    {
        #region Init
        public CycleBodyInfo()
        {
            Documentation = NodeHelper.CreateEmptyDocument();
            RequireBlocks = BlockListHelper.CreateEmptyBlockList<Assertion>();
            EnsureBlocks = BlockListHelper.CreateEmptyBlockList<Assertion>();
            ExceptionIdentifierBlocks = BlockListHelper.CreateEmptyBlockList<Identifier>();
            EntityDeclarationBlocks = BlockListHelper.CreateEmptyBlockList<EntityDeclaration>();
            BodyInstructionBlocks = BlockListHelper.CreateEmptyBlockList<Instruction>();
            ExceptionHandlerBlocks = BlockListHelper.CreateEmptyBlockList<ExceptionHandler>();
            AncestorType = OptionalReferenceHelper.CreateReference(NodeHelper.CreateDefaultObjectType());
        }
        #endregion

        #region Properties
        public Document Documentation { get; private set; }
        public IBlockList<Assertion> RequireBlocks { get; private set; }
        public IBlockList<Assertion> EnsureBlocks { get; private set; }
        public IBlockList<Identifier> ExceptionIdentifierBlocks { get; private set; }
        public IBlockList<EntityDeclaration> EntityDeclarationBlocks { get; private set; }
        public IBlockList<Instruction> BodyInstructionBlocks { get; private set; }
        public IBlockList<ExceptionHandler> ExceptionHandlerBlocks { get; private set; }
        public IOptionalReference<ObjectType> AncestorType { get; private set; }
        #endregion

        #region Client Interface
        public void Update(Body body)
        {
            switch (body)
            {
                case EffectiveBody AsEffective:
                    Documentation = AsEffective.Documentation;
                    RequireBlocks = AsEffective.RequireBlocks;
                    EnsureBlocks = AsEffective.EnsureBlocks;
                    ExceptionIdentifierBlocks = AsEffective.ExceptionIdentifierBlocks;
                    EntityDeclarationBlocks = AsEffective.EntityDeclarationBlocks;
                    BodyInstructionBlocks = AsEffective.BodyInstructionBlocks;
                    ExceptionHandlerBlocks = AsEffective.ExceptionHandlerBlocks;
                    break;

                case DeferredBody AsDeferred:
                    Documentation = AsDeferred.Documentation;
                    RequireBlocks = AsDeferred.RequireBlocks;
                    EnsureBlocks = AsDeferred.EnsureBlocks;
                    ExceptionIdentifierBlocks = AsDeferred.ExceptionIdentifierBlocks;
                    break;

                case ExternBody AsExtern:
                    Documentation = AsExtern.Documentation;
                    RequireBlocks = AsExtern.RequireBlocks;
                    EnsureBlocks = AsExtern.EnsureBlocks;
                    ExceptionIdentifierBlocks = AsExtern.ExceptionIdentifierBlocks;
                    break;

                case PrecursorBody AsPrecursor:
                    Documentation = AsPrecursor.Documentation;
                    RequireBlocks = AsPrecursor.RequireBlocks;
                    EnsureBlocks = AsPrecursor.EnsureBlocks;
                    ExceptionIdentifierBlocks = AsPrecursor.ExceptionIdentifierBlocks;
                    AncestorType = AsPrecursor.AncestorType;
                    break;
            }
        }
        #endregion
    }
}
