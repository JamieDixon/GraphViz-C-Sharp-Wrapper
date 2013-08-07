// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetProcessStartInfoQuery.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the IGetProcessStartInfoQuery interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Queries
{
    using System.Diagnostics;

    public interface IGetProcessStartInfoQuery : IQuery<IProcessStartInfoWrapper, ProcessStartInfo>
    {
    }
}
