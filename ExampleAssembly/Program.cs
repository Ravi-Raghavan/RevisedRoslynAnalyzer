using System;

namespace roslynTester.examples
{
    public class Program
    {
        public static int Foo(int x, int y = 1, int z = 1)
        {
            return 2 * x;
        }
        public static void Main(string [] args)
        {
            /*
            var x = 1 + 2;
            var y = x * 2;
            var z = Foo(x) + y;
            y = z + x;
            y = x + x;
            */


            //var x = 1;
            //var y = 2;
            //var z = x + y;
            //var w = x + y + x;

            //var x = 1;
            //var y = x * 2;
            //var z = Foo(x, y, x) + y;
            //var w = z + y * 2 - 3 * x + (y / x);
            //y = z + x;
            //y = x + x;
            //x = y * 2 - 3 + w * y;
            //double d = 5.4;
            //d = 4 + 3.4;

            //var x = 1;
            //var y = 2;
            //var z = 3;
            //var w = 4;
            //y = 3 * w - 2 * z + 4 * x;
            //x = 2 * y + 2 * x - 2 * w;

            //var a = 2;
            //var b = 1;
            //var c = a % b;
            //var d = ++a;
            //var e = a--;

        }
    }
}