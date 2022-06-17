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
	public class HelperTest
	{
        public static void isArithmetic(ExpressionSyntax syntaxNode, CSharpCompilation compilation)
        {
            SyntaxTree AST = syntaxNode.SyntaxTree;
            SemanticModel semanticModel = compilation.GetSemanticModel(AST);

            SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(syntaxNode);
            IEnumerable<SyntaxNode> descendants = syntaxNode.DescendantNodes();
            foreach(SyntaxNode node in descendants)
            {
                Console.WriteLine("----------------");
                Console.WriteLine(node.Kind());
                Console.WriteLine(node);
                Console.WriteLine(node.GetType());
                Console.WriteLine("----------------");
            }
        }
        public static void testScriptWithFunctions()
        {
            SyntaxTree AST = CSharpSyntaxTree.ParseText(CodeString.test2);
            CompilationUnitSyntax root = AST.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("HelloWorld")
                .AddReferences(MetadataReference.CreateFromFile(
                typeof(string).Assembly.Location))
                .AddSyntaxTrees(AST);
            SyntaxNode treeRoot = AST.GetRoot();

            SemanticModel semanticModel = compilation.GetSemanticModel(AST);

            IEnumerable<SyntaxNode> descendentNodes = root.DescendantNodes();

            foreach (SyntaxNode syntaxNode in descendentNodes)
            {
                if (syntaxNode is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax mds = (MethodDeclarationSyntax)(syntaxNode);
                    Console.WriteLine($"Name of Method: {mds.Identifier}");
                    Console.WriteLine($"Method Body: {mds.Body}");
                    //SymbolInfo symbolInfo;
                    //Console.WriteLine($"Argument List: {mds}");

                    IEnumerable<ParameterListSyntax> parameters = mds.DescendantNodes().OfType<ParameterListSyntax>();
                    foreach (ParameterListSyntax parameter in parameters)
                    {
                        IEnumerable<ParameterSyntax> parameters2 = parameter.DescendantNodes().OfType<ParameterSyntax>();
                        foreach (ParameterSyntax param in parameters2)
                        {
                            Console.WriteLine(param.Identifier.ToString());
                        }
                    }

                }
                else
                {
                    //Console.WriteLine(syntaxNode.Kind());
                }

            }

        }

        public static void testRandomMethod()
        {
            SyntaxTree AST = CSharpSyntaxTree.ParseText(CodeString.simpleMethodStuff);
            CompilationUnitSyntax root = AST.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("HelloWorld")
                .AddReferences(MetadataReference.CreateFromFile(
                typeof(string).Assembly.Location))
                .AddSyntaxTrees(AST);
            SyntaxNode treeRoot = AST.GetRoot();

            SemanticModel semanticModel = compilation.GetSemanticModel(AST);

            IEnumerable<SyntaxNode> descendentNodes = root.DescendantNodes();
            foreach (SyntaxNode syntaxNode in descendentNodes)
            {
                if(syntaxNode is ExpressionSyntax)
                {
                    isArithmetic((ExpressionSyntax)syntaxNode, compilation);
                }
                /*
                if (syntaxNode is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax mds = (MethodDeclarationSyntax)(syntaxNode);
                    Console.WriteLine($"Name of Method: {mds.Identifier}");
                    Console.WriteLine($"Method Body: {mds.Body}");
                    //SymbolInfo symbolInfo;
                    //Console.WriteLine($"Argument List: {mds}");

                    IEnumerable<ParameterListSyntax> parameters = mds.DescendantNodes().OfType<ParameterListSyntax>();
                    foreach (ParameterListSyntax parameter in parameters)
                    {
                        IEnumerable<ParameterSyntax> parameters2 = parameter.DescendantNodes().OfType<ParameterSyntax>();
                        foreach (ParameterSyntax param in parameters2)
                        {
                            Console.WriteLine(param.Identifier.ToString());
                        }
                    }

                }
                else if (syntaxNode is EqualsValueClauseSyntax)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(syntaxNode.Kind());
                    Console.WriteLine(syntaxNode);
                    Console.WriteLine(((EqualsValueClauseSyntax)(syntaxNode)).Value);
                    Console.WriteLine("----------------");
                }
                else if (syntaxNode is VariableDeclarationSyntax)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(syntaxNode.Kind());
                    Console.WriteLine(syntaxNode);
                    Console.WriteLine("----------------");
                }
                else if (syntaxNode is IdentifierNameSyntax)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(syntaxNode.Kind());
                    Console.WriteLine(syntaxNode);
                    Console.WriteLine("----------------");
                }
                else if (syntaxNode is VariableDeclaratorSyntax)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(syntaxNode.Kind());
                    Console.WriteLine(syntaxNode);
                    Console.WriteLine("----------------");
                }
                else if (syntaxNode is InvocationExpressionSyntax)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(syntaxNode.Kind());
                    Console.WriteLine(syntaxNode);
                    Console.WriteLine("----------------");
                }
                else
                {
                    Console.WriteLine(syntaxNode.Kind());
                }
                */
            }

        }


    }
}

