using Unite.Test.Extensions.TestFixtures;
using Xunit;

namespace UnitePluginTest.CollectionDefinition
{
    // Important note: Fixtures can be shared across assemblies, but collection definitions must be in the same assembly as the test that uses them.
    // https://xunit.net/docs/shared-context
    [CollectionDefinition("Ui collection")]
    public class UiCollection : ICollectionFixture<UiThreadFixture>
    {

    }
}