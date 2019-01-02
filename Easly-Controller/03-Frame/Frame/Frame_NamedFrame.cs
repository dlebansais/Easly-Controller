using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public interface IFrameNamedFrame : IFrameFrame
    {
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }
    }

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public abstract class FrameNamedFrame : FrameFrame, IFrameNamedFrame
    {
        #region Properties
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (string.IsNullOrEmpty(PropertyName) || nodeType.GetProperty(PropertyName) == null)
                return false;

            return true;
        }
        #endregion
    }
}
