using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Base
{
    public abstract class WebPartDefinitionBase : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string Title { get; set; }

        /// <summary>
        /// ID of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string Id { get; set; }

        /// <summary>
        /// ZoneId of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string ZoneId { get; set; }

        /// <summary>
        /// ZoneIndex of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public int ZoneIndex { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] Id:[{1}] ZoneId:[{2}] ZoneIndex:[{3}]", new[] { Title, Id, ZoneId, ZoneIndex.ToString() });
        }

        #endregion
    }
}
