using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Validation.Common
{
    public enum ValidationResultType
    {
        MustBeString,
        NotEmptyString,
        NotNullString,
        NoSpacesBeforeOrAfter,
        NoMoreThan,
        NotDefaultGuid,
        NotEqual
    }

    public class ValidationResult
    {
        public string PropertyName { get; set; }
        public ValidationResultType ResultType { get; set; }

        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
