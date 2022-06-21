using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
	public class UnitTest9 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
//		Value of b at Location : (8,20)-(8,21) is 10
//Value of b at Location : (9,24)-(9,25) is 10

		[Fact]
		public async Task Test9()
        {
			TestCode = CodeString.testNine;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(8, 20, 8, 21)
				.WithArguments("b", 10)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(9, 24, 9, 25)
				.WithArguments("b", 10)
			);

		}

	}
}

