using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnvironmentVariablesService.Tests
{
    [TestClass]
    public class ServiceExtensionsTests
    {

        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable(TestScopeVariables.STRING_VALUE_NAME, "STRING_VALUE from env");
            Environment.SetEnvironmentVariable(TestScopeVariables.INT_VALUE_NAME, TestScopeVariables.DefaultIntValue.ToString());
            Environment.SetEnvironmentVariable(TestScopeVariables.NULLABLE_INT_VALUE_NAME, TestScopeVariables.DefaultNullableIntValue.ToString());
        }

        [TestMethod]
        public void DefaultValuesAreReadable()
        {
            var services = new ServiceCollection()
                .AddEnvVariables()
                    .AddScope<TestScopeVariables>()
                    .Read();

            var provider = services.BuildServiceProvider();

            var env = provider.GetService<TestScopeVariables>();

            Assert.AreEqual("STRING_VALUE from env", env.STRING_VALUE);
            Assert.AreEqual(TestScopeVariables.DefaultIntValue, env.INT_VALUE);
            Assert.AreEqual(TestScopeVariables.DefaultNullableIntValue, env.NULLABLE_INT_VALUE);
        }

        [TestMethod]
        public void Int()
        {
            Environment.SetEnvironmentVariable(TestScopeVariables.INT_VALUE_NAME, "1");
            var services = new ServiceCollection()
                .AddEnvVariables()
                    .AddScope<TestScopeVariables>()
                    .Read();

            var provider = services.BuildServiceProvider();

            var env = provider.GetService<TestScopeVariables>();

            Assert.AreEqual(1, env.INT_VALUE);
        }

        [TestMethod]
        public void NullableInt_Null()
        {
            Environment.SetEnvironmentVariable(TestScopeVariables.NULLABLE_INT_VALUE_NAME, null);
            var services = new ServiceCollection()
                .AddEnvVariables()
                    .AddScope<TestScopeVariables>()
                    .Read();

            var provider = services.BuildServiceProvider();

            var env = provider.GetService<TestScopeVariables>();

            Assert.AreEqual(TestScopeVariables.DefaultNullableIntValue, env.NULLABLE_INT_VALUE);
        }

        [TestMethod]
        public void NullableInt_NotNull()
        {
            Environment.SetEnvironmentVariable(TestScopeVariables.NULLABLE_INT_VALUE_NAME, "8712");
            var services = new ServiceCollection()
                .AddEnvVariables()
                    .AddScope<TestScopeVariables>()
                    .Read();

            var provider = services.BuildServiceProvider();

            var env = provider.GetService<TestScopeVariables>();

            Assert.AreEqual((int?)8712, env.NULLABLE_INT_VALUE);
        }
    }
}
