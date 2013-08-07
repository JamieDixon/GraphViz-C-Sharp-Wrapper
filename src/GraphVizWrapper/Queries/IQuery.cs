// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphVizWrapper.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the IQuery interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Queries
{
    public interface IQuery<in TInput, out TOutput>
    {
        TOutput Invoke(TInput input);
    }
}
