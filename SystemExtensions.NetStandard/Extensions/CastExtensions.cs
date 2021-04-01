namespace System.Extensions
{
    public static class CastExtensions
    {
        public static int Floor(this double value)
        {
            return (int)Math.Floor(value);
        }

        public static int Ceiling(this double value)
        {
            return (int)Math.Ceiling(value);
        }

        public static int Round(this double value)
        {
            return (int)Math.Round(value);
        }

        public static int Floor(this float value)
        {
            return (int)Math.Floor(value);
        }

        public static int Ceiling(this float value)
        {
            return (int)Math.Ceiling(value);
        }

        public static int Round(this float value)
        {
            return (int)Math.Round(value);
        }

        public static int ToInt(this double value)
        {
            return (int)value;
        }

        public static int ToInt(this float value)
        {
            return (int)value;
        }

        public static int ToInt(this decimal value)
        {
            return (int)value;
        }
    }
}
