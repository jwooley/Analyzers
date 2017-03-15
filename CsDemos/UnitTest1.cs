using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic customers = new System.Dynamic.ExpandoObject();
            if (customers != null)
            {
                foreach(var c in customers)
                {
                    DoSomethingComplex(c);
                    foreach(var o in c.Orders)
                    {
                        DoSomethingComplex(o);
                        if (o.Total > 1000)
                        {
                            // do nothing
                        }
                        else
                        {
                            DoSomethingComplex(o.OrderDetails);
                            while (true)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void DoSomethingComplex(object x)
        {
            Console.WriteLine(x.ToString());
        }
    }
}
