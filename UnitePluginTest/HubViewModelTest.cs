using System;
using Xunit;
using UnitePlugin.ViewModel;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Display;

namespace UnitePluginTest
{
    public class HubViewModelTest
    {
        [Fact]
        public void Constructor()
        {
            HubViewModel hubViewModel = new HubViewModel();

            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000000"), hubViewModel.ControlIdentifier);
            //Assert.False(hubViewModel.IsAllocated, "view should not be allocated");
        }

        [Fact]
        public void AllocatedCallBackSuccess()
        {
            HubViewModel hubViewModel = new HubViewModel();

            HubAllocationResult result = new HubAllocationResult
            {
                AllocatedView = new DisplayView { Id = new Guid { } },
                ResultType = HubAllocationResultType.Success,
                Success = true,
            };

            //hubViewModel.AllocatedCallBack(result);

            //Assert.Equal(result.AllocatedView.Id, hubViewModel.ControlIdentifier);
            //Assert.True(hubViewModel.IsAllocated, "view should be allocated");
        }

        [Fact]
        public void AllocatedCallBackFail()
        {
            PhysicalDisplay display = new PhysicalDisplay() { Id = new Guid() };
            HubViewModel hubViewModel = new HubViewModel();

            HubAllocationResult result = new HubAllocationResult
            {
                AllocatedView = new DisplayView { Id = new Guid { } },
                ResultType = HubAllocationResultType.InternalError,
                Success = false,
            };

            //Exception ex = Assert.Throws<Exception>(() => hubViewModel.AllocatedCallBack(result));

            //Assert.Equal(result.ResultType.ToString(), ex.Message);
            //Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000000"), hubViewModel.ControlIdentifier);
            //Assert.False(hubViewModel.IsAllocated, "view should not be allocated");
        }

    }
}
