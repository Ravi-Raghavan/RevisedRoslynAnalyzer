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
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslynAnalyzer : DiagnosticAnalyzer
    {
        private static Dictionary<string, Dictionary<int, List<VariableLocation>>> variableDependencies = new Dictionary<string, Dictionary<int, List<VariableLocation>>>();
        private static Dictionary<string, List<int>> updates = new Dictionary<string, List<int>>();
        private static Dictionary<string, Dictionary<int, List<Diagnostic>>> diagnostics = new Dictionary<string, Dictionary<int, List<Diagnostic>>>();
        private static Dictionary<string, Dictionary<int, Value>> variableValues = new Dictionary<string, Dictionary<int, Value>>();
        private static Dictionary<string, string> functions = new Dictionary<string, string>();
        private static CSharpCompilation compilation = CSharpCompilation.Create("");
        private static SemanticModelAnalysisContext semanticModelAnalysisContext = new SemanticModelAnalysisContext();
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
            = ImmutableArray.Create(Descriptors.variableValue);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                                   GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSemanticModelAction(action: generateRoslynAnalyzer);

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
                AssignmentExpressionSyntax assignment = nestedDescendantNodes.OfType<AssignmentExpressionSyntax>().First();
                var typeInfo = compilation.GetSemanticModel(item.SyntaxTree).GetTypeInfo(declaration.Type);
                variableValues.Add(variableName, new Dictionary<int, Value>());
                variableValues[variableName].Add(location, new Value(new object(), true, typeInfo.Type.ToString()));
                diagnostics.Add(variableName, new Dictionary<int, List<Diagnostic>>());
                diagnostics[variableName].Add(location, new List<Diagnostic>());
                variableDependencies.Add(variableName, new Dictionary<int, List<VariableLocation>>());
                variableDependencies[variableName].Add(location, new List<VariableLocation>());
                foreach (var nestedAssignment in nestedAssignments)
                {
                    if (Arithmetic.isArithmetic(diagnostics, nestedAssignment.Value)) { await AnalyzeExpressionAsync(variableName, assignment.Left,
                                                                                            nestedAssignment.Value, location); }
                    else { await AnalyzeExpressionNonArithmeticAsync(variableName, assignment.Left, nestedAssignment.Value, location); }
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
            int previousLocation = updates[variableName][updates[variableName].Count - 1];
            string dataType = variableValues[variableName][previousLocation].dataType;
            variableValues[variableName].Add(location, new Value(new object(), true, dataType));
            diagnostics[variableName].Add(location, new List<Diagnostic>());
            variableDependencies[variableName].Add(location, new List<VariableLocation>());

            if (Arithmetic.isArithmetic(diagnostics, syntaxNode.Right))
            {
                await AnalyzeExpressionAsync(variableName, variableAssignment.Left, variableAssignment.Right, location);
            }
            else
            {
                await AnalyzeExpressionNonArithmeticAsync(variableName, variableAssignment.Left, variableAssignment.Right, location);
            }
            updates[variableName].Add(location);
        }

        static string getIdentifierValuesNonArithmetic(IEnumerable<IdentifierNameSyntax> identifiers,
                                                                string variableName, int location,
                                                                 Dictionary<string, Value> currentValues)
        {
            string variable = variableName;
            string functionCode = "";
            string[] messageArray = new string[2];
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
                VariableLocation obj = new VariableLocation();
                obj.variable = rightVariable;
                obj.location = rightLocation;
                if (!currentValues.ContainsKey(rightVariable))
                {
                    currentValues.Add(rightVariable, (variableValues[rightVariable][rightLocation]));
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
                //Console.WriteLine($"Location: {location}, Message: {rightVariable}: {variableValues[rightVariable][rightLocation].ToString()}, Type: {variableValues[rightVariable][rightLocation].dataType}");
                messageArray[0] = rightVariable;
                messageArray[1] = variableValues[rightVariable][rightLocation].ToString();
                if(messageArray[1] != "")
                {
                    semanticModelAnalysisContext.ReportDiagnostic(
                        Diagnostic.Create(
                            Descriptors.variableValue,
                            location: identifier.GetLocation(),
                            messageArgs: messageArray
                            )
                        );
                }
                
            }
            return functionCode;
        } 

        static async Task AnalyzeExpressionNonArithmeticAsync(string variableName, ExpressionSyntax LHS,
                                                            ExpressionSyntax assignment, int location)
        {
            ExpressionSyntax RHS = assignment;
            string variable = variableName;
            IEnumerable<IdentifierNameSyntax> identifiers = RHS.DescendantNodes().OfType<IdentifierNameSyntax>();
            int sum = 0;
            string functionCode = "";
            Dictionary<string, Value> currentValues = new Dictionary<string, Value>();
            functionCode = getIdentifierValuesNonArithmetic(identifiers, variableName, location, currentValues);
            string finalValue = await Evaluate.evaluateFunction(functionCode, RHS.ToString(), currentValues);
            string dataType = variableValues[variable][location].dataType;
            variableValues[variable][location] = new Value(finalValue, false, dataType);
            //Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} " +
            //    $"Final Type: {variableValues[variable][location].dataType}");
            string[] messageArray = new string[2];
            messageArray[0] = variable;
            messageArray[1] = variableValues[variable][location].ToString();
            if (messageArray[1] != "")
            {
                semanticModelAnalysisContext.ReportDiagnostic(
                    Diagnostic.Create(
                        Descriptors.variableValue,
                        location: LHS.GetLocation(),
                        messageArgs: messageArray
                        )
                    );
            }
        }

        static bool getIdentifierValuesArithmetic(IEnumerable<IdentifierNameSyntax> identifiers,
                                                   Dictionary<string, Value> currentValues,
                                                   string variable, int location)
        {
            bool display = true;
            string[] messageArray = new string[2];
            foreach (var item in identifiers)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)(item);
                string rightVariable = identifier.ToString();

                int rightLocation = updates[rightVariable][updates[rightVariable].Count - 1];

                VariableLocation obj = new VariableLocation();
                obj.variable = rightVariable;
                obj.location = rightLocation;

                if (!currentValues.ContainsKey(rightVariable))
                {
                    currentValues.Add(rightVariable, (variableValues[rightVariable][rightLocation]));
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
                //Console.WriteLine($"Location: {location}, Message: {rightVariable}: {variableValues[rightVariable][rightLocation].ToString()},Type: {variableValues[rightVariable][rightLocation].dataType}");
                messageArray[0] = rightVariable;
                messageArray[1] = variableValues[rightVariable][rightLocation].ToString();
                if (messageArray[1] != "")
                {
                    semanticModelAnalysisContext.ReportDiagnostic(
                        Diagnostic.Create(
                            Descriptors.variableValue,
                            location: identifier.GetLocation(),
                            messageArgs: messageArray
                            )
                        );
                }
            }
            return display;
        }

        static async Task AnalyzeExpressionAsync(string variableName, ExpressionSyntax LHS,
                                                    ExpressionSyntax assignment, int location)
        {
            ExpressionSyntax RHS = assignment;
            string variable = variableName;
            string dataType = (variableValues[variable][location].dataType);
            int numericValue = 0;
            bool canParse = int.TryParse(RHS.ToString().Trim(), out numericValue);
            string[] messageArray = new string[2];
            if (canParse)
            {
                
                variableValues[variable][location] = new Value(numericValue, true, dataType);
                //Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()}, Type: {variableValues[variable][location].dataType}");
                messageArray[0] = variable;
                messageArray[1] = variableValues[variable][location].ToString();
                if (messageArray[1] != "")
                {
                    semanticModelAnalysisContext.ReportDiagnostic(
                        Diagnostic.Create(
                            Descriptors.variableValue,
                            location: LHS.GetLocation(),
                            messageArgs: messageArray
                            )
                        );
                }
                return;
            }

            IEnumerable<IdentifierNameSyntax> identifiers = RHS.DescendantNodes().OfType<IdentifierNameSyntax>();
            if(RHS is IdentifierNameSyntax)
            {
                List<IdentifierNameSyntax> identifierList = new List<IdentifierNameSyntax>();
                identifierList.Add((IdentifierNameSyntax)(RHS));
                identifiers = identifierList.AsEnumerable();
            }
            int sum = 0;
            bool display = true;
            Dictionary<string, Value> currentValues = new Dictionary<string, Value>();
            display = getIdentifierValuesArithmetic(identifiers, currentValues, variable, location);
            string finalValue = await Evaluate.evaluateExpression(RHS.ToString(), currentValues);
            variableValues[variable][location] = new Value(finalValue, display, dataType);
            //Console.WriteLine($"Location: {location}, Message: {variable}: {variableValues[variable][location].ToString()} " +
            //    $"Final Type: {variableValues[variable][location].dataType}");

            messageArray[0] = variable;
            messageArray[1] = variableValues[variable][location].ToString();
            if (messageArray[1] != "")
            {
                semanticModelAnalysisContext.ReportDiagnostic(
                    Diagnostic.Create(
                        Descriptors.variableValue,
                        location: LHS.GetLocation(),
                        messageArgs: messageArray
                        )
                    );
            }

        }

        public static void generateRoslynAnalyzer(SemanticModelAnalysisContext modelAnalysisContext)
        {
            //if (textParam == "") { textParam = CodeString.test; }
            //Console.WriteLine(textParam);
            variableDependencies = new Dictionary<string, Dictionary<int, List<VariableLocation>>>();
            updates = new Dictionary<string, List<int>>();
            diagnostics = new Dictionary<string, Dictionary<int, List<Diagnostic>>>();
            variableValues = new Dictionary<string, Dictionary<int, Value>>();
            functions = new Dictionary<string, string>();
            semanticModelAnalysisContext = modelAnalysisContext;

            SemanticModel semanticModel = modelAnalysisContext.SemanticModel;
            SyntaxTree AST = semanticModel.SyntaxTree;
            SyntaxNode compilationRoot = AST.GetRoot();
            CompilationUnitSyntax root = AST.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("HelloWorld")
                .AddReferences(MetadataReference.CreateFromFile(
                typeof(string).Assembly.Location))
                .AddSyntaxTrees(AST);



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

            roslynAnalyzerAsync(MDS);

            


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
            /*
            //Dependency Printout
            Console.WriteLine("=====Dependencies=====");
            foreach (KeyValuePair<string, Dictionary<int, List<VariableLocation>>> kvp in variableDependencies)
            {
                string key = kvp.Key;
                Dictionary<int, List<VariableLocation>> dictionary = kvp.Value;

                foreach (KeyValuePair<int, List<VariableLocation>> keyValuePair in dictionary)
                {
                    int location = keyValuePair.Key;
                    List<VariableLocation> dependencies = keyValuePair.Value;
                    foreach (VariableLocation vL in dependencies)
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
                    Console.WriteLine($"Variable Name: {key}, Location: {location}, Value: {vL.value}, Type: {vL.dataType}");
                }
            }
            */
        }
    }
}

