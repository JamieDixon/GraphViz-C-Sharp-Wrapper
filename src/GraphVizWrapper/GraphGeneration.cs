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
    using System.Configuration;
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

        private readonly IGetStartProcessQuery startProcessQuery;
        private readonly IGetProcessStartInfoQuery getProcessStartInfoQuery;
        private readonly IRegisterLayoutPluginCommand registerLayoutPlugincommand;
        private Enums.RenderingEngine renderingEngine;
        private String graphvizPath = null;

        public GraphGeneration(IGetStartProcessQuery startProcessQuery, IGetProcessStartInfoQuery getProcessStartInfoQuery, IRegisterLayoutPluginCommand registerLayoutPlugincommand)
        {
            this.startProcessQuery = startProcessQuery;
            this.getProcessStartInfoQuery = getProcessStartInfoQuery;
            this.registerLayoutPlugincommand = registerLayoutPlugincommand;

            this.graphvizPath = ConfigurationManager.AppSettings["graphVizLocation"];
        }

        #region Properties

        public String GraphvizPath
        {
            get { return graphvizPath ?? AssemblyDirectory + "/" + ProcessFolder; }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    String path = value.Replace("\\", "/");
                    graphvizPath = path.EndsWith("/") ? path.Substring(0, path.LastIndexOf('/')) : path;
                }
                else
                {
                    graphvizPath = null;
                }
            }
        }
        
        public Enums.RenderingEngine RenderingEngine
        {
            get { return this.renderingEngine; }
            set { this.renderingEngine = value; }
        }

        private string ConfigLocation
        {
            get
            {
                return GraphvizPath + "/" + ConfigFile;
            }
        }

        private bool ConfigExists
        {
            get
            {
                return File.Exists(ConfigLocation);
            }
        }
        
        private static string AssemblyDirectory
        {
            get
            {
                var uriBuilder = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string path = Uri.UnescapeDataString(uriBuilder.Path);
                return path.Substring(0, path.LastIndexOf('/'));
            }
        }

        private string FilePath
        {
            get { return  GraphvizPath + "/" + this.GetRenderingEngine(this.renderingEngine) + ".exe"; }
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
                this.registerLayoutPlugincommand.Invoke(FilePath, this.RenderingEngine);
            }

            string fileType = this.GetReturnType(returnType);

            var processStartInfo = this.GetProcessStartInfo(fileType);

            using (var process = this.startProcessQuery.Invoke(processStartInfo))
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
            return this.getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
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
