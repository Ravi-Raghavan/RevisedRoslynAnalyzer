using System;
namespace roslynTester
{
	public static class CodeString
	{

        public static readonly string test = @"
public class testClass{
static int Foo(int x, int y){
            return 2 * x + y;
        }

static void Main(string [] args)
        {
            var x = 1 + 2;
            var y = x * 2;
            var z = Foo(x, 1) + y;
            var w = z + y * 2 - 3 * x + (y /x);
            y = z + x;
            y = x + x;
            x = y * 2 - 3 + w * y;

            double d = 5.4;
        }
         }   
        ";

        public static readonly string testBackup = @"
        public class testClass{
        
        static int Foo(int x, int y){
            return 2 * x + y;
        }
        static void Main(string [] args)
        {
            int param = 4;
            int paramTwo = 10;
            var random = Foo(param, paramTwo) + 12;
            var x = 1;
            var y = 3;
            var z = x + y + x;
            var w = z + y + x;
            w = w * 2 + x + 4 - y;
            z = w + 2 + x;
            var v = w;
            y = 2;
            var c = (10 * y) + z - x;
            x = 4;
            y = 2;
            z = 5;
            //var p = x * (x + y / (4 * 2 * x)) + y / 4 + z - 5;
            z = Foo(y, z);
            var ravi = z + y;

            
            
        }
}";

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

