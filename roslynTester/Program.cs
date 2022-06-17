using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace roslynTester
{
    
    class VariableLocation
    {
        public int location { get; set; }
        public string variable { get; set; }
    }
    class Program
    {
        
        
        static async Task Main(string[] args)
        {

            await RoslynAnalyzer.generateRoslynAnalyzer();
            //HelperTest.testRandomMethod();
            //RoslynAnalyzer.testRandomMethod();
            /*
            string functionString = @"

                int Foo(int x){
                    return 2 * x;
                }
                int x = 2;
            ";

            string expression = "Foo(x)";

            string value = await Evaluate.evaluateFunction(functionString, expression);
            Console.WriteLine(value);
            */

        }

        

        

        

    }
}

//TODO LIST:
/**
 * Handle all data types in analyzer
 */