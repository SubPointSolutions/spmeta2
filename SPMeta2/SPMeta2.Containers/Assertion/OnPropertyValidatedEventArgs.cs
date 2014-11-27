using System;

namespace SPMeta2.Containers.Assertion
{
    public class OnPropertyValidatedEventArgs : EventArgs
    {
        public PropertyValidationResult Result { get; set; }
    }
}
