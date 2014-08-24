using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class FolderModelHost : SSOMModelHostBase
    {
        public SPList CurrentList { get; set; }
        public SPListItem CurrentListItem { get; set; }

        public SPDocumentLibrary CurrentLibrary { get; set; }
        public SPFolder CurrentLibraryFolder { get; set; }

        public SPWeb CurrentWeb { get; set; }
        public SPFolder CurrentWebFolder { get; set; }
    }
}
