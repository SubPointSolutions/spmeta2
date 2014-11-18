using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services
{
    public abstract class TraceServiceBase
    {
        #region methods

        public virtual void Critical(int id, object message)
        {
            Critical(id, message, null);
        }

        public abstract void Critical(int id, object message, Exception exception);

        public virtual void Error(int id, object message)
        {
            Error(id, message, null);
        }

        public abstract void Error(int id, object message, Exception exception);

        public virtual void Warning(int id, object message)
        {
            Warning(id, message, null);
        }

        public abstract void Warning(int id, object message, Exception exception);

        public virtual void Information(int id, object message)
        {
            Information(id, message, null);
        }

        public abstract void Information(int id, object message, Exception exception);


        public virtual void Verbose(int id, object message)
        {
            Verbose(id, message, null);
        }

        public abstract void Verbose(int id, object message, Exception exception);

        #endregion
    }
}
