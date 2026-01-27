using Microsoft.CodeAnalysis;
using Sybil;
using System.Text;
using System.Linq;

namespace System.Extensions;
#nullable enable
[Generator(LanguageNames.CSharp)]
public class BlittableStringGenerator : IIncrementalGenerator
{
    private const string UsingSystem = "System";
    private const string UsingSystemRuntimeInterop = "System.Runtime.InteropServices";
    private const string AttributeNamespace = "System.Extensions";
    private const string AttributeName = "GenerateBlittableStringAttribute";
    private const string AttributeShortName = "GenerateBlittableString";
    private const string String = "String";
    private const string Public = "public";
    private const string Required = "required";
    private const string Size = "Size";
    private const string Int = "int";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            var compilationUnitBuilder = SyntaxBuilder.CreateCompilationUnit()
                .WithNamespace(
                SyntaxBuilder.CreateNamespace(AttributeNamespace)
                    .WithClass(SyntaxBuilder.CreateClass(AttributeName)
                        .WithModifier(Public)
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

            foreach (var attr in assemblyAttributes)
            {
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

                Execute(sourceProductionContext, (int)fixedSize);
            }
        });
    }

    private static void Execute(SourceProductionContext sourceProductionContext, int size)
    {
        var structName = $"{String}{size}";
        var builder = new StringBuilder();
        builder.AppendLine($"using {UsingSystem};");
        builder.AppendLine($"using {UsingSystemRuntimeInterop};");
        builder.AppendLine();
        builder.AppendLine($"namespace {AttributeNamespace};");
        builder.AppendLine();
        builder.AppendLine("[StructLayout(LayoutKind.Sequential, Pack = 1)]");
        builder.AppendLine($"public unsafe partial struct {structName}");
        builder.AppendLine("{");
        builder.AppendLine($"    public const int Size = {size};");
        builder.AppendLine();
        builder.AppendLine("    public fixed char Value[Size];");
        builder.AppendLine();
        builder.AppendLine("    public Span<char> AsSpan()");
        builder.AppendLine("    {");
        builder.AppendLine("        fixed (char* ptr = this.Value)");
        builder.AppendLine("        {");
        builder.AppendLine("            return MemoryMarshal.CreateSpan(ref *ptr, Size);");
        builder.AppendLine("        }");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine("    public readonly ReadOnlySpan<char> AsReadOnlySpan()");
        builder.AppendLine("    {");
        builder.AppendLine("        fixed (char* ptr = this.Value)");
        builder.AppendLine("        {");
        builder.AppendLine("            return new ReadOnlySpan<char>(ptr, Size);");
        builder.AppendLine("        }");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine("    public void Set(ReadOnlySpan<char> value)");
        builder.AppendLine("    {");
        builder.AppendLine("        fixed (char* ptr = this.Value)");
        builder.AppendLine("        {");
        builder.AppendLine("            var span = MemoryMarshal.CreateSpan(ref *ptr, Size);");
        builder.AppendLine("            span.Clear();");
        builder.AppendLine("            value[..Math.Min(value.Length, Size - 1)].CopyTo(span);");
        builder.AppendLine("        }");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine("    public override readonly string ToString()");
        builder.AppendLine("    {");
        builder.AppendLine("        var span = this.AsReadOnlySpan();");
        builder.AppendLine("        var nullIndex = span.IndexOf('\\0');");
        builder.AppendLine("        return nullIndex >= 0 ? new string(span[..nullIndex]) : new string(span);");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine($"    public static implicit operator string({structName} value) => value.ToString();");
        builder.AppendLine();
        builder.AppendLine($"    public static implicit operator {structName}(string value)");
        builder.AppendLine("    {");
        builder.AppendLine($"        var result = default({structName});");
        builder.AppendLine("        result.Set(value);");
        builder.AppendLine("        return result;");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine($"    public static implicit operator ReadOnlySpan<char>({structName} value) => value.AsReadOnlySpan();");
        builder.AppendLine("}");

        sourceProductionContext.AddSource($"{structName}.g.cs", builder.ToString());
    }
}
#nullable disable
