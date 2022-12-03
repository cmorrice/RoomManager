using Intel.Unite.Common.Command;
using Xunit;

namespace UnitePluginTest.Helpers
{
    public static class EqualHelper
    {
        public static void AssertMessage(Message expectedMessage, Message actualMessage)
        {
            Assert.Equal(expectedMessage.Priority, actualMessage.Priority);
            Assert.Equal(expectedMessage.DataType, actualMessage.DataType);
            Assert.Equal(expectedMessage.SourceModuleId, actualMessage.SourceModuleId);
        }
    }
}