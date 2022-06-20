using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
	public class UnitTest3: CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public void Test3()
        {
			TestCode = CodeString.testThree;
			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(4, 21, 4, 22)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(5, 21, 5, 22)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 25, 6, 26)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 29, 6, 30)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(6, 21, 6, 22)
				.WithArguments("z", 3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 25, 7, 26)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 29, 7, 30)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 33, 7, 34)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(7, 21, 7, 22)
				.WithArguments("w", 4)
			);
		}
	}
}

