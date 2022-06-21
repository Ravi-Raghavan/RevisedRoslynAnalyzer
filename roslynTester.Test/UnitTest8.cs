using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
//	Value of a at Location : (4,21)-(4,22) is 2
//Value of b at Location : (5,21)-(5,22) is 3
//Value of a at Location : (6,21)-(6,22) is 2
//Value of b at Location : (6,25)-(6,26) is 3
//Value of a at Location : (6,17)-(6,18) is 5
	public class UnitTest8 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test8()
        {
			TestCode = CodeString.testEight;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(4, 21, 4, 22)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(5, 21, 5, 22)
				.WithArguments("b", 3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 21, 6, 22)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 25, 6, 26)
				.WithArguments("b", 3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 17, 6, 18)
				.WithArguments("a", 5)
			);

			await RunAsync();

		}
	}
}

