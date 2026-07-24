using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Mapping;
using OmniGenerator.Lib.Hierarchy;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.Lib.Generators
{
    [TestFixture]
    public class FieldGeneratorContainerTests
    {
        private FieldMapper _mapper = null!;

        [SetUp]
        public void SetUp()
        {
            _mapper = new FieldMapper();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithMinimalConfiguration_InitializesSuccessfully()
        {
            var config = CreateMinimalConfiguration();

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithRootFields_InitializesRootGenerators()
        {
            var config = CreateMinimalConfiguration();
            config.Fields.Add(new FieldConfigurationConstant
            {
                Name = "RootField",
                Constant = "RootValue"
            });

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.RootHasFields(), Is.True);
        }

        [Test]
        public void Constructor_WithDocumentFields_InitializesDocumentGenerators()
        {
            var config = CreateConfigurationWithDocument();

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("TestDocument"), Is.True);
        }

        [Test]
        public void Constructor_WithGroupFields_InitializesGroupGenerators()
        {
            var config = CreateConfigurationWithGroup();

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("TestGroup"), Is.True);
        }

        [Test]
        public void Constructor_WithNestedGroups_InitializesAllGroupGenerators()
        {
            var config = CreateConfigurationWithNestedGroups();

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("ParentGroup"), Is.True);
            Assert.That(container.ElementHasFields("ChildGroup"), Is.True);
        }

        [Test]
        public void Constructor_WithMultipleDocuments_InitializesAllDocumentGenerators()
        {
            var config = CreateConfigurationWithMultipleDocuments();

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("Document1"), Is.True);
            Assert.That(container.ElementHasFields("Document2"), Is.True);
        }

        #endregion

        #region RootHasFields Tests

        [Test]
        public void RootHasFields_Always_ReturnsTrue()
        {
            var config = CreateMinimalConfiguration();

            var container = new FieldGeneratorContainer(config, _mapper);

            // Root is always created even with empty fields
            Assert.That(container.RootHasFields(), Is.True);
        }

        [Test]
        public void RootHasFields_WithRootFields_ReturnsTrue()
        {
            var config = CreateMinimalConfiguration();
            config.Fields.Add(new FieldConfigurationConstant
            {
                Name = "RootField",
                Constant = "Value"
            });

            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.RootHasFields(), Is.True);
        }

        #endregion

        #region ElementHasFields Tests

        [Test]
        public void ElementHasFields_WithNonExistentElement_ReturnsFalse()
        {
            var config = CreateMinimalConfiguration();
            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("NonExistent"), Is.False);
        }

        [Test]
        public void ElementHasFields_WithExistingElementWithFields_ReturnsTrue()
        {
            var config = CreateConfigurationWithDocument();
            var container = new FieldGeneratorContainer(config, _mapper);

            Assert.That(container.ElementHasFields("TestDocument"), Is.True);
        }

        [Test]
        public void ElementHasFields_WithRootElement_ReturnsTrue()
        {
            var config = CreateMinimalConfiguration();
            var container = new FieldGeneratorContainer(config, _mapper);

            // Root element always exists
            Assert.That(container.ElementHasFields(HierarchyConfiguration.Name), Is.True);
        }

        #endregion

        #region GenerateRegularFields Tests

        [Test]
        public void GenerateRegularFields_WithConstantField_GeneratesCorrectValue()
        {
            var config = CreateConfigurationWithDocument();
            var container = new FieldGeneratorContainer(config, _mapper);

            var fields = container.GenerateRegularFields("TestDocument");

            Assert.That(fields, Has.Count.EqualTo(1));
            Assert.That(fields.ContainsKey("DocField"), Is.True);
            Assert.That(fields["DocField"].StringValue, Is.EqualTo("DocValue"));
        }

        [Test]
        public void GenerateRegularFields_WithMultipleFields_GeneratesAllFields()
        {
            var config = CreateMinimalConfiguration();
            var docConfig = new DocumentConfiguration
            {
                Name = "TestDoc",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            docConfig.Fields.Add(new FieldConfigurationConstant { Name = "Field1", Constant = "Value1" });
            docConfig.Fields.Add(new FieldConfigurationConstant { Name = "Field2", Constant = "Value2" });
            docConfig.Fields.Add(new FieldConfigurationNumeric { Name = "Field3", Min = 10, Max = 20 });
            config.Root.Elements.Add(docConfig);

            var container = new FieldGeneratorContainer(config, _mapper);
            var fields = container.GenerateRegularFields("TestDoc");

            Assert.That(fields, Has.Count.EqualTo(3));
            Assert.That(fields.ContainsKey("Field1"), Is.True);
            Assert.That(fields.ContainsKey("Field2"), Is.True);
            Assert.That(fields.ContainsKey("Field3"), Is.True);
        }

        [Test]
        public void GenerateRegularFields_CalledMultipleTimes_GeneratesNewValues()
        {
            var config = CreateMinimalConfiguration();
            var docConfig = new DocumentConfiguration
            {
                Name = "TestDoc",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            docConfig.Fields.Add(new FieldConfigurationIncrement { Name = "Counter", Start = 0, Increment = 1 });
            config.Root.Elements.Add(docConfig);

            var container = new FieldGeneratorContainer(config, _mapper);

            var fields1 = container.GenerateRegularFields("TestDoc");
            var fields2 = container.GenerateRegularFields("TestDoc");
            var fields3 = container.GenerateRegularFields("TestDoc");

            Assert.That(fields1["Counter"].Value.Convert<int>(), Is.EqualTo(0));
            Assert.That(fields2["Counter"].Value.Convert<int>(), Is.EqualTo(1));
            Assert.That(fields3["Counter"].Value.Convert<int>(), Is.EqualTo(2));
        }

        #endregion

        #region GenerateRootFields Tests

        [Test]
        public void GenerateRootFields_WithNoRootFields_ReturnsEmptyDictionary()
        {
            var config = CreateMinimalConfiguration();
            var container = new FieldGeneratorContainer(config, _mapper);

            var fields = container.GenerateRootFields();

            Assert.That(fields, Is.Empty);
        }

        [Test]
        public void GenerateRootFields_WithRootFields_GeneratesFields()
        {
            var config = CreateMinimalConfiguration();
            config.Fields.Add(new FieldConfigurationConstant { Name = "RootField1", Constant = "RootValue1" });
            config.Fields.Add(new FieldConfigurationConstant { Name = "RootField2", Constant = "RootValue2" });

            var container = new FieldGeneratorContainer(config, _mapper);
            var fields = container.GenerateRootFields();

            Assert.That(fields, Has.Count.EqualTo(2));
            Assert.That(fields["RootField1"].StringValue, Is.EqualTo("RootValue1"));
            Assert.That(fields["RootField2"].StringValue, Is.EqualTo("RootValue2"));
        }

        [Test]
        public void GenerateRootFields_CalledMultipleTimes_GeneratesNewValues()
        {
            var config = CreateMinimalConfiguration();
            config.Fields.Add(new FieldConfigurationIncrement { Name = "RootCounter", Start = 100, Increment = 5 });

            var container = new FieldGeneratorContainer(config, _mapper);

            var fields1 = container.GenerateRootFields();
            var fields2 = container.GenerateRootFields();
            var fields3 = container.GenerateRootFields();

            Assert.That(fields1["RootCounter"].Value.Convert<int>(), Is.EqualTo(100));
            Assert.That(fields2["RootCounter"].Value.Convert<int>(), Is.EqualTo(105));
            Assert.That(fields3["RootCounter"].Value.Convert<int>(), Is.EqualTo(110));
        }

        #endregion

        #region Integration Tests

        [Test]
        public void Container_WithComplexHierarchy_InitializesAllGenerators()
        {
            var config = CreateComplexConfiguration();
            var container = new FieldGeneratorContainer(config, _mapper);

            // Root has fields
            Assert.That(container.RootHasFields(), Is.True);

            // Documents have fields
            Assert.That(container.ElementHasFields("Document1"), Is.True);
            Assert.That(container.ElementHasFields("Document2"), Is.True);

            // Groups have fields
            Assert.That(container.ElementHasFields("ParentGroup"), Is.True);
            Assert.That(container.ElementHasFields("ChildGroup"), Is.True);
        }

        [Test]
        public void Container_GeneratesFieldsForAllElements()
        {
            var config = CreateComplexConfiguration();
            var container = new FieldGeneratorContainer(config, _mapper);

            // Generate fields for all elements
            var rootFields = container.GenerateRootFields();
            var doc1Fields = container.GenerateRegularFields("Document1");
            var doc2Fields = container.GenerateRegularFields("Document2");
            var parentGroupFields = container.GenerateRegularFields("ParentGroup");
            var childGroupFields = container.GenerateRegularFields("ChildGroup");

            // Verify all generated
            Assert.That(rootFields, Is.Not.Empty);
            Assert.That(doc1Fields, Is.Not.Empty);
            Assert.That(doc2Fields, Is.Not.Empty);
            Assert.That(parentGroupFields, Is.Not.Empty);
            Assert.That(childGroupFields, Is.Not.Empty);
        }

        [Test]
        public void Container_WithDependentFields_ResolvesCorrectly()
        {
            var config = CreateMinimalConfiguration();
            var docConfig = new DocumentConfiguration
            {
                Name = "TestDoc",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            docConfig.Fields.Add(new FieldConfigurationConstant { Name = "Source", Constant = "TestValue" });
            docConfig.Fields.Add(new FieldConfigurationKeyCalculator { Name = "Hash", DependentUpon = "Source", KeyType = EKeyType.Dummy });
            config.Root.Elements.Add(docConfig);

            var container = new FieldGeneratorContainer(config, _mapper);
            var fields = container.GenerateRegularFields("TestDoc");

            Assert.That(fields, Has.Count.EqualTo(2));
            Assert.That(fields["Source"].StringValue, Is.EqualTo("TestValue"));
            Assert.That(fields["Hash"].Value.RawValue, Is.Not.Null);
            Assert.That(fields["Hash"].StringValue, Is.Not.Empty);
        }

        #endregion

        #region Helper Methods

        private HierarchyConfiguration CreateMinimalConfiguration()
        {
            return new HierarchyConfiguration
            {
                Root = new GroupConfiguration
                {
                    Name = "Root",
                    MinOccurs = 1,
                    MaxOccurs = 1
                }
            };
        }

        private HierarchyConfiguration CreateConfigurationWithDocument()
        {
            var config = CreateMinimalConfiguration();
            var docConfig = new DocumentConfiguration
            {
                Name = "TestDocument",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            docConfig.Fields.Add(new FieldConfigurationConstant
            {
                Name = "DocField",
                Constant = "DocValue"
            });
            config.Root.Elements.Add(docConfig);
            return config;
        }

        private HierarchyConfiguration CreateConfigurationWithGroup()
        {
            var config = CreateMinimalConfiguration();
            var groupConfig = new GroupConfiguration
            {
                Name = "TestGroup",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            groupConfig.Fields.Add(new FieldConfigurationConstant
            {
                Name = "GroupField",
                Constant = "GroupValue"
            });
            config.Root.Elements.Add(groupConfig);
            return config;
        }

        private HierarchyConfiguration CreateConfigurationWithNestedGroups()
        {
            var config = CreateMinimalConfiguration();
            var parentGroup = new GroupConfiguration
            {
                Name = "ParentGroup",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            parentGroup.Fields.Add(new FieldConfigurationConstant { Name = "ParentField", Constant = "ParentValue" });

            var childGroup = new GroupConfiguration
            {
                Name = "ChildGroup",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            childGroup.Fields.Add(new FieldConfigurationConstant { Name = "ChildField", Constant = "ChildValue" });

            parentGroup.Elements.Add(childGroup);
            config.Root.Elements.Add(parentGroup);
            return config;
        }

        private HierarchyConfiguration CreateConfigurationWithMultipleDocuments()
        {
            var config = CreateMinimalConfiguration();

            var doc1 = new DocumentConfiguration
            {
                Name = "Document1",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            doc1.Fields.Add(new FieldConfigurationConstant { Name = "Doc1Field", Constant = "Doc1Value" });

            var doc2 = new DocumentConfiguration
            {
                Name = "Document2",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            doc2.Fields.Add(new FieldConfigurationConstant { Name = "Doc2Field", Constant = "Doc2Value" });

            config.Root.Elements.Add(doc1);
            config.Root.Elements.Add(doc2);
            return config;
        }

        private HierarchyConfiguration CreateComplexConfiguration()
        {
            var config = CreateMinimalConfiguration();

            // Root fields
            config.Fields.Add(new FieldConfigurationConstant { Name = "RootField", Constant = "RootValue" });

            // Parent group with fields
            var parentGroup = new GroupConfiguration
            {
                Name = "ParentGroup",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            parentGroup.Fields.Add(new FieldConfigurationConstant { Name = "ParentField", Constant = "ParentValue" });

            // Child group with fields
            var childGroup = new GroupConfiguration
            {
                Name = "ChildGroup",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            childGroup.Fields.Add(new FieldConfigurationConstant { Name = "ChildField", Constant = "ChildValue" });

            // Documents with fields
            var doc1 = new DocumentConfiguration
            {
                Name = "Document1",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            doc1.Fields.Add(new FieldConfigurationConstant { Name = "Doc1Field", Constant = "Doc1Value" });

            var doc2 = new DocumentConfiguration
            {
                Name = "Document2",
                MinOccurs = 1,
                MaxOccurs = 1
            };
            doc2.Fields.Add(new FieldConfigurationIncrement { Name = "Doc2Counter", Start = 0, Increment = 1 });

            // Build hierarchy
            parentGroup.Elements.Add(childGroup);
            parentGroup.Elements.Add(doc1);
            config.Root.Elements.Add(parentGroup);
            config.Root.Elements.Add(doc2);

            return config;
        }

        #endregion
    }
}
