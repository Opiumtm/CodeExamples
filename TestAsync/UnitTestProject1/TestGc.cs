using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestProject1
{
    [TestClass]
    public class TestGc
    {
        public AutoResetEvent e;
        public WeakReference<AwaiterObj> a;
        public AwaiterObj ao;


        private void Prepare()
        {
            e = new AutoResetEvent(false);
            ao = new AwaiterObj();
            ao.AwaitOn(TestEventHelpers.AwaitEvent(e));
            a = new WeakReference<AwaiterObj>(ao);
        }

        private async Task Cleanup()
        {
            e.Set();
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            e.Dispose();
            ao = null;
            a = null;
            WaitForGc();
        }

        [TestMethod]
        public async Task TestTask1()
        {
            Prepare();
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(true, "Есть твёрдая ссылка");

                ao = null;

                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(true, "Нет твёрдой ссылки, ожидание не завершено");

                e.Set();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(false, "Ожидание завершено");
            }
            finally
            {
                await Cleanup();
            }
        }

        [TestMethod]
        public async Task TestTask2()
        {
            Prepare();
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(true, "Есть твёрдая ссылка");
                Assert.IsFalse(ao.IsAwaited, "Не дождался");

                e.Set();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(true, "Есть твёрдая ссылка-2");
                Assert.IsTrue(ao.IsAwaited, "Дождался");

                ao = null;

                await Task.Delay(TimeSpan.FromSeconds(0.5));
                WaitForGc();
                TestObject(false, "Нет твёрдой ссылки");
            }
            finally
            {
                await Cleanup();
            }
        }

        private void TestObject(bool shouldPresent, string msg)
        {
            var r = GetReference(a);
            if (shouldPresent)
            {
                Assert.IsNotNull(r, msg);
            }
            else
            {
                Assert.IsNull(r, msg);
            }
        }

        private T GetReference<T>(WeakReference<T> r) where T : class
        {
            T i;
            if (r.TryGetTarget(out i))
            {
                return i;
            }
            return null;
        }

        private void WaitForGc()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }
    }

    public static class TestEventHelpers
    {
        public static Task AwaitEvent(AutoResetEvent e)
        {
            var tcs = new TaskCompletionSource<bool>();
            Task.Run(() =>
            {
                try
                {
                    e.WaitOne();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
    }

    public class AwaiterObj
    {
        public async void AwaitOn(Task task)
        {
            try
            {
                await task;
                IsAwaited = true;
            }
            catch (Exception ex)
            {
                // Ignore
            }
        }

        public bool IsAwaited;
    }
}