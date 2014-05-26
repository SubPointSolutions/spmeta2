using System;
using System.IO;
using SPMeta2.Regression.Common.Utils;


namespace SPMeta2.Regression.Profiles
{
    public static class ProfileHelper
    {
        #region methods

        public static TProfile LoadCurrentProfile<TProfile>()
            where TProfile : TestProfileBase, new()
        {
            TProfile result = null;

            var className = typeof(TProfile).Name;
            var machineProfileFileName = string.Format(@"SPMetaRegressionProfiles/{0}.{1}.xml", Environment.MachineName, className);

            if (File.Exists(machineProfileFileName))
            {
                // pragmatic way
                var profileXml = File
                                    .ReadAllText(machineProfileFileName)
                                    .Replace("{$MachineName$}", Environment.MachineName);

                result = XmlSerializerUtils.DeserializeFromString<TProfile>(profileXml);
            }

            if (result != null) return result;

            return new TProfile();
        }

        #endregion
    }
}
