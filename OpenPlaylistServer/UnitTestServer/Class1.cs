using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestServer
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(5, Add(3, 2));
        }

        [Fact]
        public void FaillingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
