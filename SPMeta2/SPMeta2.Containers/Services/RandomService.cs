using System;

namespace SPMeta2.Containers.Services
{
    public abstract class RandomService
    {
        public abstract Guid Guid();

        public abstract byte[] Content();
        public abstract byte[] Content(long lenght);

        public abstract bool Bool();

        public abstract string String();
        public abstract string String(long lenght);

        public abstract int Int();
        public abstract int Int(int maxValue);


        public abstract double Double();
        public abstract double Double(double maxValue);

        public abstract string UserLogin();
        public abstract string UserName();

        public abstract string DbServerName();

        public abstract string UserEmail();

        public abstract string ManagedPath();
    }

    public static class RandomServiceExtensions
    {
        #region methods

        public static short Short(this RandomService service)
        {
            return service.Short(short.MaxValue);
        }

        public static short Short(this RandomService service, short maxValue)
        {
            return (short)service.Int(maxValue);
        }


        public static uint UInt(this RandomService service)
        {
            return service.UInt(uint.MaxValue);
        }

        public static uint UInt(this RandomService service, uint maxValue)
        {
            return (uint)service.Int((int)maxValue);
        }

        #endregion
    }

    //public abstract int Short();
    //  public abstract int Short(int maxValue);

}
