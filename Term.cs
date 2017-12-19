using System;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    class Term {
        public decimal Coefficient;
        public decimal Exponent;

        public override string ToString() {
            if (Coefficient == 0) {
                return "";
            } else if (Exponent == 0) {
                return Coefficient.ToString();
            } else if (Exponent == 1) {
                if (Coefficient == 1) {
                    return "x";
                } else {
                    return string.Format("{0}x", Coefficient);
                }
            } else if (Coefficient == 1) {
                return string.Format("x^{0}", Exponent);
            } else {
                return string.Format("{0}x^{1}", Coefficient, Exponent);
            }
        }
    }
}
