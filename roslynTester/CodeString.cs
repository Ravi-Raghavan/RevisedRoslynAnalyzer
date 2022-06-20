using System;
namespace roslynTester
{
	public static class CodeString
	{

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

        public static readonly string test2 = @"
        public class testClass{
        void Foo(int x){
            return x * 2;
        }
        void Method()
        {
            var x = 1;
            var y = Foo(x) + 10;
            var z = Foo(y);
        }
}";

        public static readonly string simpleMethodStuff = @"

        void Method()
        {
            var a = 2;
            var b = 1;
            var c = b + 2 % a + 2 - 3 / 4 * 2;
        }
        ";


    }
}

