using System;

namespace Com.GitHub.ZachDeibert.DerivativeCalculator {
    static class Entry {
        static void ResetTerm(object sender, ConsoleCancelEventArgs args) {
            Console.Write("\r   {0}[2T\r", (char) 27);
        }

        static void Main(string[] args) {
            Console.CancelKeyPress += ResetTerm;
            while (true) {
                Console.Write("y = ");
                Expression expr = new Expression(Console.ReadLine());

                Console.WriteLine("y_0 = {0}", expr.Solve(0));
                Console.WriteLine("y_1 = {0}", expr.Solve(1));
                Console.WriteLine("y_2 = {0}", expr.Solve(2));

                Console.WriteLine();
            }
        }
    }
}
