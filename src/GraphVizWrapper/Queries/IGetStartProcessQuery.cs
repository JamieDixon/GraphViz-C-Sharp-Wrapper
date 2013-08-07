// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetStartProcessQuery.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GraphVizWrapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Queries
{
    using System.Diagnostics;

    public interface IGetStartProcessQuery : IQuery<ProcessStartInfo, Process>
    {
    }
}