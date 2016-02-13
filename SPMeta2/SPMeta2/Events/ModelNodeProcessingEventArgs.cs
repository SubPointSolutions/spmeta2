using System;
using SPMeta2.Models;

namespace SPMeta2.Events
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
    public class ModelNodeProcessingEventArgs : EventArgs
    {
        public ModelNode ModelNode { get; set; }
        public object ModelHost { get; set; }
    }
}
