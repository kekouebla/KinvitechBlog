using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TDD.XUnit.Example
{
    public class TestCaseTest : Test
    {
        public TestCaseTest()
        {
            Print_None_Before_The_Method_WasRun();

            this.WarRun("testMethod");

            Print_1_After_The_Method_WasRun();
        }

        [Fact]
        public void Print_None_Before_The_Method_WasRun()
        {
            var printNone = "None";
            Console.WriteLine(printNone);
            Assert.Equal("None", printNone);
        }

        [Fact]
        public void Print_1_After_The_Method_WasRun()
        {
            var print1 = "1";
            Console.WriteLine(print1);
            Assert.Equal("1", print1);
        }

        public override string WarRun(string v)
        {
            return v;
        }
    }
}
