using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPMeta2.Containers.Services
{
    public abstract class RandomService
    {
        static RandomService()
        {
            var _localeIds = new List<int>();

            //- German
            //- French
            //- Dutch
            //- Italian
            //- Russian
            //- Spanish 
            //- Swedish 

            var languages = new int[]
                {
                    1031,
                    1036,
                   // 1043,
                    //1040,
                    1049,
                   // 1034,
                    //1053
                };

            _localeIds.AddRange(languages);

            DefaultLocaleIds = _localeIds;
        }

        public static IEnumerable<int> DefaultLocaleIds { get; set; }

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
        public abstract string ActiveDirectoryGroup();
        public abstract string UserName();

        public abstract string DbServerName();

        public abstract string UserEmail();

        public abstract string ManagedPath();
    }

    public static class RandomServiceExtensions
    {
        #region localization

        public static IEnumerable<int> LocaleIds(this RandomService service)
        {
            return RandomService.DefaultLocaleIds;
        }

        public static int LocaleId(this RandomService service)
        {
            return LocaleId(service, RandomService.DefaultLocaleIds);
        }

        public static int LocaleId(this RandomService service, IEnumerable<int> localeIds)
        {
            return service.RandomFromArray(localeIds);
        }

        #endregion

        #region methods

        public static string VersionString(this RandomService service)
        {
            return new Version(service.Int(10), service.Int(10), service.Int(10), service.Int(10)).ToString();
        }

        public static bool? NullableBool(this RandomService service)
        {
            if (service.Bool())
                return service.Bool();

            return null;
        }

        public static T RandomEnumValue<T>(this RandomService service)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        public static T RandomFromArray<T>(this RandomService service, IEnumerable<T> array)
        {
            return array.ToList()[service.Int(array.Count() - 1)];
        }

        public static IEnumerable<T> RandomArrayFromArray<T>(this RandomService service, IEnumerable<T> array)
        {
            if (array.Count() == 0)
                return Enumerable.Empty<T>();

            var resultArray = new List<T>();
            var itemsCount = service.Int(array.Count() - 1);

            if (itemsCount == 0)
                itemsCount = array.Count() / 2;

            for (var i = 0; i < itemsCount; i++)
            {
                var newItem = array.ToList()[service.Int(array.Count() - 1)];

                if (!resultArray.Contains(newItem))
                    resultArray.Add(newItem);
            }

            return resultArray;
        }

        public static List<T> RandomListFromArray<T>(this RandomService service, IEnumerable<T> array)
        {
            var result = new List<T>();

            foreach (var value in RandomArrayFromArray<T>(service, array))
                result.Add(value);

            return result;
        }

        public static Collection<T> RandomCollectionFromArray<T>(this RandomService service, IEnumerable<T> array)
        {
            var result = new Collection<T>();

            foreach (var value in RandomArrayFromArray<T>(service, array))
                result.Add(value);

            return result;
        }

        public static string HttpUrl(this RandomService service)
        {
            return string.Format("http://url-{0}.com", service.String(8));
        }

        public static string HttpsUrl(this RandomService service)
        {
            return string.Format("https://url-{0}.com", service.String(8));
        }

        public static string Email(this RandomService service)
        {
            return string.Format("{0}@{1}.com", service.String(8), service.String(3));
        }

        public static short Short(this RandomService service)
        {
            return service.Short(short.MaxValue);
        }

        public static short Short(this RandomService service, short maxValue)
        {
            return (short)service.Int(maxValue);
        }

        public static byte Byte(this RandomService service)
        {
            return service.Byte(byte.MaxValue);
        }

        public static byte[] ByteArray(this RandomService service)
        {
            return ByteArray(service, 64);
        }

        public static byte[] ByteArray(this RandomService service, int length)
        {
            var result = new List<byte>();

            for (var i = 0; i < length; i++)
                result.Add(service.Byte());

            return result.ToArray();
        }

        public static byte Byte(this RandomService service, byte maxValue)
        {
            return (byte)service.Int(maxValue);
        }

        public static string CamlQuery(this RandomService service)
        {
            return string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>",
                        service.String());
        }

        public static string TxtFileName(this RandomService service)
        {
            return string.Format("{0}.txt", service.String());
        }

        public static string DocxFileName(this RandomService service)
        {
            return string.Format("{0}.docx", service.String());
        }

        public static string WspFileName(this RandomService service)
        {
            return string.Format("{0}.wsp", service.String());
        }

        public static string AspxFileName(this RandomService service)
        {
            return string.Format("{0}.aspx", service.String());
        }

        public static string XmlFileName(this RandomService service)
        {
            return string.Format("{0}.xml", service.String());
        }

        public static uint UInt(this RandomService service)
        {
            return service.UInt(uint.MaxValue);
        }

        public static uint UInt(this RandomService service, uint maxValue)
        {
            return (uint)service.Int((int)maxValue);
        }

        public static DateTime Today(this RandomService service)
        {
            return DateTime.Now;
        }

        public static DateTime TodayUtc(this RandomService service)
        {
            return DateTime.UtcNow;
        }

        public static DateTime Date(this RandomService service)
        {
            return service.Date(365);
        }

        public static DateTime Date(this RandomService service, int days)
        {
            return DateTime.Now.AddDays(service.Int(days));
        }

        public static DateTime DateUtc(this RandomService service)
        {
            return service.DateUtc(365);
        }

        public static DateTime DateUtc(this RandomService service, int days)
        {
            return DateTime.UtcNow.AddDays(service.Int(days));
        }

        #endregion


        public static string DomainUserEmail(this RandomService service)
        {
            var emailString = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultTestDomainUserEmails);

            if (!string.IsNullOrEmpty(emailString))
            {
                var emails = emailString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return emails[service.Int(emails.Length)];
            }

            throw new Exception(string.Format("Environment value [{0}] is NULL", EnvironmentConsts.DefaultTestDomainUserEmails));
        }
    }
}
