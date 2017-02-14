using SPMeta2.Services;
using System;
using System.Diagnostics;

namespace SPMeta2.Containers.Utils
{
    public class IndentableTrace
    {
        #region static

        static IndentableTrace()
        {
            DefaultIndentString = "    ";
        }


        public static string DefaultIndentString { get; set; }

        public static void WithScope(Action<IndentableTrace> action)
        {
            action(new IndentableTrace());
        }


        #endregion

        #region properties

        protected int IndentIndex { get; set; }
        protected string IndentString { get; set; }

        #endregion

        #region methods

        protected string GetCurrentIndent()
        {
            var result = string.Empty;

            for (var i = 0; i < IndentIndex; i++)
                result += (IndentString ?? DefaultIndentString);

            return result;
        }

        public IndentableTrace WriteLine(string traceMessage)
        {
            var m2logService = ServiceContainer.Instance.GetService<TraceServiceBase>();

            m2logService.Information(0, string.Format("{0}{1}", GetCurrentIndent(), traceMessage));

            return this;
        }

        public IndentableTrace WithTraceIndent(Action<IndentableTrace> action)
        {
            try
            {
                IndentIndex++;
                action(this);
            }
            finally
            {
                IndentIndex--;
            }

            return this;
        }

        #endregion
    }
}
