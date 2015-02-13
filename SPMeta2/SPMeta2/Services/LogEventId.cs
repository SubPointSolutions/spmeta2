using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    /// <summary>
    /// Log events provided by SPMeta2 implementation and related components.
    ///
    /// Range 0-9999 is reserved for SPMeta.
    /// Please use LogEventId.Custom + XXX to log your own events.
    /// </summary>
    public enum LogEventId : int
    {
        Unknown = 0,

        Initialization = 100,

        InternalCall = 250,
        InternalEventCall = 251,

        CoreCalls = 500,

        ModelDefinition = 1000,

        ModelTree = 2000,
        ModelTreeModelContextEventIsNull = 2001,

        ModelProcessing = 3000,

        ModelProcessingNullModelHandler = 3001,
        ModelProcessingError = 3002,

        ModelProvision = 4000,
        ModelProvisionCoreCall = 4001,
        ModelProvisionProcessingNewObject = 4002,
        ModelProvisionProcessingExistingObject = 4003,

        ModelProvisionUpdatingEventCall = 4004,
        ModelProvisionUpdatedEventCall = 4005,

        Custom = 10000
    }

    // model definition
    // model build
    // model processing
    // model provision
}
