using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using System;
using Microsoft.CodeAnalysis;

namespace roslynTester.Test;

public class UnitTest1 : CSharpAnalyzerTest<RoslynAnalyzer, XUnitVerifier>
{
    [Fact]
    public void Test1()
    {
        TestCode = CodeString.testOne;

        ExpectedDiagnostics.Clear();
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of y is 6")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of y is 6")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of x is 3")
            );
        ExpectedDiagnostics.Add(
            new DiagnosticResult(Descriptors.variableValue.Id, DiagnosticSeverity.Info)
                .WithMessage("Value of y is 6")
            );

    }
}
