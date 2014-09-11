using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Common
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
    public enum ModelEventType
    {
        Unknown,

        OnProvisioning,
        OnProvisioned,

        OnUpdating,
        OnUpdated,

        OnRetracting,
        OnRetracted,

        OnError
    }
}
