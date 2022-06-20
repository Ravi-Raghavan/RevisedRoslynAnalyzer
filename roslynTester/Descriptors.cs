using System;
using Microsoft.CodeAnalysis;

namespace roslynTester
{
    public static class Descriptors
    {


        public static readonly DiagnosticDescriptor variableValue = new(
            id: "PG0002",
            title: "variableValue",
            messageFormat: "Value of {0} is {1}",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
            );

    }
}

