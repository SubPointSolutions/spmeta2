using SPMeta2.Utils;

namespace SPMeta2.Containers.Assertion
{
    public class PropertyValidationResult
    {
        public object Tag { get; set; }

        public PropResult Src { get; set; }
        public PropResult Dst { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
}
