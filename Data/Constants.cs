using System;

namespace d4lilah
{
    public static class Constants
    {
        public const int SCRIPT_TYPE_ENTITY = 0;
        public const int SCRIPT_TYPE_WIDGET = 1;
        public const int SCRIPT_TYPE_LEVEL = 2;
        public const int SCRIPT_TYPE_MISC = 3;

        public const string SCRIPT_TYPE_ENTITY_NAME = "entity";
        public const string SCRIPT_TYPE_WIDGET_NAME = "widget";
        public const string SCRIPT_TYPE_LEVEL_NAME = "level";

        public static Random rand = new Random();
    }
}
