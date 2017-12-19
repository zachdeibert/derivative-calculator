using System;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    class DifferenceQuotient {
        const long PrecisionLow = 0;
        const long PrecisionMed = 0;
        const long PrecisionHigh = 256;
        const int ScalingFactor = 10;
        const int MinFullMantissa = int.MaxValue / ScalingFactor;
        const long MantissaIntOverflow = 0x7FFFFFFF00000000;
        const long MantissaIntFlow = 0x00000000FFFFFFFF;
        const int ExponentMask = 0x00FF0000;
        const int ExponentShift = 16;
        const int MaxExponent = 28;
        const long SignMask = 0x80000000;
        Expression Expression;

        static void NormalizeMantissa(ref long low, ref long mid, ref long high) {
            mid += (low & MantissaIntOverflow) >> 32;
            low &= MantissaIntFlow;
            high += (mid & MantissaIntOverflow) >> 32;
            mid &= MantissaIntFlow;
            // Discard the data that overflowed the high dword
            high &= MantissaIntFlow;
        }

        static void FillMantissa(ref decimal point, ref long low, ref long mid, ref long high, bool neg, ref int exp) {
            if (low == 0 && mid == 0 && high == 0) {
                exp = MaxExponent;
            } else {
                while (high < MinFullMantissa && exp < MaxExponent) {
                    low *= 10;
                    mid *= 10;
                    high *= 10;
                    ++exp;
                    NormalizeMantissa(ref low, ref mid, ref high);
                }
                point = new decimal((int) low, (int) mid, (int) high, neg, (byte) exp);
            }
        }

        static void CalculateDelta(decimal point, ref long low, ref long mid, ref long high, ref int exp) {
            low = PrecisionLow;
            mid = PrecisionMed;
            high = PrecisionHigh;
            NormalizeMantissa(ref low, ref mid, ref high);
        }

        static void DecodeDecimal(decimal val, out long low, out long mid, out long high, out int exp, out bool neg) {
            int[] bits = decimal.GetBits(val);
            low = bits[0];
            mid = bits[1];
            high = bits[2];
            exp = (bits[3] & ExponentMask) >> ExponentShift;
            neg = (bits[3] & SignMask) != 0;
        }

        static decimal GetDelta(ref decimal point) {
            long low;
            long mid;
            long high;
            int exp;
            bool neg;
            DecodeDecimal(point, out low, out mid, out high, out exp, out neg);
            int origExp = exp;
            FillMantissa(ref point, ref low, ref mid, ref high, neg, ref exp);
            CalculateDelta(point, ref low, ref mid, ref high, ref exp);
            return new decimal((int) low, (int) mid, (int) high, neg, (byte) exp);
        }

        public decimal SolveAtPoint(decimal x) {
            decimal h = GetDelta(ref x);
            return (Expression.Solve(x + h) - Expression.Solve(x)) / h;
        }

        public DifferenceQuotient(Expression expression) {
            Expression = expression;
        }
    }
}