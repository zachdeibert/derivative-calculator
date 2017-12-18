using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    static class Extensions {
        public static decimal ToDecimal(this IEnumerable<char> chars) {
            return decimal.Parse(new string(chars.ToArray()));
        }

        public static IEnumerable<char> FindNumber(this IEnumerable<char> chars, out int length) {
            int skipLen = chars.TakeWhile(c => c == ' ').Count();
            IEnumerable<char> res = chars.Skip(skipLen).TakeWhile(c => (c >= '0' && c <= '9') || c == '.' || c == '-');
            length = skipLen + res.Count();
            return res;
        }
    }
}
