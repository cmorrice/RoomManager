using Intel.Unite.Common.Command;

namespace UnitePluginTest.Helpers
{
    public static class TestHelper
    {


        public static class Compare
        {
            public static bool AreMessagesEqual(Message expectedMessage, Message actualMessage)
            {
                if ( expectedMessage.Priority != actualMessage.Priority || 
                    expectedMessage.DataType != actualMessage.DataType || 
                    expectedMessage.SourceModuleId != actualMessage.SourceModuleId  )
                    return false;

                return true;
            }
        }

    }
}
