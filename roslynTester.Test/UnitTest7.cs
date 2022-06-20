using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
	/*
	 * 
Value of a at Location : (6,21)-(6,22) is 2
Value of a at Location : (7,27)-(7,28) is 2
	 */
	public class UnitTest7 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test7()
        {
			TestCode = CodeString.testSeven;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 21, 6, 22)
				.WithArguments("a", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 27, 7, 28)
				.WithArguments("a", 2)
			);

			await RunAsync();
		}
	}
}

