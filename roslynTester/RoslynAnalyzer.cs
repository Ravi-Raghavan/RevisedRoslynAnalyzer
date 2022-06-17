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
	public class RoslynAnalyzer
	{
        private static Dictionary<string, Dictionary<int, List<VariableLocation>>> variableDependencies = new Dictionary<string, Dictionary<int, List<VariableLocation>>>();
        private static Dictionary<string, List<int>> updates = new Dictionary<string, List<int>>();
        private static Dictionary<string, Dictionary<int, List<Diagnostic>>> diagnostics = new Dictionary<string, Dictionary<int, List<Diagnostic>>>();
        private static Dictionary<string, Dictionary<int, Value>> variableValues = new Dictionary<string, Dictionary<int, Value>>();
        private static Dictionary<string, string> functions = new Dictionary<string, string>();
        
        static bool isArithmetic(ExpressionSyntax syntaxNode)
        {
            IEnumerable<IdentifierNameSyntax> identifiers = syntaxNode.DescendantNodes().OfType<IdentifierNameSyntax>();
            IEnumerable<BinaryExpressionSyntax> operators = syntaxNode.DescendantNodes().OfType<BinaryExpressionSyntax>();
            //Checking Variables
            foreach(IdentifierNameSyntax identifier in identifiers)
            {
                if (!diagnostics.ContainsKey(identifier.ToString()))
                {
                    return false;
                }
            }

            //Check Operators
            foreach(BinaryExpressionSyntax binaryExpression in operators)
            {
                if ((binaryExpression.Kind() == SyntaxKind.AddExpression) || (binaryExpression.Kind() == SyntaxKind.SubtractExpression)
                    || (binaryExpression.Kind() == SyntaxKind.MultiplyExpression) || (binaryExpression.Kind() == SyntaxKind.DivideExpression)) {

                    continue;
                }
                return false;
            }

            return true;
        }

        static async Task processDeclaration(VariableDeclarationSyntax syntaxNode)
        {
            VariableDeclarationSyntax declaration = (VariableDeclarationSyntax)(syntaxNode);
            IEnumerable<InvocationExpressionSyntax> methodInvocations = declaration.DescendantNodes().OfType<InvocationExpressionSyntax>();
            bool empty = methodInvocations.Count() == 0;
            SeparatedSyntaxList<VariableDeclaratorSyntax> list = declaration.Variables;
            foreach (VariableDeclaratorSyntax item in list)
            {
                VariableDeclaratorSyntax variable = item;
                string variableName = variable.Identifier.ToString();
                int location = variable.GetLocation().GetLineSpan().EndLinePosition.Line;
                IEnumerable<SyntaxNode> nestedDescendantNodes = syntaxNode.DescendantNodes();
                IEnumerable<EqualsValueClauseSyntax> nestedAssignments = nestedDescendantNodes.OfType<EqualsValueClauseSyntax>();

                if (nestedAssignments.Count() == 0)
                {
                    updates.Add(variableName, new List<int>());
                    updates[variableName].Add(location);

                    diagnostics.Add(variableName, new Dictionary<int, List<Diagnostic>>());
                    diagnostics[variableName].Add(location, new List<Diagnostic>());
                    continue;
                }

                foreach (var nestedAssignment in nestedAssignments)
                {
                    diagnostics.Add(variableName, new Dictionary<int, List<Diagnostic>>());
                    diagnostics[variableName].Add(location, new List<Diagnostic>());
                    if (isArithmetic(nestedAssignment.Value)) { await AnalyzeExpressionAsync(variableName, nestedAssignment.Value, location); }
                    else { await AnalyzeExpressionNonArithmeticAsync(variableName, nestedAssignment.Value, location); }
                    updates.Add(variableName, new List<int>());
                    updates[variableName].Add(location);
                }
            }
        }

        static async Task processAssignment(AssignmentExpressionSyntax syntaxNode)
        {
            AssignmentExpressionSyntax variableAssignment = (AssignmentExpressionSyntax)(syntaxNode);
            int location = variableAssignment.GetLocation().GetLineSpan().EndLinePosition.Line;
            string variableName = variableAssignment.Left.ToString();

            IEnumerable<InvocationExpressionSyntax> methodInvocations = variableAssignment.DescendantNodes().OfType<InvocationExpressionSyntax>();

            if (isArithmetic(syntaxNode.Right))
            {
                await AnalyzeExpressionAsync(variableName, variableAssignment.Right, location);

            }
            else
            {
                await AnalyzeExpressionNonArithmeticAsync(variableName, variableAssignment.Right, location);
            }
            updates[variableName].Add(location);

        }
        static async Task AnalyzeExpressionNonArithmeticAsync(string variableName, ExpressionSyntax assignment, int location)
        {
            ExpressionSyntax RHS = assignment;
            string variable = variableName;
            if (!variableValues.ContainsKey(variable))
            {
                variableValues.Add(variable, new Dictionary<int, Value>());
            }
            if (!variableValues[variable].ContainsKey(location))
            {
                variableValues[variable].Add(location, new Value( new object(),  true));
            }
            IEnumerable<IdentifierNameSyntax> identifiers = RHS.DescendantNodes().OfType<IdentifierNameSyntax>();
            int sum = 0;
            string functionCode = "";
            bool bothSides = false;
            int RHS_value = -1;
            Dictionary<string, object> currentValues = new Dictionary<string, object>();
            foreach (var item in identifiers)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)(item);
                string rightVariable = identifier.ToString();
                if (functions.ContainsKey(rightVariable))
                {
                    functionCode += functions[rightVariable];
                    continue;
                }
                int rightLocation = updates[rightVariable][updates[rightVariable].Count - 1];

                if (!variableDependencies.ContainsKey(variable))
                {
                    variableDependencies.Add(variable, new Dictionary<int, List<VariableLocation>>());
                }
                if (!variableDependencies[variable].ContainsKey(location))
                {
                    variableDependencies[variable].Add(location, new List<VariableLocation>());
                }

                VariableLocation obj = new VariableLocation();
                obj.variable = rightVariable;
                obj.location = rightLocation;

                if (!currentValues.ContainsKey(rightVariable))
                {
                    currentValues.Add(rightVariable, (variableValues[rightVariable][rightLocation].value));
                }

                variableDependencies[variable][location].Add(obj);

                if (!diagnostics.ContainsKey(rightVariable))
                {
                    diagnostics.Add(rightVariable, new Dictionary<int, List<Diagnostic>>());
                }
                if (!diagnostics[rightVariable].ContainsKey(location))
                {
                    diagnostics[rightVariable].Add(location, new List<Diagnostic>());
                }


                if (variableValues[rightVariable][rightLocation].display)
                {
                    Console.WriteLine($"Location: {location}, Message: {rightVariable}: {variableValues[rightVariable][rightLocation].ToString()} ");
                }

            }


            string finalValue = await Evaluate.evaluateFunction(functionCode, RHS.ToString(), currentValues);
            int variableFinalValue = 0;
            bool canConvert = int.TryParse(finalValue, out variableFinalValue);
            if (canConvert)
            {
                variableValues[variable][location] = new Value( variableFinalValue, false) ;
            }
            else
            {
                variableValues[variable][location] = new Value(sum, false);
            }
            if (variableValues[variable][location].display)
            {
                Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} ");
            }
        }

        static async Task AnalyzeExpressionAsync(string variableName, ExpressionSyntax assignment, int location)
        {
            ExpressionSyntax RHS = assignment;
            string variable = variableName;

            if (!variableValues.ContainsKey(variable))
            {
                variableValues.Add(variable, new Dictionary<int, Value>());
            }
            if (!variableValues[variable].ContainsKey(location))
            {
                variableValues[variable].Add(location, new Value(new object(), true));
            }

            int numericValue = 0;
            bool canParse = int.TryParse(RHS.ToString().Trim(), out numericValue);
            if (canParse)
            {
                variableValues[variable][location] = new Value(numericValue, true);
                Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} ");
                return;
            }

            if (RHS is IdentifierNameSyntax)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)(RHS);
                string rightVariable = identifier.ToString();
                int rightLocation = updates[rightVariable][updates[rightVariable].Count - 1];

                if (!variableDependencies.ContainsKey(variable))
                {
                    variableDependencies.Add(variable, new Dictionary<int, List<VariableLocation>>());
                }
                if (!variableDependencies[variable].ContainsKey(location))
                {
                    variableDependencies[variable].Add(location, new List<VariableLocation>());
                }

                VariableLocation obj = new VariableLocation();
                obj.variable = rightVariable;
                obj.location = rightLocation;

                variableDependencies[variable][location].Add(obj);

                Value rightValue = variableValues[rightVariable][rightLocation];

                variableValues[variable][location] = new Value(rightValue.value, rightValue.display);

                if (!diagnostics.ContainsKey(rightVariable))
                {
                    diagnostics.Add(rightVariable, new Dictionary<int, List<Diagnostic>>());
                }
                if (!diagnostics[rightVariable].ContainsKey(location))
                {
                    diagnostics[rightVariable].Add(location, new List<Diagnostic>());
                }

                if (variableValues[rightVariable][rightLocation].display)
                {
                    Console.WriteLine($"Location: {location}, Message: {rightVariable}: {variableValues[rightVariable][rightLocation].ToString()} ");
                }
                

                if (variableValues[variable][location].display)
                {
                    Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} ");
                }
                return;
            }

            IEnumerable<IdentifierNameSyntax> identifiers = RHS.DescendantNodes().OfType<IdentifierNameSyntax>();
            int sum = 0;
            bool display = true;
            Dictionary<string, object> currentValues = new Dictionary<string, object>();
            foreach (var item in identifiers)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)(item);
                string rightVariable = identifier.ToString();
                
                int rightLocation = updates[rightVariable][updates[rightVariable].Count - 1];

                if (!variableDependencies.ContainsKey(variable))
                {
                    variableDependencies.Add(variable, new Dictionary<int, List<VariableLocation>>());
                }
                if (!variableDependencies[variable].ContainsKey(location))
                {
                    variableDependencies[variable].Add(location, new List<VariableLocation>());
                }

                VariableLocation obj = new VariableLocation();
                obj.variable = rightVariable;
                obj.location = rightLocation;

                if (!currentValues.ContainsKey(rightVariable))
                {
                    currentValues.Add(rightVariable, (variableValues[rightVariable][rightLocation].value));
                }
                
                display = display && variableValues[rightVariable][rightLocation].display;
                variableDependencies[variable][location].Add(obj);

                if (!diagnostics.ContainsKey(rightVariable))
                {
                    diagnostics.Add(rightVariable, new Dictionary<int, List<Diagnostic>>());
                }
                if (!diagnostics[rightVariable].ContainsKey(location))
                {
                    diagnostics[rightVariable].Add(location, new List<Diagnostic>());
                }

                bool printRHS = rightVariable.Equals(variable);
                if (variableValues[rightVariable][rightLocation].display)
                {
                    Console.WriteLine($"Location: {location}, Message: {rightVariable}: {variableValues[rightVariable][rightLocation].ToString()} ");
                }

            }

            string finalValue = await Evaluate.evaluateExpression(RHS.ToString(), currentValues);
            int variableFinalValue = 0;
            bool canConvert = int.TryParse(finalValue, out variableFinalValue);
            if (canConvert)
            {
                variableValues[variable][location] = new Value(variableFinalValue, display);
                
            }
            else
            {
                variableValues[variable][location] = new Value(sum, display);
            }

            if (variableValues[variable][location].display)
            {
                Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} ");
            }
            
        }

        public static async Task generateRoslynAnalyzer(string textParam = "")
        {
            if (textParam == "") { textParam = CodeString.test; }
            //Console.WriteLine(textParam);
            variableDependencies = new Dictionary<string, Dictionary<int, List<VariableLocation>>>();
            updates = new Dictionary<string, List<int>>();
            diagnostics = new Dictionary<string, Dictionary<int, List<Diagnostic>>>();
            variableValues = new Dictionary<string, Dictionary<int, Value>>();
            functions = new Dictionary<string, string>();

            SyntaxTree AST = CSharpSyntaxTree.ParseText(textParam);
            SyntaxNode compilationRoot = AST.GetRoot();
            CompilationUnitSyntax root = AST.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("HelloWorld")
                .AddReferences(MetadataReference.CreateFromFile(
                typeof(string).Assembly.Location))
                .AddSyntaxTrees(AST);

            SemanticModel semanticModel = compilation.GetSemanticModel(AST);
            IEnumerable<SyntaxNode> descendentNodes = root.DescendantNodes();

            IEnumerable<MethodDeclarationSyntax> methods = descendentNodes.OfType<MethodDeclarationSyntax>();

            MethodDeclarationSyntax? MDS = null;
            foreach(MethodDeclarationSyntax method in methods)
            {
                if(method.Identifier.ToString().Equals("Main"))
                {
                    //Console.WriteLine("MAIN");
                    MDS = method;
                    continue;
                }
                if (!functions.ContainsKey(method.Identifier.ToString()))
                {
                    functions.Add(method.Identifier.ToString(), method.ToString());
                }
                
            }
            if(MDS == null)
            {
                return;
            }

            await roslynAnalyzerAsync(MDS);

            //Dependency Printout
            Console.WriteLine("=====Dependencies=====");
            foreach(KeyValuePair<string, Dictionary<int, List<VariableLocation>>> kvp in variableDependencies)
            {
                string key = kvp.Key;
                Dictionary<int, List<VariableLocation>> dictionary = kvp.Value;

                foreach (KeyValuePair<int, List<VariableLocation>> keyValuePair in dictionary)
                {
                    int location = keyValuePair.Key;
                    List<VariableLocation> dependencies = keyValuePair.Value;
                    foreach(VariableLocation vL in dependencies)
                    {
                        Console.WriteLine($"Variable Name: {key}, Location: {location}, Variable Dependency: {vL.variable}, Variable Location: {vL.location}");
                    }
                }
            }

            //Variable Values Printout
            Console.WriteLine("=====Variable Values=====");
            foreach (KeyValuePair<string, Dictionary<int, Value>> kvp in variableValues)
            {
                string key = kvp.Key;
                Dictionary<int, Value> dictionary = kvp.Value;

                foreach (KeyValuePair<int, Value> keyValuePair in dictionary)
                {
                    int location = keyValuePair.Key;
                    Value vL = keyValuePair.Value;
                    Console.WriteLine($"Variable Name: {key}, Location: {location}, Value: {vL.value}");
                }
            }


        }
        public static async Task roslynAnalyzerAsync(MethodDeclarationSyntax MDS)
        {
            IEnumerable<SyntaxNode> descendentNodes = MDS.DescendantNodes();
            IEnumerable<VariableDeclarationSyntax> variableDeclarations = descendentNodes.OfType<VariableDeclarationSyntax>();
            IEnumerable<AssignmentExpressionSyntax> assignments = descendentNodes.OfType<AssignmentExpressionSyntax>();

            foreach (SyntaxNode syntaxNode in descendentNodes)
            {
                if (syntaxNode is VariableDeclarationSyntax)
                {
                    await processDeclaration((VariableDeclarationSyntax)syntaxNode);
                }
                else if (syntaxNode is AssignmentExpressionSyntax)
                {
                    await processAssignment((AssignmentExpressionSyntax)syntaxNode);
                }
            }
        }
    }
}

