using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    public static class BuiltInWebpartTemplates
    {
        #region constructors

        static BuiltInWebpartTemplates()
        {
            var asm = typeof(BuiltInWebpartTemplates).Assembly;

            ContentEditorWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ContentEditor.webpart");
        }

        #endregion

        #region properties

        public static string ContentEditorWebPart { get; set; }

        #endregion
    }
}
