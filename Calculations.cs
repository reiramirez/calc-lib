using calc_lib.Models;
using System;
using System.Collections.Generic;

namespace calc_lib
{
    public static class Calculations
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
        public static int Add(int x, int y)
        {
            return x + y;
        }

        public static int Subtract(int x, int y)
        {
            return x - y;
        }

        public static int Multiply(int x, int y)
        {
            return x * y;
        }

        public static int Divide(int x, int y)
        {
            return x / y;
        }
        #endregion
        #region Basic fraction operations
        public static Fraction SimplifyFraction(Fraction fraction)
        {
            // To simplify a fraction, divide the top and bottom by the highest number that can divide into both numbers exactly.
            // Before anything else, check if either numerator or denominator is 1. If so, it's as simple as it can be.
            // This is a guard statement, a type of defensive programming style.
            if (fraction.numerator == 1 || fraction.denominator == 1)
                return fraction;

            // Start with the denominator. Check if it's a multiple of the numerator.
            if (fraction.denominator % fraction.numerator == 0)
            {
                // If so, divide both numbers by the numerator.
                fraction.denominator /= fraction.numerator;
                fraction.numerator = 1;
                return fraction;
            }

            // If not, perform the same check on the numerator.
            if (fraction.numerator % fraction.denominator == 0)
            {
                fraction.numerator /= fraction.denominator;
                fraction.denominator = 1;
                return fraction;
            }

            // If still not, find the factors of the numerator,
            var factors_of_numerator = PrimeFactorization(fraction.numerator);

            // and the denominator,
            var factors_of_denominator = PrimeFactorization(fraction.denominator);

            // then sort both lists in descending order.
            factors_of_numerator.Sort((x, y) => y.CompareTo(x));
            factors_of_denominator.Sort((x, y) => y.CompareTo(x));

            // then check each factor of the numerator to see if the factors of the denominator contain it.
            // By sorting both lists in descending order, we make sure that the greatest common factor (GCF) is found first.
            foreach (var numerator_factor in factors_of_numerator)
            {
                foreach (var denominator_factor in factors_of_denominator)
                {
                    if (numerator_factor == denominator_factor)
                    {
                        fraction.numerator /= numerator_factor;
                        fraction.denominator /= denominator_factor;

                        // Perform the whole method recursively to further simplify the fraction.
                        return SimplifyFraction(fraction);
                    }
                }
            }

            // Finally, if still not, then it's as simple as it can be.
            return fraction;
        }
        public static Fraction AddFractions(Fraction f1, Fraction f2)
        {
            // To add fractions, we first make both fractions have the same denominator.
            // This is easily achieved by multiplying both fractions' numerators by each other's denominators.
            if (f1.denominator != f2.denominator)
            {
                f1.numerator *= f2.denominator;
                f2.numerator *= f1.denominator;
            }

            // Then, we create a new fraction with the added numerators as the new numerator,
            // and the product of both denominators as the new denominator.
            var result = new Fraction(f1.numerator + f2.numerator, f1.denominator * f2.denominator);

            // Then we simplify the fraction and return it as a result.
            return SimplifyFraction(result);
        }
        public static Fraction SubtractFractions(Fraction f1, Fraction f2)
        {
            // To subtract fractions, we first make both fractions have the same denominator.
            // This is easily achieved by multiplying both fractions' numerators by each other's denominators.
            if (f1.denominator != f2.denominator)
            {
                f1.numerator *= f2.denominator;
                f2.numerator *= f1.denominator;
            }

            // Then, we create a new fraction with the subtracted numerators as the new numerator,
            // and the product of both denominators as the new denominator.
            var result = new Fraction(f1.numerator - f2.numerator, f1.denominator * f2.denominator);

            // Then we simplify the fraction and return it as a result.
            return SimplifyFraction(result);
        }
        public static Fraction MultiplyFractions(Fraction f1, Fraction f2)
        {
            // To multiply fractions, we first create a new fraction with the product of both numerators as the new numerator,
            var result = new Fraction(f1.numerator * f2.numerator, f1.denominator * f2.denominator);

            // Then we simplify the fraction and return it as a result.
            return SimplifyFraction(result);
        }
        public static Fraction DivideFractions(Fraction f1, Fraction f2)
        {
            // To divide fractions, we first flip the terms of the second fraction.
            f2.numerator += f2.denominator;                 // Add x and y together into "new x"
            f2.denominator = f2.numerator - f2.denominator; // Now that "new x" = x + y, remove y from "new x" to get x
            f2.numerator -= f2.denominator;                 // Finally, now that y = x, remove x from "new x" to get y

            // Then we simplify reuse our existing MultiplyFractions function to multiply the two fractions.
            return MultiplyFractions(f1, f2);
        }
        #endregion

        #region NO LOOKING UNDER HERE
        public static List<int> PrimeFactorization(int number)
        {
            var n = number;
            var factors = new List<int>();

            // If the number is 0, there are no factors. Return an empty list.
            if (n == 0)
                return factors;

            // Check if 2 is a factor of n. If so, divide n by 2 and add 2 as a factor. Repeat until 2 is no longer a factor of n.
            while (n % 2 == 0)
            {
                factors.Add(2);
                n /= 2;
            }

            // n is now definitely odd. For each value starting from 3 to the square root of n, perform the same check.
            for (var next = 3; next <= Math.Sqrt(n); next++)
            {
                while (n % next == 0)
                {
                    factors.Add(next);
                    n /= next;
                }
            }

            if (n > 2)
                factors.Add(n);

            return factors;
        }
        #endregion
    }
}
