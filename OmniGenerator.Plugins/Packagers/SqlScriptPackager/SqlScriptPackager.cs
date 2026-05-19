using Microsoft.ProgramSynthesis.Extraction.Text.Build.NodeTypes;
using Microsoft.ProgramSynthesis.Utils;
using Microsoft.ProgramSynthesis.Utils.Interactive;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Plugins.Packagers.SqlScriptPackager
{
    [OmniGeneratorPluginMetadata("packager.omni.sql", "A packager a builds SQL scripts from groups acting as tables and documents acting as table rows")]
    public class SqlScriptPackager : OmniGeneratorPluginBase, IPackager
    {
        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            var packagename = $"{DateTime.Now:yyyyMMddHHmmss}_Script.sql";

            if (!Directory.Exists(basepath))
                Directory.CreateDirectory(basepath);

            var filefullpath = Path.Combine(basepath, packagename);

            await using (var fs = new FileStream(filefullpath, FileMode.Create, FileAccess.ReadWrite))
            await using (var sw = new StreamWriter(fs))
            {
                var deletes = GetDeletes(root);
                foreach (var delete in deletes)
                    await sw.WriteLineAsync(delete);

                foreach (var group in root.Groups)
                {
                    var inserts = GetInserts(group);
                    foreach (var insert in inserts)
                        await sw.WriteLineAsync(insert);
                }
            }
        }

        private IEnumerable<string> GetInserts(Group group)
        {
            foreach (var table in group.GetDocuments(null).GroupBy(d => d.Name))
            {
                var firstRow = table.First();
                yield return $"INSERT {table.Key} ({string.Join(",", firstRow.Fields.Keys)})";
                yield return $"SELECT {GetValues(firstRow.Fields)}";

                foreach (var row in table.Skip(1))
                    yield return $"UNION SELECT {GetValues(row.Fields)}";

                yield return string.Empty;
            }
        }

        private IEnumerable<string> GetDeletes(Root root)
        {
            var fields = new RootFields(root.Fields);
            var truncateTableBefore = Convert.ToBoolean(fields.TruncateBeforeInsert.Value);

            if (truncateTableBefore)
            {
                var tables = root
                    .GetAllDocuments()
                    .DistinctBy(d => d.Name);

                foreach (var table in tables)
                    yield return $"TRUNCATE TABLE {table.Name};";

                yield return string.Empty;
            }
        }

        private string GetValues(IReadOnlyDictionary<string, Field> fields)
        {
            return string.Join(",", fields
                .Keys
                .Select(fn => fields[fn].Value switch
                {
                    DateTime dt => $"'{dt:yyyyMMdd HH:mm:ss}'",
                    string s    => $"'{s.Replace("'", "''")}'",
                    _           => fields[fn].Value
                }));
        }
    }
}
