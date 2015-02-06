using System;
using System.Text;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Utils;

namespace SPMeta2.Containers.Services.Rnd
{
    public class DefaultRandomService : RandomService
    {
        double _defaultTrueProbability = 0.49;
        private Random _rnd = new Random();

        public override Guid Guid()
        {
            return System.Guid.NewGuid();
        }

        public override string String()
        {
            return String(32);
        }

        public override string String(long lenght)
        {
            var iterations = (lenght / 32) + 1;

            var result = string.Empty;

            for (var i = 0; i < iterations; i++)
                result += System.Guid.NewGuid().ToString("N");

            return result.Substring(0, (int)lenght);
        }

        public override int Int()
        {
            return Int(int.MaxValue);
        }

        public override int Int(int maxValue)
        {
            return _rnd.Next(maxValue);
        }

        public override double Double()
        {
            return Double(double.MaxValue);
        }

        public override double Double(double maxValue)
        {
            return _rnd.NextDouble() * maxValue;
        }

        private bool _boolFlag = false;

        public override bool Bool()
        {

            return _rnd.NextDouble() < _defaultTrueProbability;
        }

        public override string UserLogin()
        {
            var userLogins = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultTestUserLogins);

            if (!string.IsNullOrEmpty(userLogins))
            {
                var logins = userLogins.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return logins[Int(logins.Length)];
            }

            throw new Exception(string.Format("Environment value [{0}] is NULL", EnvironmentConsts.DefaultTestUserLogins));
        }

        public override string UserEmail()
        {
            return string.Format("{0}@{0}.com", Environment.UserName);
        }

        public override string UserName()
        {
            return string.Format("{0}", Environment.UserName);
        }

        public override string ManagedPath()
        {
            return "sites";
        }

        public override byte[] Content()
        {
            return Content(32);
        }

        public override byte[] Content(long lenght)
        {
            return Encoding.UTF8.GetBytes(String(lenght));
        }

        public override string DbServerName()
        {
            return string.Format("{0}", RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultSqlServerName));
        }

        public override string ActiveDirectoryGroup()
        {
            var groups = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultTestADGroups);

            if (!string.IsNullOrEmpty(groups))
            {
                var groupValues = groups.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return groupValues[Int(groupValues.Length)];
            }

            throw new Exception(string.Format("Environment value [{0}] is NULL", EnvironmentConsts.DefaultTestADGroups));
        }
    }
}
