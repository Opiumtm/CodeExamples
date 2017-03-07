using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestProject1
{
    [TestClass]
    public class TestDispatcherExceptions
    {
        [TestMethod]
        public async Task TestDispatcherException()
        {
            try
            {
                await ThrowExceptionDispatcher();
            }
            catch (TestException ex)
            {
                Assert.AreEqual(ex.Data, "TEST");
            }
        }

        [TestMethod]
        public async Task TestDispatcherException2()
        {
            try
            {
                await ThrowExceptionDispatcher2();
            }
            catch (TestException ex)
            {
                Assert.AreEqual(ex.Data, "TEST");
            }
        }

        private Task ThrowExceptionDispatcher()
        {
            return Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ThrowException();
            }).AsTask();
        }

        private IAsyncAction ThrowExceptionDispatcher2()
        {
            return Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ThrowException();
            });
        }

        private CoreDispatcher Dispatcher
        {
            get { return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher; }
        }

        private void ThrowException()
        {
            throw new TestException() { Data = "TEST"};
        }
    }

    public class TestException : Exception
    {
        public string Data { get; set; }
    }
}