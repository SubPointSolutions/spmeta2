using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Assertion
{
    public class OnPropertyValidatedEventArgs : EventArgs
    {
        public PropertyValidationResult Result { get; set; }
    }
}
