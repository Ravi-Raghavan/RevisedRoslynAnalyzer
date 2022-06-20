using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;
/**
 * 
 * Value of x at Location : (9,17)-(9,18) is 1	D
Value of x at Location : (10,21)-(10,22) is 1	D
Value of y at Location : (10,17)-(10,18) is 2	D
Value of x at Location : (11,25)-(11,26) is 1	D
Value of y at Location : (11,33)-(11,34) is 2	D
Value of y at Location : (12,25)-(12,26) is 2	D
Value of x at Location : (12,37)-(12,38) is 1	D
Value of y at Location : (12,42)-(12,43) is 2	D
Value of x at Location : (12,45)-(12,46) is 1	D
Value of x at Location : (13,21)-(13,22) is 1	D
Value of x at Location : (14,17)-(14,18) is 1	D
Value of x at Location : (14,21)-(14,22) is 1	D
Value of y at Location : (14,13)-(14,14) is 2	D
Value of y at Location : (15,17)-(15,18) is 2	D
Value of y at Location : (15,33)-(15,34) is 2	D
Value of d at Location : (16,20)-(16,21) is 5.4	D
Value of d at Location : (17,13)-(17,14) is 7.4	D
 */
namespace roslynTester.Test
{
	public class UnitTest2: CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
	{
		[Fact]
		public async Task Test2()
        {
			TestCode = CodeString.testTwo;

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(9, 17, 9, 18)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(10, 21, 10, 22)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(10, 17, 10, 18)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(11, 25, 11, 26)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(11, 33, 11, 34)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 25, 12, 26)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 37, 12, 38)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 42, 12, 43)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(12, 45, 12, 46)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(13, 21, 13, 22)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(14, 17, 14, 18)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(14, 21, 14, 22)
				.WithArguments("x", 1)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(14, 13, 14, 14)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(15, 17, 15, 18)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(15, 33, 15, 34)
				.WithArguments("y", 2)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(16, 20, 16, 21)
				.WithArguments("d", 5.4)
			);

			ExpectedDiagnostics.Add(
			new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
				.WithSpan(17, 13, 17, 14)
				.WithArguments("d", 7.4)
			);

			await RunAsync();
		}
	}
}

