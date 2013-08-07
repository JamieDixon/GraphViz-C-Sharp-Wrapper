// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphGeneration.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GraphVizWrapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Interfaces
{
    public interface IGraphGeneration
    {
        /// <summary>
        /// Gets or sets the Rendering Engine to be used.
        /// </summary>
        Enums.RenderingEngine RenderingEngine { get; set; }

        /// <summary>
        /// Generates a graph based on the dot file passed in.
        /// </summary>
        /// <param name="dotFile">
        /// A string representation of a dot file.
        /// </param>
        /// <param name="returnType">
        /// The type of file to be returned.
        /// </param>
        /// <returns>
        /// a byte array.
        /// </returns>
        byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType);
    }
}
