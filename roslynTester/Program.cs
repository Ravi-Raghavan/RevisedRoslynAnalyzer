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
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace roslynTester
{
    class KeyObj {

        private int x;
        private int y;
        public KeyObj(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj is KeyObj))
            {
                return false;
            }
            KeyObj k = (KeyObj)(obj);
            return k.x == this.x && k.y == this.y;
        }

    }
    public class Program
    {
        public static async Task Main(string [] args)
        {
            RoslynAnalyzer.generateRoslynAnalyzer();

            //Dictionary<KeyObj, int> dict = new Dictionary<KeyObj, int>();
            //KeyObj kbj = new KeyObj(1, 2);
            //dict.Add(kbj, 10);

            //KeyObj kbj2 = new KeyObj(1, 2);
            //Console.WriteLine(dict.ContainsKey(kbj2));
        }
    }
}

//TODO LIST:
/**
 * Handle all data types in analyzer
 */