namespace GraphVizWrapper
{
    public static class Enums
    {
        public enum GraphReturnType
        {
            Pdf,
            Jpg,
            Png
        }

        public enum RenderingEngine
        {
            Dot, // First item in enum is default rendering engine (E[0])
            Neato,
            Fdp
        }
    }
}
