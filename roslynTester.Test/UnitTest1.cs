using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;

namespace roslynTester.Test;

public class UnitTest1 : CSharpAnalyzerTest<RoslynAnalyzer2, XUnitVerifier>
{
    [Fact]
    public async Task Test1()
    {
        TestCode = CodeString.testOne;

        ExpectedDiagnostics.Clear();
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(7, 21, 7, 22)
                .WithArguments("x", "3")

            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(8, 25, 8, 26)
                .WithArguments("x", "3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(8, 21, 8, 22)
                .WithArguments("y", "6")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(9, 29, 9, 30)
                .WithArguments("x", "3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(9, 34, 9, 35)
                .WithArguments("y", "6")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(10, 25, 10, 26)
                .WithArguments("x", "3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(11, 21, 11, 22)
                .WithArguments("x", "3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(11, 25, 11, 26)
                .WithArguments("x", "3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithSpan(11, 17, 11, 18)
                .WithArguments("y", "6")
            );
        await RunAsync();
    }
}
