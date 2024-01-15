namespace SeedGenerator.Lib.Data
{
    public class Root
    {
        public FieldCollection Fields { get; set; } = new FieldCollection();
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();

        public Root(IEnumerable<Group> groups)
        {
            Groups = groups;
        }

        public IEnumerable<Document> GetDocuments(bool recursive = false)
        {
            foreach (var document in Groups.SelectMany(x => x.GetDocuments(true)))
            {
                yield return document;
            }
        }

        public void GenerateAggregateFields()
        {

        }
    }
}
