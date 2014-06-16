using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.SSOM.Utils
{
    public class ComparePair<TModel, TObj>
    {
        public ComparePair(TModel model, TObj obj)
        {
            Model = model;
            Object = obj;
        }

        public TModel Model { get; set; }
        public TObj Object { get; set; }
    }
}
