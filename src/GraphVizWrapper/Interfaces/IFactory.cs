namespace GraphVizWrapper.Interfaces
{
    public interface IFactory<out T>
    {
        T Create();
    }

}
