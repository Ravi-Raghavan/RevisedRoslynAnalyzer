using System;
using Microsoft.CodeAnalysis;

namespace roslynTester
{
    public static class Descriptors
    {


        public static readonly DiagnosticDescriptor variableValue = new(
            id: "PG0002",
            title: "variableValue",
            messageFormat: "{0}",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
            );

    }
}

