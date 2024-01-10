using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data
{
    public class Group : Element
    {
        public List<Element> Elements { get; set; } = new List<Element>();

        public Group(long id, string name)
            : base("group", name, id)
        {

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
    }
}
