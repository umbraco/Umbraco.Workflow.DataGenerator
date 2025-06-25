using System.Reflection;
using Umbraco.Workflow.Core;
using Umbraco.Workflow.Core.Persistence;

namespace Umbraco.Workflow.DataGenerator.Configuration;

internal sealed class WorkflowDestructor : IWorkflowDestructor
{
    private readonly IDatabaseAccessor _databaseAccessor;

    public WorkflowDestructor(IDatabaseAccessor databaseAccessor) => _databaseAccessor = databaseAccessor;

    /// <inheritdoc/>
    public async Task Armageddon()
    {
        string[] deleteThese = GetTableNames([Constants.Tables.Settings]);

        foreach (string tableName in deleteThese)
        {
            await _databaseAccessor.Execute($"DELETE FROM {tableName}");
        }
    }

    private string[] GetTableNames(string[]? exclude = null)
    {
        // Get the nested 'Tables' class type
        Type? tablesType = typeof(Constants).GetNestedType("Tables", BindingFlags.Public);
        if (tablesType is null)
        {
            return [];
        }

        // Get all constant string fields from the class
        FieldInfo[] fields = tablesType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        List<string> tableNames = [];

        foreach (FieldInfo field in fields)
        {
            if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
            {
                string? value = field.GetRawConstantValue()?.ToString();

                if (!string.IsNullOrWhiteSpace(value) &&
                    (exclude is null || !exclude.Contains(value, StringComparer.OrdinalIgnoreCase)))
                {
                    tableNames.Add(value);
                }
            }
        }

        return [.. tableNames];
    }
}
