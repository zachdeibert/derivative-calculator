using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    class Derivative {
        const decimal Point = 0;
        const int Accuracy = 10;
        public readonly List<Term> Terms;

        public override string ToString() {
            string res = Terms.Select(t => t.ToString()).Aggregate((a, b) => string.Concat(a, " + ", b));
            string old;
            do {
                res = (old = res).Replace(" +  + ", " + ");
            } while (res != old);
            if (res.StartsWith(" + ")) {
                res = res.Substring(3);
            }
            if (res.EndsWith(" + ")) {
                res = res.Remove(res.Length - 3);
            }
            return res;
        }

        public Derivative(DifferenceQuotient differenceQuotient) {
            Terms = new List<Term>();
            decimal factor = 1;
            for (int i = 1; i < Accuracy; ++i) {
                Terms.Add(new Term {
                    Coefficient = differenceQuotient.SolveAtPointRecursive(Point, i) * factor,
                    Exponent = i - 1
                });
                factor /= i;
            }
        }
    }
}
