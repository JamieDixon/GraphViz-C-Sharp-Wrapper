// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphGeneration.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GraphGeneration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    
    using Commands;
    using Queries;

    /// <summary>
    /// The main entry class for the wrapper.
    /// </summary>
    public class GraphGeneration : IGraphGeneration
    {
        private const string ConfigFile = "config6";

        private readonly IGetStartProcessQuery _startProcessQuery;
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IRegisterLayoutPluginCommand _registerLayoutPlugincommand;
        private Enums.RenderingEngine _renderingEngine;
        private readonly string _graphvizPath;
        private readonly string _renderingEnginesPath;

        public GraphGeneration(IGetStartProcessQuery startProcessQuery, IGetProcessStartInfoQuery getProcessStartInfoQuery, IRegisterLayoutPluginCommand registerLayoutPlugincommand)
        {
            this._startProcessQuery = startProcessQuery;
            this._getProcessStartInfoQuery = getProcessStartInfoQuery;
            this._registerLayoutPlugincommand = registerLayoutPlugincommand;

            this._graphvizPath = ConfigurationManager.AppSettings["graphVizLocation"];
            this._renderingEnginesPath = ConfigurationManager.AppSettings["renderingEnginesLocation"];
        }

        #region Properties

      public Enums.RenderingEngine RenderingEngine
        {
            get { return this._renderingEngine; }
            set { this._renderingEngine = value; }
        }

        private string ConfigLocation
        {
            get
            {
                return this._graphvizPath + "/" + GraphGeneration.ConfigFile;
            }
        }

        private bool ConfigExists
        {
            get
            {
                return File.Exists(ConfigLocation);
            }
        }

        private string FilePath
        {
            get { return  this._renderingEnginesPath + "\\" + this.GetRenderingEngine(this._renderingEngine) + ".exe"; }
        }

        #endregion
 
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
        public byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType)
        {

            byte[] output;

            if (!ConfigExists)
            {
                this._registerLayoutPlugincommand.Invoke(FilePath, this.RenderingEngine);
            }

            string fileType = this.GetReturnType(returnType);

            var processStartInfo = this.GetProcessStartInfo(fileType);

            using (var process = this._startProcessQuery.Invoke(processStartInfo))
            {
                process.BeginErrorReadLine();
                using (var stdIn = process.StandardInput)
                {
                    stdIn.WriteLine(dotFile);
                }
                using (var stdOut = process.StandardOutput)
                {
                    var baseStream = stdOut.BaseStream;
                    output = this.ReadFully(baseStream);
                }
            }

            return output;
        }

        #region Private Methods
        
        private System.Diagnostics.ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return this._getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = this.FilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = "-v -o -T" + returnType,
                CreateNoWindow = true
            });
        }

        private byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private string GetReturnType(Enums.GraphReturnType returnType)
        {
            var nameValues = new Dictionary<Enums.GraphReturnType, string>
                                 {
                                     { Enums.GraphReturnType.Png, "png" },
                                     { Enums.GraphReturnType.Jpg, "jpg" },
                                     { Enums.GraphReturnType.Pdf, "pdf" },
                                     { Enums.GraphReturnType.Plain, "plain" },
                                     { Enums.GraphReturnType.PlainExt, "plain-ext" },
                                     { Enums.GraphReturnType.Svg, "svg" }

                                 };
            return nameValues[returnType];
        }

        private string GetRenderingEngine(Enums.RenderingEngine renderingType)
        {
            var nameValues = new Dictionary<Enums.RenderingEngine, string>
                                 {
                                     { Enums.RenderingEngine.Dot, "dot" },
                                     { Enums.RenderingEngine.Neato, "neato" },
                                     { Enums.RenderingEngine.Twopi, "twopi" },
                                     { Enums.RenderingEngine.Circo, "circo" },
                                     { Enums.RenderingEngine.Fdp, "fdp" },
                                     { Enums.RenderingEngine.Sfdp, "sfdp" },
                                     { Enums.RenderingEngine.Patchwork, "patchwork" },
                                     { Enums.RenderingEngine.Osage, "osage" }
                                 };
            return nameValues[renderingType];
        }

        #endregion
    }
}
