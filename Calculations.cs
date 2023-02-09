using System;

namespace calc_lib
{
    public class Calculations
    {
        /*
         * 1. Basic maths (baby mode):
         * 
         * - Basic integer operations (done)
         * - Basic fraction operations
         * - Decimal rounding
         * - Mean, median, and mode
         */

        #region Basic integer operations
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Subtract(int x, int y)
        {
            return x - y;
        }

        public int Multiply(int x, int y)
        {
            return x * y;
        }

        public int Divide(int x, int y)
        {
            return x / y;
        }
        #endregion
    }
}
