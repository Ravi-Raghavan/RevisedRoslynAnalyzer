using System;
namespace roslynTester
{
	public static class CodeString
	{


        //Very basic example
        public static readonly string testOne = @"
        public class testClass{
            static int Foo(int x){
                return x + 1;
            }
            static void Main(string [] args){
                var x = 1 + 2;
                var y = x * 2;
                var z = Foo(x) + y;
                y = z + x;
                y = x + x;
            }

        }
        ";

        //Very basic example
        public static readonly string testTwo = @"
        public class testClass{
        static int Foo(int x, int y){
                    return 2 * x + y;
                }

        static void Main(string [] args)
        {
            var x = 1;
            var y = x * 2;
            var z = Foo(x, 1) + y;
            var w = z + y * 2 - 3 * x + (y /x);
            y = z + x;
            y = x + x;
            x = y * 2 - 3 + w * y;
            double d = 5.4;
            d = 4 + 3.4;
        }
         }   
        ";

        //Basic example
        public static readonly string testThree = @"
        public class testClass{
            static void Main(string [] args){
                var x = 1;
                var y = 2;
                var z = x + y;
                var w = x + y + x;
            }
        }
        ";

        //Testing double datatype as well as functions
        public static readonly string testFour = @"
        public class testClass{
        static int Foo(int x, int y, int z){
                    return 2 * x + y;
                }

        static void Main(string [] args)
        {
            var x = 1;
            var y = x * 2;
            var z = Foo(x, y, x) + y;
            var w = z + y * 2 - 3 * x + (y /x);
            y = z + x;
            y = x + x;
            x = y * 2 - 3 + w * y;
            double d = 5.4;
            d = 4 + 3.4;
        }
         }   
        ";

        //Testing reassignments
        public static readonly string testFive = @"
        public class testClass{
        static int Foo(int x){
            return x * 2;
        }
        static void Main(string [] args)
        {
            var x = 1;
            var y = 2;
            var z = 3;
            var w = 4;
            y = 3 * w - 2 * z + 4 * x;
            x = 2 * y + 2 * x - 2 * w;
        }
}";

        //Testing unary expressions
        public static readonly string testSix = @"
       public class testClass{
            static void Main()
            {
                var a = 2;
                var b = 1;
                var c = a % b;
                var d = ++a;
                var e = a--;
            }
        }
        ";

        //Testing non-arithmetic expressions;
        public static readonly string testSeven = @"

        public class testClass{

            public static void Main(string [] args){
                var a = 2;
                var b = ~ a;
                var c = b >> 2;
                var d = c & b;
            }
        }

        ";

        // Testing the case where variable reassignments are
        //done using the same variable
        public static readonly string testEight = @"
        public class testClass{
            public static void Main(string [] args){
                var a = 2;
                var b = 3;
                a = a + b; 
            }
        }
        ";

        //Variable starting out as "arithmetic" and then
        //being reassigned using non-arithmetic values
        public static readonly string testNine = @"
        public class testClass{
            public static int Foo(int x){
                return 2 * x;
            }

            public static void Main(string [] args){
                var a = Foo(2);
                var b = 10;
                b = a + b;
            }
        }
        ";


        //Testing strictly doubles
        public static readonly string testTen = @"
        public class testClass{
            public static double Foo(double x){
                return 1.5 + x;
            }

            public static void Main(string [] args){
                double a = 2.3;
                double b = 2.45;
                double c = Foo(a);
                b = 2 * a;
            }
        }
        ";

    }
}

