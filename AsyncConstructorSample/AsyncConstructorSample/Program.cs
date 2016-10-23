using System;
using System.Threading.Tasks;

namespace AsyncConstructorSample
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFunc().Wait();
        }

        private static async Task TestFunc()
        {
            // var test = await new AsyncConstructedClass(3);
            // Would translate to code below:
            Task _asyncToken;
            var test = new AsyncConstructedClass(out _asyncToken, 3);
            await _asyncToken;
            test.HelloWorld();
        }
    }

    public class AsyncConstructedClass
    {
        /*        
        public async AsyncConstructedClass(int sleepSec)
        {
            _sleepSec = sleepSec;
            await Task.Delay(TimeSpan.FromSeconds(sleepSec));
        }

        Would translate to code below:        
        */

        public AsyncConstructedClass(out Task _asyncToken, int sleepSec)
        {
            _sleepSec = sleepSec; // readonly variables could be assigned only BEFORE first "await" in async constructor 
                                  // as anything starting with first await would be placed inside local labmda function
            Func<Task> _localMethod = async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(sleepSec));
                // _sleepSec = sleepSec; - invalid as it's placed inside lambda and readonly field isn't accessible here
            };
            _asyncToken = _localMethod();
        }

        private readonly int _sleepSec;

        public void HelloWorld()
        {
            Console.WriteLine($"I'm constructed asynchronously with {_sleepSec} sec wait");
        }
    }
}
