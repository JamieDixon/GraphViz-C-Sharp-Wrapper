namespace GraphVizWrapper.Interfaces
{
    public interface IQuery<in TInput, out TOutput>
    {
        TOutput Invoke(TInput input);
    }
}
