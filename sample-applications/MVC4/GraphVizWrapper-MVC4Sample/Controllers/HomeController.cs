//---------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   TODO: Summary
// </summary>
//---------------------------------------------------------------------------------------------------------------------
namespace GraphVizWrapper_MVC4Sample.Controllers
{
    using System;
    using System.Web.Mvc;

    using GraphVizWrapper;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The graph viz wrapper.
        /// </summary>
        private readonly IGraphGeneration graphVizWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="graphVizWrapper">
        /// The graph viz wrapper.
        /// </param>
        public HomeController(IGraphGeneration graphVizWrapper)
        {
            this.graphVizWrapper = graphVizWrapper;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var bytes = this.graphVizWrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Jpg);
            
            // Alternatively you could save the image on the server as a file.
            var viewModel = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(bytes));
            ViewBag.Data = viewModel;
            return this.View();
        }
    }
}
