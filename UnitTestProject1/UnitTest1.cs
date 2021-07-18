using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		Calculator _calc;

		public UnitTest1()
		{
			_calc = new Calculator();
		}
		[TestMethod]
		public void shouldaddtwonumbers()
		{
			int res = _calc.Add(5, 4);
			Assert.AreEqual(res, 9);
		}
		[TestMethod]
		public void shouldsubtwonumbers()
		{
			int res = _calc.Sub(5, 3);
			Assert.AreEqual(res, 2);
		}
	}
}
