using NUnit.Framework;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Generators;
using System;
using System.IO;
using System.Linq;

namespace OmniGenerator.Test.Lib.Configuration
{
    /// <summary>
    /// Tests for JSON configuration deserialization.
    /// Verifies that various field configuration types can be correctly deserialized from JSON.
    /// </summary>
    [TestFixture]
    public class ConfigurationDeserializationTests
    {
        private string _testDataDirectory = null!;

        [SetUp]
        public void SetUp()
        {
            _testDataDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Configuration");
            Directory.CreateDirectory(_testDataDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDataDirectory))
            {
                Directory.Delete(_testDataDirectory, true);
            }
        }

        #region Basic Configuration Tests

        [Test]
        public async Task ReadConfigurationAsync_ValidMinimalConfiguration_Succeeds()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""render-resolution"": 300,
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "minimal-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            Assert.That(config, Is.Not.Null);
            Assert.That(config.PackagerName, Is.EqualTo("pdf"));
            Assert.That(config.RenderResolutionDPI, Is.EqualTo(300));
            Assert.That(config.Hierarchy, Is.Not.Null);
            Assert.That(config.Hierarchy.Root, Is.Not.Null);
            Assert.That(config.Hierarchy.Root.Name, Is.EqualTo("TestDoc"));
        }

        [Test]
        public void ReadConfigurationAsync_InvalidJson_ThrowsConfigurationException()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document""
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1
    }
  }
}"; // Missing comma after "document"
            var configPath = Path.Combine(_testDataDirectory, "invalid-config.json");
            File.WriteAllText(configPath, configJson);

            // Act & Assert
            Assert.ThrowsAsync<ConfigurationException>(async () =>
                await ConfigurationReader.ReadConfigurationAsync(configPath));
        }

        [Test]
        public void ReadConfigurationAsync_MissingFile_ThrowsConfigurationException()
        {
            // Arrange
            var configPath = Path.Combine(_testDataDirectory, "non-existent-config.json");

            // Act & Assert
            Assert.ThrowsAsync<ConfigurationException>(async () =>
                await ConfigurationReader.ReadConfigurationAsync(configPath));
        }

        #endregion

        #region Field Configuration - Basic Types

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationRegex_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""regex"",
          ""name"": ""DocumentID"",
          ""pattern"": ""[A-Z]{3}\\d{6}""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "regex-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            Assert.That(config.Hierarchy.Root.Fields, Is.Not.Null);
            Assert.That(config.Hierarchy.Root.Fields.Count, Is.EqualTo(1));
            
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationRegex>());
            
            var regexField = field as FieldConfigurationRegex;
            Assert.That(regexField!.Name, Is.EqualTo("DocumentID"));
            Assert.That(regexField.Pattern, Is.EqualTo(@"[A-Z]{3}\d{6}"));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationConstant_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""constant"",
          ""name"": ""Status"",
          ""value"": ""ACTIVE""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "constant-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationConstant>());
            
            var constantField = field as FieldConfigurationConstant;
            Assert.That(constantField!.Name, Is.EqualTo("Status"));
            Assert.That(constantField.Constant, Is.EqualTo("ACTIVE"));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationNumeric_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""numeric"",
          ""name"": ""Amount"",
          ""min"": 100,
          ""max"": 1000
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "numeric-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationNumeric>());
            
            var numericField = field as FieldConfigurationNumeric;
            Assert.That(numericField!.Name, Is.EqualTo("Amount"));
            Assert.That(numericField.Min, Is.EqualTo(100));
            Assert.That(numericField.Max, Is.EqualTo(1000));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationDate_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""date"",
          ""name"": ""CreatedDate"",
          ""day-diff-min"": -365,
          ""day-diff-max"": 0
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "date-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationDate>());
            
            var dateField = field as FieldConfigurationDate;
            Assert.That(dateField!.Name, Is.EqualTo("CreatedDate"));
            Assert.That(dateField.DayDiffMin, Is.EqualTo(-365));
            Assert.That(dateField.DayDiffMax, Is.EqualTo(0));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationIncrement_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""increment"",
          ""name"": ""Sequence"",
          ""start"": 1000,
          ""increment"": 10
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "increment-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationIncrement>());
            
            var incrementField = field as FieldConfigurationIncrement;
            Assert.That(incrementField!.Name, Is.EqualTo("Sequence"));
            Assert.That(incrementField.Start, Is.EqualTo(1000));
            Assert.That(incrementField.Increment, Is.EqualTo(10));
        }

        #endregion

        #region Field Configuration - Collection Types

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationWeightedList_ObjectFormat_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""list"",
          ""name"": ""ProductType"",
          ""list"": {
            ""Premium"": 0.2,
            ""Standard"": 0.5,
            ""Basic"": 0.3
          }
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "weighted-list-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationWeightedList>());
            
            var weightedListField = field as FieldConfigurationWeightedList;
            Assert.That(weightedListField!.Name, Is.EqualTo("ProductType"));
            Assert.That(weightedListField.List, Is.Not.Null);
            Assert.That(weightedListField.List!.Count(), Is.EqualTo(3));
            
            var items = weightedListField.List!.ToList();
            Assert.That(items.Any(i => i.Value == "Premium" && i.Weight == 0.2), Is.True);
            Assert.That(items.Any(i => i.Value == "Standard" && i.Weight == 0.5), Is.True);
            Assert.That(items.Any(i => i.Value == "Basic" && i.Weight == 0.3), Is.True);
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationWeightedList_ArrayFormat_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""list"",
          ""name"": ""Color"",
          ""list"": [""Red"", ""Green"", ""Blue""]
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "list-array-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationWeightedList>());
            
            var listField = field as FieldConfigurationWeightedList;
            Assert.That(listField!.Name, Is.EqualTo("Color"));
            Assert.That(listField.List, Is.Not.Null);
            Assert.That(listField.List!.Count(), Is.EqualTo(3));
            
            var items = listField.List!.ToList();
            Assert.That(items.All(i => i.Weight == 1.0), Is.True);
            Assert.That(items.Any(i => i.Value == "Red"), Is.True);
            Assert.That(items.Any(i => i.Value == "Green"), Is.True);
            Assert.That(items.Any(i => i.Value == "Blue"), Is.True);
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationWeightedListFromFile_Deserializes()
        {
            // Arrange
            var listFileContent = @"Premium,0.2
Standard,0.5
Basic,0.3";
            var listFilePath = Path.Combine(_testDataDirectory, "products.txt");
            await File.WriteAllTextAsync(listFilePath, listFileContent);

            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""list"",
          ""name"": ""ProductType"",
          ""list-file"": ""products.txt""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "weighted-list-file-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var field = config.Hierarchy.Root.Fields.First();
            Assert.That(field, Is.InstanceOf<FieldConfigurationWeightedList>());
            
            var weightedListField = field as FieldConfigurationWeightedList;
            Assert.That(weightedListField!.Name, Is.EqualTo("ProductType"));
            Assert.That(weightedListField.ListFilePath, Is.EqualTo("products.txt"));
        }

        #endregion

        #region Field Configuration - Dependent Types

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationComposite_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""constant"",
          ""name"": ""FirstName"",
          ""value"": ""John""
        },
        {
          ""$type"": ""constant"",
          ""name"": ""LastName"",
          ""value"": ""Doe""
        },
        {
          ""$type"": ""composite"",
          ""name"": ""FullName"",
          ""format"": ""{FirstName} {LastName}"",
          ""dependent-upon"": ""FirstName""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "composite-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var compositeField = config.Hierarchy.Root.Fields.FirstOrDefault(f => f.Name == "FullName");
            Assert.That(compositeField, Is.Not.Null);
            Assert.That(compositeField, Is.InstanceOf<FieldConfigurationComposite>());
            
            var composite = compositeField as FieldConfigurationComposite;
            Assert.That(composite!.Format, Is.EqualTo("{FirstName} {LastName}"));
            Assert.That(composite.DependentUpon, Is.EqualTo("FirstName"));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationKeyCalculator_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""constant"",
          ""name"": ""ReferenceField"",
          ""value"": ""ABC123""
        },
        {
          ""$type"": ""key"",
          ""name"": ""CalculatedKey"",
          ""key-type"": ""Rlmc"",
          ""dependent-upon"": ""ReferenceField""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "key-calculator-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var keyCalcField = config.Hierarchy.Root.Fields.FirstOrDefault(f => f.Name == "CalculatedKey");
            Assert.That(keyCalcField, Is.Not.Null);
            Assert.That(keyCalcField, Is.InstanceOf<FieldConfigurationKeyCalculator>());
            
            var keyCalc = keyCalcField as FieldConfigurationKeyCalculator;
            Assert.That(keyCalc!.KeyType, Is.EqualTo(EKeyType.Rlmc));
            Assert.That(keyCalc.DependentUpon, Is.EqualTo("ReferenceField"));
        }

        [Test]
        public async Task ReadConfigurationAsync_WithFieldConfigurationAggregate_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""numeric"",
          ""name"": ""ItemAmount"",
          ""min"": 10,
          ""max"": 100
        },
        {
          ""$type"": ""aggregate"",
          ""name"": ""TotalAmount"",
          ""aggregate-type"": ""Sum"",
          ""scope"": ""DirectChildren"",
          ""target-element"": ""Item"",
          ""dependent-upon"": ""ItemAmount""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "aggregate-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            var aggregateField = config.Hierarchy.Root.Fields.FirstOrDefault(f => f.Name == "TotalAmount");
            Assert.That(aggregateField, Is.Not.Null);
            Assert.That(aggregateField, Is.InstanceOf<FieldConfigurationAggregate>());
            
            var aggregate = aggregateField as FieldConfigurationAggregate;
            Assert.That(aggregate!.AggregateType, Is.EqualTo(EFFieldAggregateType.Sum));
            Assert.That(aggregate.Scope, Is.EqualTo(EScope.DirectChildren));
            Assert.That(aggregate.TargetElement, Is.EqualTo("Item"));
            Assert.That(aggregate.DependentUpon, Is.EqualTo("ItemAmount"));
        }

        #endregion

        #region Multiple Fields and Complex Scenarios

        [Test]
        public async Task ReadConfigurationAsync_WithMultipleFieldTypes_Deserializes()
        {
            // Arrange
            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 5,
      ""fields"": [
        {
          ""$type"": ""increment"",
          ""name"": ""ID"",
          ""start"": 1,
          ""increment"": 1
        },
        {
          ""$type"": ""regex"",
          ""name"": ""Code"",
          ""pattern"": ""[A-Z]{2}\\d{4}""
        },
        {
          ""$type"": ""constant"",
          ""name"": ""Type"",
          ""value"": ""Invoice""
        },
        {
          ""$type"": ""numeric"",
          ""name"": ""Amount"",
          ""min"": 10,
          ""max"": 1000
        },
        {
          ""$type"": ""date"",
          ""name"": ""Date"",
          ""day-diff-min"": -30,
          ""day-diff-max"": 0
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "multiple-fields-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            Assert.That(config.Hierarchy.Root.Fields, Is.Not.Null);
            Assert.That(config.Hierarchy.Root.Fields.Count, Is.EqualTo(5));
            
            Assert.That(config.Hierarchy.Root.Fields.ElementAt(0), Is.InstanceOf<FieldConfigurationIncrement>());
            Assert.That(config.Hierarchy.Root.Fields.ElementAt(1), Is.InstanceOf<FieldConfigurationRegex>());
            Assert.That(config.Hierarchy.Root.Fields.ElementAt(2), Is.InstanceOf<FieldConfigurationConstant>());
            Assert.That(config.Hierarchy.Root.Fields.ElementAt(3), Is.InstanceOf<FieldConfigurationNumeric>());
            Assert.That(config.Hierarchy.Root.Fields.ElementAt(4), Is.InstanceOf<FieldConfigurationDate>());
        }

        [Test]
        public async Task ReadConfigurationAsync_WithExternalFieldConfigurationFile_MergesFields()
        {
            // Arrange
            var fieldsJson = @"[
  {
    ""$type"": ""constant"",
    ""name"": ""ExternalField1"",
    ""value"": ""Value1""
  },
  {
    ""$type"": ""constant"",
    ""name"": ""ExternalField2"",
    ""value"": ""Value2""
  }
]";
            var fieldsPath = Path.Combine(_testDataDirectory, "external-fields.json");
            await File.WriteAllTextAsync(fieldsPath, fieldsJson);

            var configJson = @"{
  ""packager"": ""pdf"",
  ""hierarchy"": {
    ""field-configuration-file"": ""external-fields.json"",
    ""root"": {
      ""$type"": ""document"",
      ""name"": ""TestDoc"",
      ""min-occurs"": 1,
      ""max-occurs"": 1,
      ""fields"": [
        {
          ""$type"": ""constant"",
          ""name"": ""InternalField"",
          ""value"": ""InternalValue""
        }
      ]
    }
  }
}";
            var configPath = Path.Combine(_testDataDirectory, "external-fields-config.json");
            await File.WriteAllTextAsync(configPath, configJson);

            // Act
            var config = await ConfigurationReader.ReadConfigurationAsync(configPath);

            // Assert
            // External fields are merged into hierarchy.Fields
            Assert.That(config.Hierarchy.Fields, Is.Not.Null);
            Assert.That(config.Hierarchy.Fields.Count, Is.EqualTo(2));
            
            var hierarchyFieldNames = config.Hierarchy.Fields.Select(f => f.Name).ToList();
            Assert.That(hierarchyFieldNames, Contains.Item("ExternalField1"));
            Assert.That(hierarchyFieldNames, Contains.Item("ExternalField2"));
            
            // Internal field is in root.Fields
            Assert.That(config.Hierarchy.Root.Fields, Is.Not.Null);
            Assert.That(config.Hierarchy.Root.Fields.Count, Is.EqualTo(1));
            Assert.That(config.Hierarchy.Root.Fields.First().Name, Is.EqualTo("InternalField"));
        }

        #endregion
    }
}
