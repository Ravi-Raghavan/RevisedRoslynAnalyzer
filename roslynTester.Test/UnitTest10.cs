using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{


//	Value of a at Location : (8,24)-(8,25) is 2.3
//Value of b at Location : (9,24)-(9,25) is 2.45
//Value of a at Location : (10,32)-(10,33) is 2.3
//Value of a at Location : (11,25)-(11,26) is 2.3
//Value of b at Location : (11,17)-(11,18) is 4.6
	public class UnitTest10 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test10()
        {
			TestCode = CodeString.testTen;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(8, 24, 8, 25)
				.WithArguments("a", 2.3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(9, 24, 9, 25)
				.WithArguments("b", 2.45)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(10, 32, 10, 33)
				.WithArguments("a", 2.3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(11, 25, 11, 26)
				.WithArguments("a", 2.3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(11, 17, 11, 18)
				.WithArguments("b", 4.6)
			);

			await RunAsync();
		}
	}
}

