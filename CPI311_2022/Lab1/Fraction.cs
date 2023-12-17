using System;
namespace CPI311.Labs
{
    public class Fraction
    {
        public int numerator;
        public int denominator;

        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; Simplify(); }
        }

        public int Denominator
        {
            get { return denominator; }
            set { denominator = value; Simplify(); }
        }

        //Constructor
        public Fraction(int n = 0, int d = 1)
        {
            numerator = n;
            if (d == 0) d = 1; // avoid to divide by 0
            denominator = d;
            Simplify();
        }

        public static Fraction multiply(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator - b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public override String ToString()
        {
            if(denominator == 1)
            {
                return numerator + "";
            }
            return numerator + "/" + denominator;
        }

        private void Simplify()
        {
            if (denominator < 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            int gcd = GCD(Math.Max(numerator, denominator), Math.Min(numerator, denominator));
            numerator /= gcd;
            denominator /= gcd;
        }

        public static int GCD(int bigger, int smaller)
        {
            if (smaller == 0) return bigger;
            return GCD(smaller, bigger % smaller);
        }
    }
}

