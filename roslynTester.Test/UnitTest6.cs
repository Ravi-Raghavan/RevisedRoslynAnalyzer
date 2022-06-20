using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
	/**
	 * 
	 * 

Value of a at Location : (5,21)-(5,22) is 2
Value of b at Location : (6,21)-(6,22) is 1
Value of a at Location : (7,25)-(7,26) is 2
Value of b at Location : (7,29)-(7,30) is 1
Value of a at Location : (8,27)-(8,28) is 2
Value of a at Location : (9,25)-(9,26) is 2
	 */
	public class UnitTest6 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test6() {
			TestCode = CodeString.testSix;
			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(5, 21, 5, 22)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 21, 6, 22)
				.WithArguments("b", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 25, 7, 26)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 29, 7, 30)
				.WithArguments("b", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(8, 27, 8, 28)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(9, 25, 9, 26)
				.WithArguments("a", 2)
			);
			await RunAsync();

		}

	}
}

