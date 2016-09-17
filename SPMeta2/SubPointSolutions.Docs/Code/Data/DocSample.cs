using SPMeta2.Utils;
using System.Collections.Generic;
using System.Text;
namespace SubPointSolutions.Docs.Code.Data
{
    public class DocSampleTag
    {
        public DocSampleTag()
        {
            Values = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Values { get; set; }
    }

    public class DocSample
    {
        public DocSample()
        {
            Tags = new List<DocSampleTag>();
            
            IsMethod = true;
            IsClass = false;
        }

        #region properties

        public bool IsMethod { get; set; }
        public bool IsClass { get; set; }

        public List<DocSampleTag> Tags { get; set; }

        public string Scope { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string MethodBodyWithFunction { get; set; }
        public string MethodBody { get; set; }
        public string Language { get; set; }

        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }

        public string ClassComment { get; set; }

        public string MethodFullName { get; set; }

        public string ClassFullName { get; set; }


        public int MethodParametersCount { get; set; }

        public string SourceFileName { get; set; }

        public string SourceFileNameWithoutExtension { get; set; }

        public string SourceFileFolder { get; set; }
        public string SourceFilePath { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(MethodBody))
                return MethodBody;

            return base.ToString();
        }

        public static DocSample FromXml(string xml)
        {
            return XmlSerializerUtils.DeserializeFromString<DocSample>(xml);
        }

        public string ToXml()
        {
            return XmlSerializerUtils.SerializeToString(this);
        }

        #endregion


    }
}
