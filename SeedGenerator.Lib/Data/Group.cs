namespace SeedGenerator.Lib.Data
{
    public class Group : Element
    {
        public List<Element> Elements { get; set; } = new List<Element>();

        public Group(long id, string name)
            : base("group", name, id)
        {

        }

        public IEnumerable<Group> GetGroups(bool recursive = false)
        {
            var groups = Elements.Where(x => x is Group).Cast<Group>();
            foreach (var group in groups)
            {
                if(recursive)
                {
                    foreach(var subGroup in group.GetGroups(recursive))
                    {
                        yield return subGroup;
                    }
                }

                yield return group;
            }
        }

        public IEnumerable<Group> GetGroups(string name, bool recursive = false)
        {
            return GetGroups(recursive)
                .Where(x => x.Name == name);
        }

        public IEnumerable<Document> GetDocuments(bool recusive = false)
        {
            foreach(var element in Elements)
            {
                switch (element)
                {
                    case Document document:
                        yield return document;
                        break;

                    case Group group:
                        if(recusive) 
                        {
                            foreach(var document in group.GetDocuments(recusive))
                            {
                                yield return document;
                            }
                        }
                        break;

                    default:
                        throw new Exception($"{element.GetType().Name} was an unexpected type");
                }
            }
        }

        public IEnumerable<Document> GetDocuments(string name, bool recusive = false)
        {
            return GetDocuments(recusive)
                .Where(document => document.Name == name);
        }
    }
}
