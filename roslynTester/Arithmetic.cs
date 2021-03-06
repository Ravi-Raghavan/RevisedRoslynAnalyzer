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
	public class Arithmetic
	{
        public static bool isArithmetic(Dictionary<string, Dictionary<int, List<Diagnostic>>> diagnostics, ExpressionSyntax syntaxNode)
        {
            IEnumerable<IdentifierNameSyntax> identifiers = syntaxNode.DescendantNodes().OfType<IdentifierNameSyntax>();
            IEnumerable<BinaryExpressionSyntax> operators = syntaxNode.DescendantNodes().OfType<BinaryExpressionSyntax>();
            IEnumerable<PostfixUnaryExpressionSyntax> unaryPostfix = syntaxNode.DescendantNodes().OfType<PostfixUnaryExpressionSyntax>();
            IEnumerable<PrefixUnaryExpressionSyntax> unaryPrefix = syntaxNode.DescendantNodes().OfType<PrefixUnaryExpressionSyntax>();

            if (syntaxNode is BinaryExpressionSyntax)
            {
                List<BinaryExpressionSyntax> binaryExpressions = new List<BinaryExpressionSyntax>();
                binaryExpressions.Add((BinaryExpressionSyntax)(syntaxNode));
                operators = binaryExpressions.AsEnumerable();
            }
            if(syntaxNode is PostfixUnaryExpressionSyntax || unaryPostfix.Count() > 0)
            {
                return false;
            }
            if(syntaxNode is PrefixUnaryExpressionSyntax || unaryPrefix.Count() > 0)
            {
                return false;
            }
            //Checking Variables
            foreach (IdentifierNameSyntax identifier in identifiers)
            {
                if (!diagnostics.ContainsKey(identifier.ToString()))
                {
                    return false;
                }
            }

            //Check Operators
            foreach (BinaryExpressionSyntax binaryExpression in operators)
            {
                if ((binaryExpression.Kind() == SyntaxKind.AddExpression) || (binaryExpression.Kind() == SyntaxKind.SubtractExpression)
                    || (binaryExpression.Kind() == SyntaxKind.MultiplyExpression) || (binaryExpression.Kind() == SyntaxKind.DivideExpression))
                {

                    continue;
                }
                return false;
            }

            return true;
        }
    }
}

