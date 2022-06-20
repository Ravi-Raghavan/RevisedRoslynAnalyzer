using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
namespace roslynTester.Test
{
	/*
Value of x at Location : (8,17)-(8,18) is 1	D
Value of y at Location : (9,17)-(9,18) is 2	D
Value of z at Location : (10,17)-(10,18) is 3		D
Value of w at Location : (11,17)-(11,18) is 4		D
Value of w at Location : (12,21)-(12,22) is 4	D
Value of z at Location : (12,29)-(12,30) is 3	D
Value of x at Location : (12,37)-(12,38) is 1	D
Value of y at Location : (12,13)-(12,14) is 10	D
Value of y at Location : (13,21)-(13,22) is 10	D
Value of x at Location : (13,29)-(13,30) is 1		D
Value of w at Location : (13,37)-(13,38) is 4	D
Value of x at Location : (13,13)-(13,14) is 14
	 */
	public class UnitTest5 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test5()
        {
			TestCode = CodeString.testFive;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(8, 17, 8, 18)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(9, 17, 9, 18)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(10, 17, 10, 18)
				.WithArguments("z", 3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(11, 17, 11, 18)
				.WithArguments("w", 4)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 21, 12, 22)
				.WithArguments("w", 4)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 29, 12, 30)
				.WithArguments("z", 3)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 37, 12, 38)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 13, 12, 14)
				.WithArguments("y", 10)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(13, 21, 13, 22)
				.WithArguments("y", 10)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(13, 29, 13, 30)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(13, 37, 13, 38)
				.WithArguments("w", 4)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(13, 13, 13, 14)
				.WithArguments("x", 14)
			);

			await RunAsync();
		}
	}
}

