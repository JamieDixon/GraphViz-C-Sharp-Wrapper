namespace GraphVizWrapper.Interfaces
{
    public interface IGraphVizWrapper
    {
        byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType);
        Enums.RenderingEngine RenderingEngine{get;set;}
    }
}
