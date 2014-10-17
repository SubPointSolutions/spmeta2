using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Common
{
    /// <summary>
    /// Reflects a particular model provisioning event.
    /// </summary>
    public enum ModelEventType
    {
        /// <summary>
        /// Invalid flag.
        /// </summary>
        Unknown,

        /// <summary>
        /// Before model provisioning.
        /// Provision engine has already tried to find the artifact, but has not started updating and provisioning process.
        /// If artifact exists, it is passed to the model event.
        /// If not, NULL reference is passed instead.
        /// </summary>
        OnProvisioning,

        /// <summary>
        /// Before saving model updates.
        /// Provision engine has already tried to find the artifact, created new if it did not exist or updated existing otherwise.
        /// Artifact must always be a valid not-null reference. 
        /// </summary>
        OnProvisioned,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnUpdating,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnUpdated,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnRetracting,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnRetracted,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnError,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnModelHostResolving,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnModelHostResolved,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnModelHostActionInvoking,

        /// <summary>
        /// Reserved for the future usage.
        /// </summary>
        OnModelHostActionInvoked
    }
}
