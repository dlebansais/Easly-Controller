namespace EaslyController.Controller
{
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
        /// Try to read the clipboard to get a node.
        /// </summary>
        /// <param name="node">The node read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid node was found; Otherwise, false.</returns>
        public static bool TryReadNode(out INode node)
        {
            return TryReadContent(out node);
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
            return TryReadContent(out nodeList);
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
        /// <param name="nodeList">The block list read, null if none or if the clipboard contains invalid data.</param>
        /// <returns>True if a valid block list was found; Otherwise, false.</returns>
        public static bool TryReadBlockList(out IList<IBlock> nodeList)
        {
            return TryReadContent(out nodeList);
        }

        /// <summary>
        /// Writes the content of a list of blocks to the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="nodeList">The block list to write.</param>
        public static void WriteBlockList(IDataObject dataObject, IList<IBlock> nodeList)
        {
            WriteContent(dataObject, nodeList);
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
}
