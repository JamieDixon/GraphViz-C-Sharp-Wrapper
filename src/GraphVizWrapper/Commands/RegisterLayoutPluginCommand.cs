// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterLayoutPluginCommand.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the RegisterLayoutPluginCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Commands
{
    using Queries;
    
    public class RegisterLayoutPluginCommand : IRegisterLayoutPluginCommand
    {
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IGetStartProcessQuery _getStartProcessQuery;

        public RegisterLayoutPluginCommand(IGetProcessStartInfoQuery getProcessStartInfoQuery, IGetStartProcessQuery getStartProcessQuery)
        {
            _getStartProcessQuery = getStartProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
        }

        public void Invoke(string configFilePath, Enums.RenderingEngine renderingEngine)
        {
            var processStartInfo = _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
                                                 {
                                                     FileName = configFilePath,
                                                     UseShellExecute = false,
                                                     Arguments = "-c",
                                                     CreateNoWindow = false
                                                 });

            _getStartProcessQuery.Invoke(processStartInfo);
        }

        public void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
