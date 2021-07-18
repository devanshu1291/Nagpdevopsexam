using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
	public class Calculator
	{
        public int Add(int num1, int num2)
        {
            int result = num1 + num2;
            return result;
        }

        public int Sub(int num1, int num2)
        {
            int result = num1 - num2;
            return result;
        }
    }
}