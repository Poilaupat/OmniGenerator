using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.ErrorSimulation;
using OmniGenerator.Lib.ErrorSimulation.Mutators;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Test.Lib.ErrorSimulation
{
    [TestFixture]
    public class ErrorSimulatorTests
    {
        private static ErrorSimulator CreateSimulator() =>
            new(
                new IFieldErrorMutator[]
                {
                    new MisreadMutator(),
                    new SubstitutionMutator(),
                    new InconsistencyMutator()
                },
                NullLogger<ErrorSimulator>.Instance);

        private static Root CreateRootWithDocument(string documentName, params Field[] fields)
        {
            var fieldMap = fields.ToDictionary(f => f.Name);
            var document = new Document(documentName, imageComposer: null, fieldMap);
            var group = new Group("root", Array.Empty<Group>(), new[] { document });
            return new Root(new[] { group });
        }

        [Test]
        public void Apply_ShouldDoNothing_WhenConfigurationDisabled()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = false,
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 1.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.DataStringValue, Is.EqualTo("123456"));
        }

        [Test]
        public void Apply_ShouldMutateDataChannel_WhenMisreadRuleAlwaysTriggers()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = true,
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 1.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.Value, Is.EqualTo("123456"));
            Assert.That(field.DataStringValue, Does.Contain("?"));
        }

        [Test]
        public void Apply_ShouldNotMutate_WhenProbabilityIsZero()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = true,
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 0.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.DataStringValue, Is.EqualTo("123456"));
        }

        [Test]
        public void Apply_ShouldSkipRule_WhenErrorTypeIsDisabled()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = true,
                DisabledErrors = { EErrorSimulationType.Misread },
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 1.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.DataStringValue, Is.EqualTo("123456"));
        }

        [Test]
        public void Apply_ShouldApplyOnlyFirstRule_WhenMultipleRulesTargetSameField()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = true,
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 1.0
                    },
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Cheque",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Inconsistency,
                        Probability = 1.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.DataStringValue, Does.Contain("?"), "First (Misread) rule should apply to the data channel.");
            Assert.That(field.ImageStringValue, Is.EqualTo("123456"), "Second (Inconsistency) rule must be skipped.");
        }

        [Test]
        public void Apply_ShouldNotMutate_WhenTargetDocumentDoesNotMatch()
        {
            // Arrange
            var root = CreateRootWithDocument("Cheque", new Field("Amount", "123456"));
            var configuration = new ErrorSimulationConfiguration
            {
                Enabled = true,
                Rules =
                {
                    new ErrorSimulationRule
                    {
                        TargetDocument = "Invoice",
                        TargetField = "Amount",
                        Type = EErrorSimulationType.Misread,
                        Probability = 1.0
                    }
                }
            };

            // Act
            CreateSimulator().Apply(root, configuration);

            // Assert
            var field = root.GetAllDocuments().Single().Fields["Amount"];
            Assert.That(field.DataStringValue, Is.EqualTo("123456"));
        }
    }
}
