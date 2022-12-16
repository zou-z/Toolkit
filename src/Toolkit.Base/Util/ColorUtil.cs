namespace Toolkit.Base.Util
{
    public static class ColorUtil
    {
        public static bool IsValidStringColor(string colorString)
        {
            if (colorString != null && (colorString.Length == 7 || colorString.Length == 9) && colorString[0] == '#')
            {
                colorString = colorString.ToUpper();
                for (int i = 1; i < colorString.Length; ++i)
                {
                    int ascii = colorString[i];
                    if (ascii < 48 || (57 < ascii && ascii < 65) || 70 < ascii)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
