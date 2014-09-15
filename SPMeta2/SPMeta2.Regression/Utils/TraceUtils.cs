using System;
using System.Diagnostics;

namespace SPMeta2.Regression.Utils
{
    public class TraceUtils
    {
        #region static

        public static void WithScope(Action<TraceUtils> action)
        {
            action(new TraceUtils());
        }

        #endregion

        #region properties

        protected int IndentIndex { get; set; }

        #endregion

        #region methods

        protected string GetCurrentIndent()
        {
            var result = string.Empty;

            for (var i = 0; i < IndentIndex; i++)
                result += "    ";

            return result;
        }

        public TraceUtils WriteLine(string traceMessage)
        {
            Trace.WriteLine(string.Format("{0}{1}", GetCurrentIndent(), traceMessage));

            return this;
        }

        public TraceUtils WithTraceIndent(Action<TraceUtils> action)
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
