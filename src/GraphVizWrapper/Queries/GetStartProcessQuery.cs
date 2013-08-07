// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStartProcessQuery.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GetStartProcessQuery type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Queries
{
    using System.Diagnostics;

    public class GetStartProcessQuery : IGetStartProcessQuery
    {
        public Process Invoke(ProcessStartInfo processStartInfo)
        {
            return Process.Start(processStartInfo);
        }
    }
}