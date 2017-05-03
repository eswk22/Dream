using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Helper
{
    public class AssertHelper
    {
        public static void HasEqualFieldValues<T>(T expected, T actual)
        {
            var failures = new List<string>();
            var fields = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                var v1 = field.GetValue(expected);
                var v2 = field.GetValue(actual);
                if (v1 == null && v2 == null) continue;
                else if((v1== null && v2!= null) || (v1!= null && v2== null))
                    failures.Add(string.Format("{0}: Expected:<{1}> Actual:<{2}>", field.Name, v1, v2));
                else if (!v1.Equals(v2)) failures.Add(string.Format("{0}: Expected:<{1}> Actual:<{2}>", field.Name, v1, v2));
            }
            if (failures.Any())
                Assert.Fail("AssertHelper.HasEqualFieldValues failed. " + Environment.NewLine + string.Join(Environment.NewLine, failures));
        }
    }

}
