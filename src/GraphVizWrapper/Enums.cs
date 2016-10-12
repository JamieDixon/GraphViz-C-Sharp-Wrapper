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
            Plain,
            PlainExt,
            Svg
        }

        public enum RenderingEngine
        {
            Dot, // First item in enum is default rendering engine (E[0])
            Neato,
            Twopi,
            Circo,
            Fdp,
            Sfdp,
            Patchwork,
            Osage
        }
    }
}
