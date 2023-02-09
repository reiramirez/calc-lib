using System;
using System.Collections.Generic;
using System.Text;

namespace calc_lib.Models
{
    public struct Fraction
    {
        public int numerator, denominator;

        // Simple constructor
        public Fraction(int numerator, int denominator) : this()
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        // For output purposes
        public override string ToString()
        {
            return numerator + "/" + denominator;
        }
    }
}
