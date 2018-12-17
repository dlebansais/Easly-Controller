using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IWriteableAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class WriteableAttachCallbackSet : ReadOnlyAttachCallbackSet, IWriteableAttachCallbackSet
    {
    }
}
