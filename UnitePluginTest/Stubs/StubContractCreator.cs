using Intel.Unite.Common.Module.Common;
using System.AddIn.Pipeline;
using System.Windows;
using System.Windows.Threading;

namespace UnitePluginTest.Stubs
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>	A contract creator. </summary>
    ///
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class StubContractCreator
    {
        /// <summary>	The dispatcher. </summary>
        private static Dispatcher _dispatcher;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 	Creates contract for framework element, if the creation fails it will return a null
        /// 	contract.
        /// </summary>
        ///
        /// 
        ///
        /// <param name="moduleUi">	. </param>
        /// <seealso cref="FrameworkElement"/>
        ///
        /// <returns>	The new contract. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static MarshalNativeHandleContract CreateContract(FrameworkElement moduleUi)
        {
            MarshalNativeHandleContract contract = null;
            _dispatcher.Invoke(delegate
            {
                var localContract = FrameworkElementAdapters.ViewToContractAdapter(moduleUi);
                contract = new MarshalNativeHandleContract(localContract);

            });
            return contract;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 	WARNING: Not to be used by plug-in developers, it will ignore those calls. Sets the
        /// 	current UI dispatcher.
        /// </summary>
        ///
        /// 
        ///
        /// <param name="currentDispatcher">	Current UI dispatcher. </param>
        /// <seealso cref="Dispatcher"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void SetUpDispatcher(Dispatcher currentDispatcher)
        {
            _dispatcher = currentDispatcher;
        }
    }
}
