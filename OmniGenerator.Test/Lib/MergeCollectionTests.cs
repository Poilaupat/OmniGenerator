using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Test.Lib
{
    [TestFixture]
    internal class MergeCollectionTests
    {
        private class Dummy
        {
            public string Name { get; }
            public int Value { get; }

            public Dummy(string name, int value)
            {
                Name = name;
                Value = value;
            }

            public override bool Equals(object? obj)
            {
                if (obj is not null && obj is Dummy dummy)
                    return Name == dummy.Name;

                return false;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

        }

        [Test]
        public void MergeCollectionNoCollision()
        {
            var collection1 = new Dummy[]
            {
                new Dummy("obj1", 1),
                new Dummy("obj2", 2),
                new Dummy("obj3", 3),
                new Dummy("obj4", 4),
            }.ToList();

            var collection2 = new Dummy[]
            {
                new Dummy("obj5", 5),
                new Dummy("obj6", 6),
            };

            collection1.Merge(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(6));
        }

        [Test]
        public void MergeCollectionWithCollision()
        {
            var collection1 = new Dummy[]
            {
                new Dummy("obj1", 1),
                new Dummy("obj2", 2),
                new Dummy("obj3", 3),
                new Dummy("obj4", 4),
            }.ToList();

            var collection2 = new Dummy[]
            {
                new Dummy("obj5", 5),
                new Dummy("obj2", 20),
            };

            collection1.Merge(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(5));
            Assert.That(collection1.Single(x => x.Name == "obj2").Value, Is.EqualTo(2));
        }

        [Test]
        public void MergeCollectionWithReplaceNoCollision()
        {
            var collection1 = new Dummy[]
            {
                new Dummy("obj1", 1),
                new Dummy("obj2", 2),
                new Dummy("obj3", 3),
                new Dummy("obj4", 4),
            }.ToList();

            var collection2 = new Dummy[]
            {
                new Dummy("obj5", 5),
                new Dummy("obj6", 6),
            };

            collection1.MergeWithReplace(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(6));
        }

        [Test]
        public void MergeCollectionWithReplaceWithCollision()
        {
            var collection1 = new Dummy[]
            {
                new Dummy("obj1", 1),
                new Dummy("obj2", 2),
                new Dummy("obj3", 3),
                new Dummy("obj4", 4),
            }.ToList();

            var collection2 = new Dummy[]
            {
                new Dummy("obj5", 5),
                new Dummy("obj2", 20),
            };

            collection1.MergeWithReplace(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(5));
            Assert.That(collection1.Single(x => x.Name == "obj2").Value, Is.EqualTo(20));
        }


        [Test]
        public void MergeDictionaryNoCollision()
        {
            var collection1 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj1", 1), "un" },
                { new Dummy("obj2", 2), "deux" },
                { new Dummy("obj3", 3), "trois" },
                { new Dummy("obj4", 4), "quatre" },
            };

            var collection2 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj5", 1), "cinq" },
                { new Dummy("obj6", 2), "six" },
            };

            collection1.Merge(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(6));
        }

        [Test]
        public void MergeDictionaryWithCollision()
        {
            var collection1 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj1", 1), "un" },
                { new Dummy("obj2", 2), "deux" },
                { new Dummy("obj3", 3), "trois" },
                { new Dummy("obj4", 4), "quatre" },
            };

            var collection2 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj5", 1), "cinq" },
                { new Dummy("obj2", 20), "six" },
            };

            collection1.Merge(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(5));
            Assert.That(collection1.Single(x => x.Key.Equals(new Dummy("obj2", -1))).Value, Is.EqualTo("deux"));
        }

        [Test]
        public void MergeDictionaryWithReplaceNoCollision()
        {
            var collection1 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj1", 1), "un" },
                { new Dummy("obj2", 2), "deux" },
                { new Dummy("obj3", 3), "trois" },
                { new Dummy("obj4", 4), "quatre" },
            };

            var collection2 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj5", 1), "cinq" },
                { new Dummy("obj6", 2), "six" },
            };

            collection1.MergeWithReplace(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(6));
        }

        [Test]
        public void MergeDictionaryWithReplaceWithCollision()
        {
            var collection1 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj1", 1), "un" },
                { new Dummy("obj2", 2), "deux" },
                { new Dummy("obj3", 3), "trois" },
                { new Dummy("obj4", 4), "quatre" },
            };

            var collection2 = new Dictionary<Dummy, string>
            {
                { new Dummy("obj5", 1), "cinq" },
                { new Dummy("obj2", 20), "six" },
            };

            collection1.MergeWithReplace(collection2);

            Assert.That(collection1.Count(), Is.EqualTo(5));
            Assert.That(collection1.Single(x => x.Key.Equals(new Dummy("obj2", -1))).Value, Is.EqualTo("six"));
        }


    }
}
