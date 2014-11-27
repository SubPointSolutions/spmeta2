using SPMeta2.Models;

namespace SPMeta2.Containers.Common
{
    public class EventHooks
    {
        #region properties

        public bool OnProvisioned { get; set; }
        public bool OnProvisioning { get; set; }

        public ModelNode ModelNode { get; set; }

        public string Tag { get; set; }

        #endregion
    }
}
