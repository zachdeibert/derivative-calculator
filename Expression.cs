using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    class Expression {
        public readonly string Template;

        public override string ToString() {
            return Template;
        }

        static bool FindOp(string expr, char a, char b, out int i, out bool first) {
            int ia = expr.IndexOf(a);
            int ib = expr.IndexOf(b);
            if (ia < 0) {
                first = false;
                if (ib < 0) {
                    i = -1;
                    return false;
                } else {
                    i = ib;
                    return true;
                }
            } else {
                if (ib < 0) {
                    i = ia;
                    first = true;
                    return true;
                } else {
                    i = Math.Min(ia, ib);
                    first = ia == i;
                    return true;
                }
            }
        }

        static void SimpleOp(ref string expr, char a, char b, Func<decimal, decimal, decimal> aop, Func<decimal, decimal, decimal> bop) {
            int i;
            bool first;
            while (FindOp(expr, a, b, out i, out first)) {
                int before;
                int after;
                decimal lhs = expr.Reverse().Skip(expr.Length - i).FindNumber(out before).Reverse().ToDecimal();
                decimal rhs = expr.Skip(i + 1).FindNumber(out after).ToDecimal();
                decimal res = first ? aop(lhs, rhs) : bop(lhs, rhs);
                expr = string.Concat(expr.Substring(0, i - before), res.ToString(), expr.Substring(i + 1 + after));
            }
        }

        static decimal Solve(string expr) {
            Match match = Regex.Match(expr, "\\(([^)]+)\\)");
            int sub = 0;
            while (match.Success) {
                string res = Solve(match.Groups[1].Value).ToString();
                expr = string.Concat(expr.Substring(0, match.Index - sub), res, expr.Substring(match.Index + match.Length - sub));
                sub += match.Length - res.Length;
                match = match.NextMatch();
            }
            while (true) {
                int i = expr.IndexOf('^');
                if (i < 0) {
                    break;
                } else {
                    int before;
                    int after;
                    decimal lhs = expr.Reverse().Skip(expr.Length - i).FindNumber(out before).Reverse().ToDecimal();
                    decimal rhs = expr.Skip(i + 1).FindNumber(out after).ToDecimal();
                    decimal res = (decimal) Math.Pow((double) lhs, (double) rhs); // TODO Increase accuracy!
                    expr = string.Concat(expr.Substring(0, i - before), res.ToString(), expr.Substring(i + 1 + after));
                }
            }
            SimpleOp(ref expr, '*', '/', (a, b) => a * b, (a, b) => a / b);
            SimpleOp(ref expr, '+', '_', (a, b) => a + b, (a, b) => a - b);
            return decimal.Parse(expr);
        }

        public decimal Solve(decimal x) {
            return Solve(Template.Replace("x", string.Format(" {0} ", x)).Replace('-', '_'));
        }

        public Expression(string template) {
            Template = template;
        }
    }
}
