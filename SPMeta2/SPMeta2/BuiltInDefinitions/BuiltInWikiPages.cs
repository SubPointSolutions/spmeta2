﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;

namespace SPMeta2.BuiltInDefinitions
{
    public static class BuiltInWikiPages
    {
        public static WikiPageDefinition Home = new WikiPageDefinition
        {
            FileName = "Home.aspx"
        };

        public static WikiPageDefinition HowToUserThisLibrary = new WikiPageDefinition
        {
            FileName = "How To Use This Library.aspx"
        };
    }
}
