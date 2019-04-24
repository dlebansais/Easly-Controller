namespace EaslyController.Controller
{
#if !TRAVIS
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using BaseNode;
    using PolySerializer;

    /// <summary>
    /// Helper class dedicated to managing the clipboard.
    /// </summary>
    public static class ClipboardHelper
    {
        #region Constants
        /// <summary>
        /// The clipboard format for a Easy node.
        /// </summary>
        public static readonly string ClipboardFormatNode = "185F4C03-D513-4F86-ADDB-C13C87417E81";

        /// <summary>
        /// The clipboard format for a list of Easy nodes.
        /// </summary>
        public static readonly string ClipboardFormatNodeList = "17A9B0FB-2247-4814-9529-3482BED933C2";
        #endregion

        #region Client Interface
        /// <summary>
        /// Try to read the clipboard to get a string.
        /// </summary>
        /// <param name="text">The string read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid string was found; Otherwise, false.</returns>
        public static bool TryReadText(out string text)
        {
            text = null;

            IDataObject DataObject = Clipboard.GetDataObject();
            if (DataObject != null)
            {
                string StringData = DataObject.GetData(typeof(string)) as string;
                if (StringData != null)
                    text = StringHelper.VisibleSubset(StringData);
            }

            return text != null;
        }

        /// <summary>
        /// Try to read the clipboard to get a int.
        /// </summary>
        /// <param name="value">The int read, -1 if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid int was found; Otherwise, false.</returns>
        public static bool TryReadInt(out int value)
        {
            value = -1;

            IDataObject DataObject = Clipboard.GetDataObject();
            if (DataObject != null)
                value = (int)DataObject.GetData(typeof(int));

            return value >= 0;
        }

        /// <summary>
        /// Try to read the clipboard to get a node.
        /// </summary>
        /// <param name="node">The node read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid node was found; Otherwise, false.</returns>
        public static bool TryReadNode(out INode node)
        {
            bool Success = TryReadContent(out node);

            if (Success)
            {
                Debug.Assert(node != null);

                // Create a copy of all guids.
                node = BaseNodeHelper.NodeHelper.DeepCloneNode(node, cloneCommentGuid: false);
            }

            return Success;
        }

        /// <summary>
        /// Writes the content of a node to the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="node">The node to write.</param>
        public static void WriteNode(IDataObject dataObject, INode node)
        {
            WriteContent(dataObject, node);
        }

        /// <summary>
        /// Try to read the clipboard to get a list of nodes.
        /// </summary>
        /// <param name="nodeList">The node  list read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid node list was found; Otherwise, false.</returns>
        public static bool TryReadNodeList(out IList<INode> nodeList)
        {
            bool Success = TryReadContent(out nodeList);

            if (Success)
            {
                Debug.Assert(nodeList != null);

                // Create a copy of all guids.
                nodeList = BaseNodeHelper.NodeHelper.DeepCloneNodeList(nodeList, cloneCommentGuid: false);
            }

            return Success;
        }

        /// <summary>
        /// Writes the content of a list of nodes to the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="nodeList">The node list to write.</param>
        public static void WriteNodeList(IDataObject dataObject, IList<INode> nodeList)
        {
            WriteContent(dataObject, nodeList);
        }

        /// <summary>
        /// Try to read the clipboard to get a list of blocks.
        /// </summary>
        /// <param name="blockList">The block list read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid block list was found; Otherwise, false.</returns>
        public static bool TryReadBlockList(out IList<IBlock> blockList)
        {
            bool Success = TryReadContent(out blockList);

            if (Success)
            {
                Debug.Assert(blockList != null);

                // Create a copy of all guids.
                blockList = BaseNodeHelper.NodeHelper.DeepCloneBlockList(blockList, cloneCommentGuid: false);
            }

            return Success;
        }

        /// <summary>
        /// Writes the content of a list of blocks to the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="blockList">The block list to write.</param>
        public static void WriteBlockList(IDataObject dataObject, IList<IBlock> blockList)
        {
            WriteContent(dataObject, blockList);
        }
        #endregion

        #region Implementation
        private static bool TryReadContent<T>(out T content)
            where T : class
        {
            content = null;

            IDataObject DataObject = Clipboard.GetDataObject();
            if (DataObject != null)
            {
                byte[] Data = DataObject.GetData(ClipboardFormatNode) as byte[];
                if (Data != null)
                {
                    try
                    {
                        Serializer s = new Serializer();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ms.Write(Data, 0, Data.Length);
                            ms.Seek(0, SeekOrigin.Begin);

                            content = s.Deserialize(ms) as T;

                            if (content != null)
                                return true;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return false;
        }

        private static void WriteContent<T>(IDataObject dataObject, T content)
            where T : class
        {
            Debug.Assert(dataObject != null);
            Debug.Assert(content != null);

            Serializer s = new Serializer();
            using (MemoryStream ms = new MemoryStream())
            {
                s.Serialize(ms, content);
                byte[] Buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(Buffer, 0, Buffer.Length);
                dataObject.SetData(ClipboardFormatNode, Buffer);
            }
        }
        #endregion
    }
#endif
}
