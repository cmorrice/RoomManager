using Intel.Unite.Common.Context;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Sensor;

namespace UnitePluginTest.Stubs
{
    public class StubRuntimeContext : ModuleRuntimeContext, IHubModuleRuntimeContext
    {
        public new IHubSessionContext SessionContext { get; set; }
        public ISensorManager SensorManager { get; set; }
    }
}