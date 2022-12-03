using System;
using System.Collections.Generic;
using Intel.Unite.Common.Display;

namespace UnitePluginTest.Stubs
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>	A DisplayView container. </summary>
    ///
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class StubDisplayViews
    {
        public readonly List<DisplayView> DisplayViews = new List<DisplayView>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 	Adds the provided DisplayView to the DisplayViews collection.  This is a Handler for the
        ///     ViewAllocated EventHandler on the DisplayManager.
        /// </summary>
        ///
        /// 
        ///
        /// <param name="sender">	the object invoking the handler </param>
        /// <param name="displayView">	the DisplayView that is being added </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void Add(object sender, DisplayView displayView)
        {
            DisplayViews.Add(displayView);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 	Removes the provided DisplayView from the DisplayViews collection.  This is a Handler for
        ///     the ViewDeallocated EventHandler on the DisplayManager.
        /// </summary>
        ///
        /// 
        ///
        /// <param name="sender">	the object invoking the handler </param>
        /// <param name="viewId">	the ViewId Guid of the DisplayView that is being removed </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void Remove(object sender, Guid viewId)
        {
            DisplayViews.RemoveAll(displayView => displayView.Id == viewId);
        }
    }
}