using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Services
{
    public abstract class RandomService
    {
        public abstract Guid Guid();

        public abstract bool Bool();

        public abstract string String();
        public abstract string String(int lenght);

        public abstract int Int();
        public abstract int Int(int maxValue);

        public abstract double Double();
        public abstract double Double(double maxValue);
    }
}
