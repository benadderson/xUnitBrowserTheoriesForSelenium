using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace xUnitBrowserTheories
{
    public class BrowserTheoryAttribute: TheoryAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(IMethodInfo method)
        {
            bool foundData = false;
            List<ITestCommand> results = new List<ITestCommand>();

            try
            {
                foreach (object[] dataItems in GetData(method.MethodInfo))
                {
                    foundData = true;
                    results.Add(new TheoryCommand(method, DisplayName, dataItems));
                }

                if (!foundData)
                    results.Add(new LambdaTestCommand(method, () =>
                    {
                        throw new InvalidOperationException(string.Format("No data found for {0}.{1}",
                                                                          method.TypeName,
                                                                          method.Name));
                    }));
            }
            catch (Exception ex)
            {
                results.Clear();
                results.Add(new LambdaTestCommand(method, () =>
                {
                    throw new InvalidOperationException(string.Format("An exception was thrown while getting data for theory {0}.{1}:\r\n" + ex.ToString(),
                                                                      method.TypeName,
                                                                      method.Name));
                }));
            }

            return results;
        }

        static IEnumerable<object[]> GetData(MethodInfo method)
        {
            foreach (BrowserAttribute attr in method.GetCustomAttributes(typeof(BrowserAttribute), false))
            {
                ParameterInfo[] parameterInfos = method.GetParameters();
                Type[] parameterTypes = new Type[parameterInfos.Length];

                for (int idx = 0; idx < parameterInfos.Length; idx++)
                    parameterTypes[idx] = parameterInfos[idx].ParameterType;

                if(attr._url != null)
                {
                    IEnumerable<object[]> browserAttrData = attr.GetData(method, parameterTypes);
                    if (browserAttrData != null)
                        foreach (object[] dataItems in browserAttrData)
                            yield return dataItems;
                }

                else foreach (URLAttribute attribute in method.GetCustomAttributes(typeof(URLAttribute), false))
                {
                    attr._url = attribute.Url;
                    IEnumerable<object[]> browserAttrData = attr.GetData(method, parameterTypes);
                    if (browserAttrData != null)
                        foreach (object[] dataItems in browserAttrData)
                            yield return dataItems;
                }
            }
        }

        class LambdaTestCommand : TestCommand
        {
            readonly Assert.ThrowsDelegate lambda;

            public LambdaTestCommand(IMethodInfo method, Assert.ThrowsDelegate lambda)
                : base(method)
            {
                this.lambda = lambda;
            }

            public override bool ShouldCreateInstance
            {
                get { return false; }
            }

            public override MethodResult Execute(object testClass)
            {
                try
                {
                    lambda();
                    return new PassedResult(testMethod, DisplayName);
                }
                catch (Exception ex)
                {
                    return new FailedResult(testMethod, ex, DisplayName);
                }
            }
        }
    }
}
