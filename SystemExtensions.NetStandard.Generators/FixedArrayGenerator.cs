using Microsoft.CodeAnalysis;
using Sybil;
using System.Text;
using System.Linq;

namespace System.Extensions;
#nullable enable
[Generator(LanguageNames.CSharp)]
public class FixedArrayGenerator : IIncrementalGenerator
{
    private const string UsingSystem = "System";
    private const string UsingSystemRuntimeInterop = "System.Runtime.InteropServices";
    private const string AttributeNamespace = "System.Extensions";
    private const string AttributeName = "GenerateFixedArrayAttribute";
    private const string AttributeShortName = "GenerateFixedArray";
    private const string Array = "Array";
    private const string Public = "public";
    private const string Struct = "struct";
    private const string Required = "required";
    private const string Size = "Size";
    private const string Int = "int";
    private const string TType = "T";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            var compilationUnitBuilder = SyntaxBuilder.CreateCompilationUnit()
                .WithNamespace(
                SyntaxBuilder.CreateNamespace(AttributeNamespace)
                    .WithClass(SyntaxBuilder.CreateClass(AttributeName)
                        .WithModifier(Public)
                        .WithTypeParameter(SyntaxBuilder.CreateTypeParameter(TType))
                        .WithTypeParameterConstraint(SyntaxBuilder.CreateTypeParameterConstraint(TType)
                            .WithType(Struct))
                        .WithConstructor(SyntaxBuilder.CreateConstructor(AttributeName)
                            .WithModifier(Public))
                        .WithAttribute(SyntaxBuilder.CreateAttribute("AttributeUsage")
                            .WithArgument(AttributeTargets.Assembly)
                            .WithArgument("Inherited", false)
                            .WithArgument("AllowMultiple", true))
                        .WithProperty(SyntaxBuilder.CreateProperty(Int, Size)
                            .WithModifier(Public)
                            .WithModifier(Required)
                            .WithAccessor(SyntaxBuilder.CreateGetter())
                            .WithAccessor(SyntaxBuilder.CreateSetter()))
                        .WithBaseClass(nameof(Attribute))));
            var compilationUnitSyntax = compilationUnitBuilder.Build();
            var source = compilationUnitSyntax.ToFullString();
            context.AddSource($"{AttributeName}.g", source);
        });

        context.RegisterSourceOutput(context.CompilationProvider, (sourceProductionContext, compilation) =>
        {
            var assemblyAttributes =
            compilation.Assembly.GetAttributes().Where(a => a.AttributeClass != null &&
                        (a.AttributeClass.Name == AttributeName || a.AttributeClass.Name == AttributeShortName));

            foreach(var attr in assemblyAttributes)
            {
                // Extract the type argument (T) from GenerateFixedArrayAttribute<T>
                var namedTypeSymbol = attr.AttributeClass as INamedTypeSymbol;
                if (namedTypeSymbol is null || namedTypeSymbol.TypeArguments.Length != 1)
                {
                    continue;
                }

                var targetType = namedTypeSymbol.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                // Extract the Size parameter from the named arguments.
                int? fixedSize = default;
                foreach (var arg in attr.NamedArguments)
                {
                    if (arg.Key == Size && arg.Value.Value is int intValue)
                    {
                        fixedSize = intValue;
                        break;
                    }
                }

                if (fixedSize is null)
                {
                    // As Size is required, skip if not provided.
                    continue;
                }

                Execute(sourceProductionContext, targetType, (int)fixedSize);
            }
        });
    }

    private static void Execute(SourceProductionContext sourceProductionContext, string targetType, int size)
    {
        var capitalizedType = targetType.ToUpperInvariant()[0] + targetType.Substring(1);
        var structName = $"{Array}{size}{capitalizedType}";
        var builder = new StringBuilder();
        builder.AppendLine($"using {UsingSystem};");
        builder.AppendLine($"using {UsingSystemRuntimeInterop};");
        builder.AppendLine();
        builder.AppendLine($"namespace {AttributeNamespace}");
        builder.AppendLine("{");
        builder.AppendLine("    [StructLayout(LayoutKind.Sequential, Pack = 1)]");
        builder.AppendLine($"    public unsafe struct {structName}");
        builder.AppendLine("    {");
        builder.AppendLine($"        private const int Size = {size};");
        builder.AppendLine();
        builder.AppendLine($"        private fixed {targetType} elements[Size];");
        builder.AppendLine();
        builder.AppendLine("        public int Length => Size;");
        builder.AppendLine();
        builder.AppendLine($"        public ref {targetType} this[int index]");
        builder.AppendLine("        {");
        builder.AppendLine("            get");
        builder.AppendLine("            {");
        builder.AppendLine("                if (index >= Size) throw new IndexOutOfRangeException();");
        builder.AppendLine($"                fixed ({targetType}* ptr = this.elements)");
        builder.AppendLine("                {");
        builder.AppendLine("                    return ref ptr[index];");
        builder.AppendLine("                }");
        builder.AppendLine("            }");
        builder.AppendLine("        }");
        builder.AppendLine();
        builder.AppendLine($"        public Span<{targetType}> AsSpan()");
        builder.AppendLine("        {");
        builder.AppendLine($"            fixed ({targetType}* ptr = this.elements)");
        builder.AppendLine("            {");
        builder.AppendLine($"                return new Span<{targetType}>(ptr, Size);");
        builder.AppendLine("            }");
        builder.AppendLine("        }");
        builder.AppendLine();
        builder.AppendLine($"        public static {structName} From{capitalizedType}Array({targetType}[] array)");
        builder.AppendLine("        {");
        builder.AppendLine($"            var arr = default({structName});");
        builder.AppendLine("            array.AsSpan().CopyTo(arr.AsSpan());");
        builder.AppendLine("            return arr;");
        builder.AppendLine("        }");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        sourceProductionContext.AddSource($"{structName}.g.cs", builder.ToString());
    }
}
#nullable disable
