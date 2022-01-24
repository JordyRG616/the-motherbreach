namespace StringHandler 
{
    public static class StatColorHandler
    {
        public static string DamagePaint(string text)
        {
            var container = "<#ff5cff>" + text + "</color>";
            return container;
        }

        public static string RestPaint(string text)
        {
            var container = "<#00f7ff>" + text + "</color>";
            return container;
        }

        public static string HealthPaint(string text)
        {
            var container = "<#009d4a>" + text + "</color>";
            return container;
        }

        public static string StatPaint(string text)
        {
            var container = "<#d0ff00>" + text + "</color>";
            return container;
        }
    }

    public static class KeywordHandler
    {
        public static string KeywordPaint(Keyword keyword)
        {
            var container = "<#c46f00><i><lowercase>" + keyword.ToString() + "</lowercase></i></color>";
            return container;
        }

        public static string KeywordDescription(Keyword keyword)
        {
            var desc = DescriptionDictionary.Main.GetDescription(keyword.ToString());
            var container = KeywordPaint(keyword) + "\n" + desc + "\n\n";
            return container;
        }
    }
}
