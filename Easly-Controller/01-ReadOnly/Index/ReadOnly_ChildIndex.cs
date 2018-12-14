namespace EaslyController.ReadOnly
{
    public interface IReadOnlyChildIndex : IReadOnlyIndex
    {
        string PropertyName { get; }
    }
}
