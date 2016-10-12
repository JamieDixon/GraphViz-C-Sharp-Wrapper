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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    
    using Commands;
    using Queries;

    /// <summary>
    /// The main entry class for the wrapper.
    /// </summary>
    public class GraphGeneration : IGraphGeneration
    {
        private const string ProcessFolder = "GraphViz";
        private const string ConfigFile = "config6";

        private readonly IGetStartProcessQuery _startProcessQuery;
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IRegisterLayoutPluginCommand _registerLayoutPlugincommand;
        private Enums.RenderingEngine _renderingEngine;
        private string _graphvizPath;

        public GraphGeneration(IGetStartProcessQuery startProcessQuery, IGetProcessStartInfoQuery getProcessStartInfoQuery, IRegisterLayoutPluginCommand registerLayoutPlugincommand)
        {
            _startProcessQuery = startProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
            _registerLayoutPlugincommand = registerLayoutPlugincommand;

            _graphvizPath = @"C:\Program Files (x86)\GraphViz2.38";
        }

        #region Properties

        public string GraphvizPath
        {
            get { return _graphvizPath ?? AssemblyDirectory + "/" + ProcessFolder; }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    string path = value.Replace("\\", "/");
                    _graphvizPath = path.EndsWith("/") ? path.Substring(0, path.LastIndexOf('/')) : path;
                }
                else
                {
                    _graphvizPath = null;
                }
            }
        }
        
        public Enums.RenderingEngine RenderingEngine
        {
            get { return _renderingEngine; }
            set { _renderingEngine = value; }
        }

        private string ConfigLocation => GraphvizPath + "/" + ConfigFile;

        private bool ConfigExists => File.Exists(ConfigLocation);

        private static string AssemblyDirectory
        {
            get
            {
                var uriBuilder = new UriBuilder(Assembly.GetEntryAssembly().CodeBase);
                string path = Uri.UnescapeDataString(uriBuilder.Path);
                return path.Substring(0, path.LastIndexOf('/'));
            }
        }

        private string FilePath => GraphvizPath + "/bin/" + GetRenderingEngine(_renderingEngine) + ".exe";

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
                _registerLayoutPlugincommand.Invoke(FilePath, RenderingEngine);
            }

            string fileType = GetReturnType(returnType);

            var processStartInfo = GetProcessStartInfo(fileType);

            var process = _startProcessQuery.Invoke(processStartInfo);
            
            process.BeginErrorReadLine();
            using (var stdIn = process.StandardInput)
            {
                stdIn.WriteLine(dotFile);
            }
            using (var stdOut = process.StandardOutput)
            {
                var baseStream = stdOut.BaseStream;
                output = ReadFully(baseStream);
            }
            

            return output;
        }

        #region Private Methods
        
        private ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = FilePath,
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
