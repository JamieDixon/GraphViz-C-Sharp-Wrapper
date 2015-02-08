// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enums.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the static Enums type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    public static class Enums
    {
        public enum GraphReturnType
        {
            Pdf,
            Jpg,
            Png,
<<<<<<< HEAD
            Plain,
            PlainExt
=======
            Svg
>>>>>>> 41e86e4fab18dd87be9765e701865dc5d5ebe6c9
        }

        public enum RenderingEngine
        {
            Dot, // First item in enum is default rendering engine (E[0])
            Neato,
            Fdp
        }
    }
}
