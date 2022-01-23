using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using EaslyController;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Focus;
using EaslyController.Frame;
using EaslyController.Layout;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using NUnit.Framework;

namespace Coverage
{
    [TestFixture]
    public partial class CoverageSet
    {
        private enum Imperfections
        {
            None,
            BadGuid,
        };

        private static Guid ValueGuid0 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA00}");
        private static Guid ValueGuid1 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA01}");
        private static Guid ValueGuid2 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA02}");
        private static Guid ValueGuid3 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA03}");
        private static Guid ValueGuid4 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA04}");
        private static Guid ValueGuid5 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA05}");
        private static Guid ValueGuid6 = new Guid("{FFFFFFFF-C70B-4BAF-AE1B-C342CD9BFA06}");

        private static Leaf CreateLeaf(Guid guid0)
        {
            BaseNode.Document NewLeafDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocumentation("", guid0);
            Leaf NewLeaf = new Leaf(NewLeafDocument, "leaf");

            return NewLeaf;
        }

        private static Tree CreateTree()
        {
            Leaf Placeholder = CreateLeaf(Guid.NewGuid());

            BaseNode.Document TreeDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocumentation("tree doc", Guid.NewGuid());
            Tree TreeInstance = new Tree(TreeDocument);

            BaseNodeHelper.NodeTreeHelperChild.SetChildNode(TreeInstance, nameof(Tree.Placeholder), Placeholder);
            BaseNodeHelper.NodeTreeHelper.SetBooleanProperty(TreeInstance, nameof(Main.ValueBoolean), true);
            BaseNodeHelper.NodeTreeHelper.SetEnumProperty(TreeInstance, nameof(Main.ValueEnum), BaseNode.CopySemantic.Value);
            BaseNodeHelper.NodeTreeHelper.SetGuidProperty(TreeInstance, nameof(Main.ValueGuid), Guid.NewGuid());

            return TreeInstance;
        }

        private static Main CreateRoot(Guid valueGuid, Imperfections imperfection)
        {
            Guid MainGuid = Guid.NewGuid();
            Guid LeafGuid0 = Guid.NewGuid();

            Tree PlaceholderTree = CreateTree();

            Leaf PlaceholderLeaf = CreateLeaf(imperfection == Imperfections.BadGuid ? MainGuid : LeafGuid0);

            BaseNode.Document UnassignedOptionalLeafDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocumentation("leaf doc", Guid.NewGuid());
            Leaf UnassignedOptionalLeaf = new Leaf(UnassignedOptionalLeafDocument, "optional unassigned");

            Easly.IOptionalReference<Leaf> UnassignedOptional = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Leaf>(UnassignedOptionalLeaf);
            Easly.IOptionalReference<Leaf> EmptyOptional = BaseNodeHelper.OptionalReferenceHelper.CreateEmptyReference<Leaf>();

            Leaf AssignedOptionalLeaf = CreateLeaf(Guid.NewGuid());

            Easly.IOptionalReference<Leaf> AssignedOptionalForLeaf = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Leaf>(AssignedOptionalLeaf);
            AssignedOptionalForLeaf.Assign();

            Tree AssignedOptionalTree = CreateTree();
            Easly.IOptionalReference<Tree> AssignedOptionalForTree = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Tree>(AssignedOptionalTree);
            AssignedOptionalForTree.Assign();

            Leaf FirstChild = CreateLeaf(Guid.NewGuid());
            Leaf SecondChild = CreateLeaf(Guid.NewGuid());
            Leaf ThirdChild = CreateLeaf(Guid.NewGuid());
            Leaf FourthChild = CreateLeaf(Guid.NewGuid());

            BaseNode.IBlock<Leaf> SecondBlock = BaseNodeHelper.BlockListHelper.CreateBlock<Leaf>(new List<Leaf>() { SecondChild, ThirdChild });
            BaseNode.IBlock<Leaf> ThirdBlock = BaseNodeHelper.BlockListHelper.CreateBlock<Leaf>(new List<Leaf>() { FourthChild });

            BaseNode.IBlockList<Leaf> LeafBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<Leaf>(FirstChild);
            LeafBlocks.NodeBlockList.Add(SecondBlock);
            LeafBlocks.NodeBlockList.Add(ThirdBlock);
            BaseNodeHelper.NodeTreeHelper.SetCommentText(SecondBlock.Documentation, "test");

            Leaf FirstPath = CreateLeaf(Guid.NewGuid());
            Leaf SecondPath = CreateLeaf(Guid.NewGuid());

            IList<Leaf> LeafPath = new List<Leaf>();
            LeafPath.Add(FirstPath);
            LeafPath.Add(SecondPath);

            BaseNode.Document RootDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocumentation("main doc", MainGuid);
            Main Root = new Main(RootDocument);

            BaseNodeHelper.NodeTreeHelperChild.SetChildNode(Root, nameof(Main.PlaceholderTree), PlaceholderTree);
            BaseNodeHelper.NodeTreeHelperChild.SetChildNode(Root, nameof(Main.PlaceholderLeaf), PlaceholderLeaf);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(Root, nameof(Main.UnassignedOptionalLeaf), (Easly.IOptionalReference)UnassignedOptional);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(Root, nameof(Main.EmptyOptionalLeaf), (Easly.IOptionalReference)EmptyOptional);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(Root, nameof(Main.AssignedOptionalTree), (Easly.IOptionalReference)AssignedOptionalForTree);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(Root, nameof(Main.AssignedOptionalLeaf), (Easly.IOptionalReference)AssignedOptionalForLeaf);
            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(Root, nameof(Main.LeafBlocks), (BaseNode.IBlockList)LeafBlocks);
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(Root, nameof(Main.LeafPath), (IList)LeafPath);
            BaseNodeHelper.NodeTreeHelper.SetBooleanProperty(Root, nameof(Main.ValueBoolean), true);
            BaseNodeHelper.NodeTreeHelper.SetEnumProperty(Root, nameof(Main.ValueEnum), BaseNode.CopySemantic.Value);
            BaseNodeHelper.NodeTreeHelper.SetStringProperty(Root, nameof(Main.ValueString), "s");
            BaseNodeHelper.NodeTreeHelper.SetGuidProperty(Root, nameof(Main.ValueGuid), valueGuid);

            return Root;
        }
    }
}
