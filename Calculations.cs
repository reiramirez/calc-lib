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
         * - Basic fraction operations (done)
         * - Decimal rounding (done)
         * - Mean, median, and mode (done)
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
            var factorsOfNumerator = PrimeFactorization(fraction.numerator);

            // and the denominator,
            var factorsOfDenominator = PrimeFactorization(fraction.denominator);

            // then sort both lists in descending order.
            factorsOfNumerator.Sort((x, y) => y.CompareTo(x));
            factorsOfDenominator.Sort((x, y) => y.CompareTo(x));

            // then check each factor of the numerator to see if the factors of the denominator contain it.
            // By sorting both lists in descending order, we make sure that the greatest common factor (GCF) is found first.
            foreach (var numeratorFactor in factorsOfNumerator)
            {
                foreach (var denominatorFactor in factorsOfDenominator)
                {
                    if (numeratorFactor == denominatorFactor)
                    {
                        fraction.numerator /= numeratorFactor;
                        fraction.denominator /= denominatorFactor;

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
        #region Decimal rounding
        public static DecimalNumber RoundDecimal(DecimalNumber decimalNumber)
        {
            // To round decimals, we simply check if the number to the right of the decimal point starts with a 5 or higher.
            // To do that, we first divide that number by 10 until we get a value less than 10.
            // That is the first digit to the right of the decimal point.
            while (decimalNumber.Right >= 10)
            {
                decimalNumber.Right /= 10;
            }

            // If the number to the right of the decimal point starts with a 5 or higher, return the digit left of the decimal point plus 1.
            // If not, simply return the digit left of the decimal point.
            // The following statement uses an inline if statement, also called the "ternary conditional operator" or simply "?:".
            return new DecimalNumber()
            {
                // The syntax of the "?:" operator is as follows: <condition> ? <value if true> : <value if false>
                Left = decimalNumber.Right >= 5 ? decimalNumber.Left + 1 : decimalNumber.Left,
                Right = 0
            };
        }
        #endregion
        #region Mean, median, and mode
        // The "params" keyword seen below indicates that this method takes a variable number of arguments.
        // To be able to use the params keyword, the parameter following it must be a one-dimensional array of any type.
        // i.e. "params int[] numbers" means that the "numbers" variable is a one-dimensional array of ints.
        // You can invoke functions with "params" with any number of arguments (even none!).
        // You can also add more arguments, but the "params" argument must always be last.
        public static decimal Mean(params int[] numbers)
        {
            // To get the mean (or average) of any set of numeric values, first we add them all up.
            var sum = 0;
            foreach (var number in numbers)
                sum += number;

            // To maintain accuracy, we convert the sum from an int to a decimal.
            var decimalSum = (decimal)sum;

            // Then we divide the sum by the number of values in the set and return the result.
            return decimalSum / numbers.Length;
        }
        public static decimal Median(params int[] numbers)
        {
            // To get the median of any set of numeric values, first we sort the numbers in ascending order.
            // To do that, we first have to convert it into a list.
            var numberList = new List<int>(numbers);

            // Then we sort the list. By default, List.Sort() sorts in ascending order.
            numberList.Sort();

            // Then, we check the length of the list.
            // If the length is odd, we simply return the middle value.
            if (numberList.Count % 2 == 1)
                // Since we are dividing two ints, the "smaller half" of the quotient is returned,
                // i.e. given two ints 5 and 2, 5 / 2 would return 2.
                return numberList[numberList.Count / 2];

            // if the length is even, we return the mean of the two middle terms.
            else
            {
                // In programming, array (and by extension, list) indexes always start from 0.
                // That means that by dividing the list's length by two, we are getting the right-most index.
                // i.e. given the following array with indexes indicated: [0] [1] [2] [3] with length 4
                // If we divide 4 by 2, we get 2.   That corresponds to this index ^
                var rightMiddleIndex = numberList.Count / 2;

                // To get the left middle index, we simply subtract 1 from the right middle index.
                var leftMiddleIndex = rightMiddleIndex - 1;

                return Mean(numberList[leftMiddleIndex], numberList[rightMiddleIndex]);
            }
        }
        // This method returns an "int?". The "?" operator, also called the optional operator, means that the value may be nullable.
        // The optional operator is only used for values that cannot be null by default.
        // This allows us to return a null value for a previously-not-nullable type, like int.
        public static int? Mode(params int[] numbers)
        {
            // To get the mode of any set of numeric values, we simply count the amount of times each number appears in the set.
            // We can do this using a Dictionary storing the each number and the amount of times it appeared in the set.
            var numberDictionary = new Dictionary<int, int>();

            // We loop through each number and...
            foreach (var number in numbers)
            {
                // If it doesn't exist in our dictionary, we add it and assign it a value of 1.
                if (!numberDictionary.ContainsKey(number))
                    numberDictionary.Add(number, 1);

                // If it already exists, we simply add 1 to its value.
                else
                    numberDictionary[number]++;
            }

            // Then, we turn the dictionary into a list of entries. We can simply use the constructor because Dictionary implements IEnumerable.
            var numberEntryList = new List<KeyValuePair<int, int>>(numberDictionary);

            // We then sort the entries by value in descending order.
            numberEntryList.Sort((keyValuePair1, keyValuePair2) => keyValuePair2.Value.CompareTo(keyValuePair1.Value));

            // Finally, we get the first entry in the now-sorted list.
            // If the entry's value is 1, we can assume that all numbers appeared once
            // and we return null because there is no mode.
            if (numberEntryList[0].Value == 1)
                return null;

            // Otherwise, we return the key of the first entry.
            else
                return numberEntryList[0].Key;
        }
        #endregion

        /*
         * 2. Complex maths (if math is so good, why didn't they make a math 2?):
         * 
         * - Prime factorization (done)
         * - Babylonian method for getting square roots
         * - Quake3D's fast inverse square root (just as an example)
         */
        #region Prime factorization
        public static List<int> PrimeFactorization(int number)
        {
            // To get all the prime factors of a number, we need to get creative.
            // If the number is 0 or 1, there are no prime factors. Return an empty list.
            if (number == 0 || number == 1)
                return new List<int>();

            var factors = new List<int>();

            // First, we check if 2 is a factor of the number. If so, divide the number by 2 and add 2 as a factor. Repeat until 2 is no longer a factor of the number.
            number = ExtractFactors(factors, 2, number);

            // The number is now definitely odd. For each odd number starting from 3 to the square root of the number, perform the same check.
            // This algorithm takes advantage of two things:
            //      1. Since we extracted all possible 2s from the number, the number will always be odd. Hence, we only need to check for odd factors.
            //      2. Since we are going through numbers in ascending order, and the number is always decreasing, we will come to a point where the
            //         largest possible factor to extract would be two instances of the same factor, i.e. that factor squared. An example of this is
            //         the number 289, where the 2 possible prime factors are only 17 and 17, and 17 is the square root of 289.
            // We can further shorten the code by starting the for-loop at 2, but we will be doing an "== 2" check for each number after 2, which will
            // add processing time. That's why we extracted all possible 2s first.
            for (var next = 3; next <= SquareRoot(number); next++)
            {
                if (next % 2 == 0)
                    continue;

                number = ExtractFactors(factors, next, number);
            }

            // Finally, if the number is not equal to 1, it must be the last number.
            // i.e. given the number 4, which has the prime factors of 2, and 2:
            // 4 / 2 = 2, even
            // 2 / 2 = 1, odd
            // Since we are left with 1, we do not include it in the factors.
            //
            // i.e. given the number 108, which has the prime factors of 2, 2, 3, 3, and 3:
            // 108 / 2 = 54, even
            // 54 / 2 = 27, odd
            // 27 / 3 = 9, odd
            // 9 / 3 = 3, odd
            // Since the square root of 3 = 1.7320508..., the for-loop breaks and we are left with 3.
            // 
            // i.e. given the number 396, which has the prime factors of 2, 2, 3, 3, and 11:
            // 396 / 2 = 198, even
            // 198 / 2 = 99, odd
            // 99 / 3 = 33, odd
            // 33 / 3 = 11, odd
            // Since, the square root of 11 is 3.3166247..., the for-loop breaks and we are left with 11.
            if (number != 1)
                factors.Add(number);

            return factors;
        }
        // This is an extraction of reusable code. All it does is it extracts a given factor from the number until it's not possible anymore.
        private static int ExtractFactors(List<int> factors, int factor, int number)
        {
            while (number % factor == 0)
            {
                factors.Add(factor);
                number /= factor;
            }

            return number;
        }

        #endregion

        #region NO LOOKING UNDER HERE
        public static double SquareRoot(int num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            int n = (num / 2) + 1;       // Initial estimate, never low  
            int n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } 
            return n;
        }
        #endregion
    }
}
