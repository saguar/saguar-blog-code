using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saguar.MvvmUtils.Helpers;
using System.Threading;

namespace MvvmUtils.Test
{
    [TestClass]
    public class HelperMethodsTest
    {

        //Since I istantiate the HelperMethods class with <object> type, the istance will
        //be the same independently of the methods return type
        private HelperMethods<object> _helperMethods => HelperMethods<object>.Current;

        [TestMethod]
        public async Task TestAsyncMethodsAsync()
        {
            //define a generic result object
            object result = null;

            result = await _helperMethods.ExecuteAsync(() => LongWritingOperation());
            Assert.IsNotNull(result);
            //convert the result to string type
            var strRes = result as string;
            Console.WriteLine($"first result : {strRes}");

            result = await _helperMethods.ExecuteAsync(() => LongCalculatingOperation());
            Assert.IsNotNull(result);
            //convert the result to int type
            var intRes = (int)result;
            Console.WriteLine($"second result : {intRes}");

            result = await _helperMethods.ExecuteAsync(() => LongTimeOperation());
            Assert.IsNotNull(result);
            //convert the result to datetime type
            var dateRes = result as DateTime?;
            Console.WriteLine($"third result : {dateRes.Value.ToShortDateString()}");

        }

        //synchronous methods to be tested
        private string LongWritingOperation()
        {
            Thread.Sleep(2000);
            return $"LongWritingOperation {new Random().Next(1, 100)} result";
        }

        private int LongCalculatingOperation()
        {
            Thread.Sleep(2000);
            return new Random().Next(1, 100);
        }

        private DateTime LongTimeOperation()
        {
            Thread.Sleep(2000);
            return DateTime.Now;
        }

    }
}
