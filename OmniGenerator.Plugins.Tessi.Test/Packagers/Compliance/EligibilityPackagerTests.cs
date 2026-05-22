using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;
using OmniGenerator.Lib;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Plugins.Tessi.Packagers;
using OmniGenerator.Plugins.Tessi.Packagers.Compliance;
using OmniGenerator.Plugins.Tessi.Test.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance.UnitTests
{
    [TestFixture]
    public class EligibilityPackagerTests
    {
        private InMemoryFileSystem _fileSystem = null!;
        private EligibilityPackager _packager = null!;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = new InMemoryFileSystem();
            _packager = new EligibilityPackager(_fileSystem);
        }

        private static string BasePath => @"C:\output";

        private string GetJsonContent()
        {
            var key = _fileSystem.TextFiles.Keys.First(k => k.EndsWith(".json"));
            return _fileSystem.TextFiles[key];
        }

        [Test]
        public async Task ProcessAsync_WithValidRootAndSingleDocument_CreatesFilesAndValidJson()
        {
            var root = CreateValidRoot(documentCount: 1);
            await _packager.ProcessAsync(root, BasePath, 300);

            Assert.That(_fileSystem.TextFiles.Keys.Any(k => k.EndsWith(".json")), Is.True, "Should create one JSON file");
            Assert.That(_fileSystem.TextFiles.Keys.Any(k => k.EndsWith(".top")), Is.True, "Should create one TOP file");

            using var jsonDocument = JsonDocument.Parse(GetJsonContent());
            var jsonRoot = jsonDocument.RootElement;
            Assert.That(jsonRoot.ValueKind, Is.EqualTo(JsonValueKind.Object));
            Assert.That(jsonRoot.TryGetProperty("transactions", out var transactionsElement), Is.True);
            Assert.That(transactionsElement.GetArrayLength(), Is.EqualTo(1));
        }

        [Test]
        public async Task ProcessAsync_WithEmptyRoot_CreatesFilesWithZeroTransactions()
        {
            var root = CreateValidRoot(documentCount: 0);
            await _packager.ProcessAsync(root, BasePath, 300);

            var jsonRoot = JsonSerializer.Deserialize<JsonRoot>(GetJsonContent());
            Assert.That(jsonRoot, Is.Not.Null);
            Assert.That(jsonRoot!.Transactions.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ProcessAsync_WithMultipleDocuments_CreatesTransactionForEach()
        {
            var root = CreateValidRoot(documentCount: 5);
            await _packager.ProcessAsync(root, BasePath, 300);

            using var jsonDocument = JsonDocument.Parse(GetJsonContent());
            var jsonRoot = jsonDocument.RootElement;
            Assert.That(jsonRoot.TryGetProperty("transactions", out var transactionsElement), Is.True);
            Assert.That(transactionsElement.GetArrayLength(), Is.EqualTo(5));
        }

        [Test]
        public void ProcessAsync_WithNullRoot_ThrowsException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _packager.ProcessAsync(null!, BasePath, 300));
        }

        [TestCase(0)]
        [TestCase(100)]
        [TestCase(300)]
        [TestCase(600)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public async Task ProcessAsync_WithVariousResolutions_CompletesSuccessfully(int resolution)
        {
            var root = CreateValidRoot(documentCount: 1);
            await _packager.ProcessAsync(root, BasePath, resolution);
            Assert.That(_fileSystem.TextFiles.Keys.Any(k => k.EndsWith(".json")), Is.True);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(1000000)]
        [TestCase(int.MaxValue)]
        public async Task ProcessAsync_WithVariousAmounts_SerializesCorrectly(int amount)
        {
            var root = CreateValidRootWithAmount(amount);
            await _packager.ProcessAsync(root, BasePath, 300);

            var jsonRoot = JsonSerializer.Deserialize<JsonRoot>(GetJsonContent());
            Assert.That(jsonRoot, Is.Not.Null);
            Assert.That(jsonRoot!.Transactions[0].Amount, Is.EqualTo(amount));
            Assert.That(jsonRoot.Transactions[0].Checks[0].Amount, Is.EqualTo(amount));
        }

        [Test]
        public async Task ProcessAsync_WhenDirectoryDoesNotExist_CreatesDirectory()
        {
            var root = CreateValidRoot(documentCount: 1);
            await _packager.ProcessAsync(root, BasePath, 300);
            Assert.That(_fileSystem.TextFiles.Keys.Any(k => k.EndsWith(".json")), Is.True);
        }

        [Test]
        public async Task ProcessAsync_CreatesTopFile_WithEmptyContent()
        {
            var root = CreateValidRoot(documentCount: 1);
            await _packager.ProcessAsync(root, BasePath, 300);

            var topKey = _fileSystem.TextFiles.Keys.First(k => k.EndsWith(".top"));
            Assert.That(_fileSystem.TextFiles[topKey], Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task ProcessAsync_IncludesHeaderFields_InJsonOutput()
        {
            var root = CreateValidRoot(documentCount: 1);
            await _packager.ProcessAsync(root, BasePath, 300);

            using var jsonDocument = JsonDocument.Parse(GetJsonContent());
            var jsonRoot = jsonDocument.RootElement;

            Assert.That(jsonRoot.TryGetProperty("header", out var headerElement), Is.True);
            Assert.That(headerElement.TryGetProperty("bankCode", out var bankCodeElement), Is.True);
            Assert.That(bankCodeElement.GetString(), Is.EqualTo("BANK001"));
            Assert.That(headerElement.TryGetProperty("bankUnitCode", out var bankUnitCodeElement), Is.True);
            Assert.That(bankUnitCodeElement.GetString(), Is.EqualTo("UNIT001"));
            Assert.That(headerElement.TryGetProperty("providerCode", out var providerCodeElement), Is.True);
            Assert.That(providerCodeElement.GetString(), Is.EqualTo("PROV001"));
            Assert.That(headerElement.TryGetProperty("culture", out var cultureElement), Is.True);
            Assert.That(cultureElement.GetString(), Is.EqualTo("fr-FR"));
            Assert.That(headerElement.TryGetProperty("purpose", out var purposeElement), Is.True);
            Assert.That(purposeElement.GetString(), Is.EqualTo("COMPLIANCE"));
            Assert.That(headerElement.TryGetProperty("bankFlow", out var bankFlowElement), Is.True);
            Assert.That(bankFlowElement.GetString(), Is.EqualTo("ELIGIBILITY"));
        }

        private Root CreateValidRoot(int documentCount) => CreateValidRootWithAmount(10000, documentCount);

        private Root CreateValidRootWithAmount(int amount, int documentCount = 1)
        {
            var rootFields = new Dictionary<string, Field>
            {
                { "bankCode", new Field("bankCode", "BANK001") },
                { "bankUnitCode", new Field("bankUnitCode", "UNIT001") },
                { "providerCode", new Field("providerCode", "PROV001") },
                { "culture", new Field("culture", "fr-FR") },
                { "purpose", new Field("purpose", "COMPLIANCE") },
                { "bankFlow", new Field("bankFlow", "ELIGIBILITY") },
                { "schema", new Field("schema", "https://schema.example.com/v1") },
                { "version", new Field("version", "1.0.0") },
                { "numlot", new Field("numlot", 12345) }
            };

            var documents = new List<Document>();
            for (int i = 0; i < documentCount; i++)
            {
                var docFields = new Dictionary<string, Field>
                {
                    { "scanner", new Field("scanner", "SCANNER001") },
                    { "scanType", new Field("scanType", "RECTO_VERSO") },
                    { "chain", new Field("chain", "CHAIN001") },
                    { "z4", new Field("z4", "Z4VALUE") },
                    { "z3", new Field("z3", "Z3VALUE") },
                    { "z2", new Field("z2", "Z2VALUE") },
                    { "amount", new Field("amount", amount) },
                    { "providerId", new Field("providerId", $"PROVID{i:D6}") },
                    { "remittingBranchCode", new Field("remittingBranchCode", "BRANCH001") },
                    { "deskCode", new Field("deskCode", "DESK001") },
                    { "accountNumber", new Field("accountNumber", "ACC123456789") }
                };
                documents.Add(new Document("cheque", null, docFields));
            }

            var group = new Group("batch", Array.Empty<Group>(), documents.ToArray(), new Dictionary<string, Field>());
            var root = new Root(new[] { group });

            var fieldCollection = new FieldCollection();
            foreach (var kvp in rootFields)
                fieldCollection.Add(kvp.Value);

            typeof(Root).GetProperty("Fields")!.SetValue(root, fieldCollection);
            return root;
        }
    }

    /// <summary>
    /// Unit tests for the Deposit class constructor.
    /// </summary>
    [TestFixture]
    public class DepositTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns all parameters to their corresponding properties
        /// with various input values including normal, empty, whitespace, and special character strings.
        /// </summary>
        /// <param name="culture">The culture parameter value.</param>
        /// <param name="remittingBranchCode">The remitting branch code parameter value.</param>
        /// <param name="scanBranchCode">The scan branch code parameter value.</param>
        /// <param name="scanner">The scanner parameter value.</param>
        /// <param name="scanType">The scan type parameter value.</param>
        /// <param name="chain">The chain parameter value.</param>
        [TestCase("en-US", "BRANCH001", "SCANBRANCH001", "SCANNER001", "TYPE001", "CHAIN001")]
        [TestCase("fr-FR", "12345", "67890", "ScannerXYZ", "FullScan", "ChainA")]
        [TestCase("", "", "", "", "", "")]
        [TestCase(" ", " ", " ", " ", " ", " ")]
        [TestCase("   ", "   ", "   ", "   ", "   ", "   ")]
        [TestCase("a", "b", "c", "d", "e", "f")]
        [TestCase("CultureWithSpecialChars!@#$%", "Branch\t\n", "Scan\r\n", "Scanner™®©", "Type💡🔥", "Chain∞§")]
        [TestCase("VeryLongCultureStringThatExceedsNormalExpectationsAndCouldPotentiallyCauseIssuesInSomeSystemsIfNotHandledProperly",
                  "VeryLongBranchCodeThatMightBeTooLongForSomeStorageSystems",
                  "VeryLongScanBranchCodeThatExceedsNormalLength",
                  "VeryLongScannerIdentifierThatIsProbablyTooLong",
                  "VeryLongScanTypeThatExceedsReasonableLength",
                  "VeryLongChainIdentifierThatCouldCausePotentialIssues")]
        public void Constructor_VariousInputs_AssignsPropertiesCorrectly(
            string culture,
            string remittingBranchCode,
            string scanBranchCode,
            string scanner,
            string scanType,
            string chain)
        {
            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.Culture, Is.EqualTo(culture));
            Assert.That(deposit.RemittingBranchCode, Is.EqualTo(remittingBranchCode));
            Assert.That(deposit.ScanBranchCode, Is.EqualTo(scanBranchCode));
            Assert.That(deposit.Scanner, Is.EqualTo(scanner));
            Assert.That(deposit.ScanType, Is.EqualTo(scanType));
            Assert.That(deposit.Chain, Is.EqualTo(chain));
        }

        /// <summary>
        /// Tests that the constructor always sets the RefOp property to a fixed value
        /// of "000000000000000000000000" (24 zeros) regardless of input parameters.
        /// </summary>
        [TestCase("en-US", "BRANCH001", "SCANBRANCH001", "SCANNER001", "TYPE001", "CHAIN001")]
        [TestCase("", "", "", "", "", "")]
        [TestCase("fr-FR", "12345", "67890", "ScannerXYZ", "FullScan", "ChainA")]
        public void Constructor_AnyInput_SetsRefOpToFixedValue(
            string culture,
            string remittingBranchCode,
            string scanBranchCode,
            string scanner,
            string scanType,
            string chain)
        {
            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.RefOp, Is.EqualTo("000000000000000000000000"));
            Assert.That(deposit.RefOp.Length, Is.EqualTo(24));
        }

        /// <summary>
        /// Tests that the constructor leaves the Region property uninitialized (null).
        /// The Region property is declared as nullable and is not set in the constructor.
        /// </summary>
        [Test]
        public void Constructor_AnyInput_LeavesRegionNull()
        {
            // Arrange
            var culture = "en-US";
            var remittingBranchCode = "BRANCH001";
            var scanBranchCode = "SCANBRANCH001";
            var scanner = "SCANNER001";
            var scanType = "TYPE001";
            var chain = "CHAIN001";

            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.Region, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor leaves the DepositSlip property uninitialized (null).
        /// The DepositSlip property is declared as nullable and is not set in the constructor.
        /// </summary>
        [Test]
        public void Constructor_AnyInput_LeavesDepositSlipNull()
        {
            // Arrange
            var culture = "en-US";
            var remittingBranchCode = "BRANCH001";
            var scanBranchCode = "SCANBRANCH001";
            var scanner = "SCANNER001";
            var scanType = "TYPE001";
            var chain = "CHAIN001";

            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.DepositSlip, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor correctly handles strings containing control characters
        /// such as tab, newline, and carriage return.
        /// </summary>
        [TestCase("\t\n\r", "\t", "\n", "\r", "\t\n", "\r\n", TestName = "Constructor_ControlCharacters_AssignsPropertiesCorrectly")]
        [TestCase("\0", "\0\0", "\0\0\0", "Null\0Char", "Type\0", "Chain\0", TestName = "Constructor_ControlCharacters_AssignsPropertiesCorrectly")]
        public void Constructor_ControlCharacters_AssignsPropertiesCorrectly(
            string culture,
            string remittingBranchCode,
            string scanBranchCode,
            string scanner,
            string scanType,
            string chain)
        {
            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.Culture, Is.EqualTo(culture));
            Assert.That(deposit.RemittingBranchCode, Is.EqualTo(remittingBranchCode));
            Assert.That(deposit.ScanBranchCode, Is.EqualTo(scanBranchCode));
            Assert.That(deposit.Scanner, Is.EqualTo(scanner));
            Assert.That(deposit.ScanType, Is.EqualTo(scanType));
            Assert.That(deposit.Chain, Is.EqualTo(chain));
        }

        /// <summary>
        /// Tests that the constructor correctly handles Unicode characters and emojis
        /// in string parameters.
        /// </summary>
        [TestCase("中文", "日本語", "한국어", "العربية", "עברית", "Русский")]
        [TestCase("😀", "🎉", "🔥", "💯", "🚀", "✨")]
        public void Constructor_UnicodeCharacters_AssignsPropertiesCorrectly(
            string culture,
            string remittingBranchCode,
            string scanBranchCode,
            string scanner,
            string scanType,
            string chain)
        {
            // Act
            var deposit = new Deposit(culture, remittingBranchCode, scanBranchCode, scanner, scanType, chain);

            // Assert
            Assert.That(deposit.Culture, Is.EqualTo(culture));
            Assert.That(deposit.RemittingBranchCode, Is.EqualTo(remittingBranchCode));
            Assert.That(deposit.ScanBranchCode, Is.EqualTo(scanBranchCode));
            Assert.That(deposit.Scanner, Is.EqualTo(scanner));
            Assert.That(deposit.ScanType, Is.EqualTo(scanType));
            Assert.That(deposit.Chain, Is.EqualTo(chain));
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="Check"/> class constructor.
    /// </summary>
    [TestFixture]
    public class CheckTests
    {
        /// <summary>
        /// Tests that the constructor correctly initializes all properties with valid inputs.
        /// </summary>
        [Test]
        public void Check_ValidInputs_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var culture = "en-US";
            var amount = 10000;
            var providerId = "PROVIDER123";
            var sequenceNumber = 42;
            var micr = new Micr("z4Value", "z3Value", "z2Value");
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);
            var afterCreation = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(check.Culture, Is.EqualTo(culture));
            Assert.That(check.Amount, Is.EqualTo(amount));
            Assert.That(check.ProviderId, Is.EqualTo(providerId));
            Assert.That(check.SequenceNumber, Is.EqualTo(sequenceNumber));
            Assert.That(check.Micr, Is.SameAs(micr));
            Assert.That(check.ImageRequest, Is.False);
            Assert.That(check.ComplianceId, Is.EqualTo(0));
            Assert.That(check.IsStandard, Is.True);
            Assert.That(check.ScanDate, Is.GreaterThanOrEqualTo(beforeCreation).And.LessThanOrEqualTo(afterCreation));
            Assert.That(check.Images, Is.Not.Null);
            Assert.That(check.Images, Is.Empty);
            Assert.That(check.OtherReferences, Is.Not.Null);
            Assert.That(check.OtherReferences, Has.Count.EqualTo(2));
            Assert.That(check.OtherReferences[0].Key, Is.EqualTo("additionnalLabel1"));
            Assert.That(check.OtherReferences[1].Key, Is.EqualTo("additionnalLabel2"));
            Assert.That(check.ComplianceReceptionDate, Is.EqualTo("0001-01-01T00:00:00+00:00"));
            Assert.That(check.ComplianceLogicalReceptionDate, Is.EqualTo("0001-01-01T00:00:00+00:00"));
            Assert.That(check.ComplianceRequestImageDate, Is.EqualTo("0001-01-01T00:00:00+00:00"));
            Assert.That(check.Result, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns the culture parameter.
        /// </summary>
        /// <param name="culture">The culture value to test.</param>
        [TestCase("en-US")]
        [TestCase("fr-FR")]
        [TestCase("de-DE")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("a")]
        [TestCase("very_long_culture_identifier_that_exceeds_typical_length_expectations_for_testing_purposes")]
        public void Check_VariousCultureValues_SetsCultureCorrectly(string culture)
        {
            // Arrange
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.Culture, Is.EqualTo(culture));
        }

        /// <summary>
        /// Tests that the constructor correctly assigns the providerId parameter.
        /// </summary>
        /// <param name="providerId">The provider ID value to test.</param>
        [TestCase("PROVIDER123")]
        [TestCase("A")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("PROVIDER_WITH_SPECIAL_CHARS!@#$%^&*()")]
        [TestCase("very_long_provider_id_that_exceeds_typical_length_expectations_for_boundary_testing_purposes")]
        public void Check_VariousProviderIdValues_SetsProviderIdCorrectly(string providerId)
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.ProviderId, Is.EqualTo(providerId));
        }

        /// <summary>
        /// Tests that the constructor correctly assigns various amount values including edge cases.
        /// </summary>
        /// <param name="amount">The amount value to test.</param>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(100)]
        [TestCase(-100)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Check_VariousAmountValues_SetsAmountCorrectly(int amount)
        {
            // Arrange
            var culture = "en-US";
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.Amount, Is.EqualTo(amount));
        }

        /// <summary>
        /// Tests that the constructor correctly assigns various sequence number values including edge cases.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number value to test.</param>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(42)]
        [TestCase(-42)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Check_VariousSequenceNumberValues_SetsSequenceNumberCorrectly(int sequenceNumber)
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.SequenceNumber, Is.EqualTo(sequenceNumber));
        }

        /// <summary>
        /// Tests that the constructor correctly assigns the Micr object reference.
        /// </summary>
        [Test]
        public void Check_ValidMicr_SetsMicrReference()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4Value", "z3Value", "z2Value");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.Micr, Is.SameAs(micr));
        }

        /// <summary>
        /// Tests that the constructor sets ImageRequest to false by default.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsImageRequestToFalse()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.ImageRequest, Is.False);
        }

        /// <summary>
        /// Tests that the constructor sets ComplianceId to zero by default.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsComplianceIdToZero()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.ComplianceId, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the constructor sets IsStandard to true by default.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsIsStandardToTrue()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.IsStandard, Is.True);
        }

        /// <summary>
        /// Tests that the constructor sets ScanDate to the current date and time.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsScanDateToCurrentDateTime()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);
            var afterCreation = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(check.ScanDate, Is.GreaterThanOrEqualTo(beforeCreation).And.LessThanOrEqualTo(afterCreation));
        }

        /// <summary>
        /// Tests that the constructor initializes Images as an empty list.
        /// </summary>
        [Test]
        public void Check_Constructor_InitializesImagesAsEmptyList()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.Images, Is.Not.Null);
            Assert.That(check.Images, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor initializes OtherReferences with exactly two items.
        /// </summary>
        [Test]
        public void Check_Constructor_InitializesOtherReferencesWithTwoItems()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.OtherReferences, Is.Not.Null);
            Assert.That(check.OtherReferences, Has.Count.EqualTo(2));
        }

        /// <summary>
        /// Tests that the constructor initializes OtherReferences with correct key values.
        /// </summary>
        [Test]
        public void Check_Constructor_InitializesOtherReferencesWithCorrectKeys()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.OtherReferences[0].Key, Is.EqualTo("additionnalLabel1"));
            Assert.That(check.OtherReferences[1].Key, Is.EqualTo("additionnalLabel2"));
        }

        /// <summary>
        /// Tests that the constructor sets all compliance date fields to the default ISO 8601 format date string.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsDefaultComplianceDates()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");
            var expectedDate = "0001-01-01T00:00:00+00:00";

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.ComplianceReceptionDate, Is.EqualTo(expectedDate));
            Assert.That(check.ComplianceLogicalReceptionDate, Is.EqualTo(expectedDate));
            Assert.That(check.ComplianceRequestImageDate, Is.EqualTo(expectedDate));
        }

        /// <summary>
        /// Tests that the constructor sets Result to null by default.
        /// </summary>
        [Test]
        public void Check_Constructor_SetsResultToNull()
        {
            // Arrange
            var culture = "en-US";
            var amount = 100;
            var providerId = "PROVIDER";
            var sequenceNumber = 1;
            var micr = new Micr("z4", "z3", "z2");

            // Act
            var check = new Check(culture, amount, providerId, sequenceNumber, micr);

            // Assert
            Assert.That(check.Result, Is.Null);
        }
    }

    [TestFixture]
    public class JsonRootTests
    {
        /// <summary>
        /// Tests that the JsonRoot constructor properly initializes all properties with valid string inputs.
        /// Verifies that Schema, Version, and Header are assigned correctly, Transactions list is initialized empty,
        /// and TimeStamp is set to approximately the current time.
        /// </summary>
        [Test]
        public void Constructor_ValidInputs_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var schema = "https://example.com/schema.json";
            var version = "1.0.0";
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);
            var afterCreation = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(schema));
            Assert.That(jsonRoot.Version, Is.EqualTo(version));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
            Assert.That(jsonRoot.Transactions, Is.Not.Null);
            Assert.That(jsonRoot.Transactions, Is.Empty);
            Assert.That(jsonRoot.TimeStamp, Is.GreaterThanOrEqualTo(beforeCreation));
            Assert.That(jsonRoot.TimeStamp, Is.LessThanOrEqualTo(afterCreation));
        }

        /// <summary>
        /// Tests the JsonRoot constructor with empty strings for schema and version parameters.
        /// Verifies that empty strings are accepted and assigned correctly.
        /// </summary>
        [Test]
        public void Constructor_EmptyStrings_InitializesWithEmptyStrings()
        {
            // Arrange
            var schema = string.Empty;
            var version = string.Empty;
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(string.Empty));
            Assert.That(jsonRoot.Version, Is.EqualTo(string.Empty));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
            Assert.That(jsonRoot.Transactions, Is.Not.Null);
            Assert.That(jsonRoot.Transactions, Is.Empty);
        }

        /// <summary>
        /// Tests the JsonRoot constructor with whitespace-only strings for schema and version.
        /// Verifies that whitespace strings are accepted and assigned correctly.
        /// </summary>
        [TestCase("   ", "   ")]
        [TestCase("\t", "\n")]
        [TestCase("  \t\n  ", "  \r\n  ")]
        public void Constructor_WhitespaceStrings_InitializesWithWhitespace(string schema, string version)
        {
            // Arrange
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(schema));
            Assert.That(jsonRoot.Version, Is.EqualTo(version));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
            Assert.That(jsonRoot.Transactions, Is.Not.Null);
            Assert.That(jsonRoot.Transactions, Is.Empty);
        }

        /// <summary>
        /// Tests the JsonRoot constructor with strings containing special characters.
        /// Verifies that special characters in schema and version parameters are handled correctly.
        /// </summary>
        [TestCase("schema://test@#$%", "v1.0-beta+build.123")]
        [TestCase("<schema>", "<version>")]
        [TestCase("schema\u0000test", "version\u0001test", TestName = "Constructor_SpecialCharacters_NullAndControlChars")]
        public void Constructor_SpecialCharacters_InitializesCorrectly(string schema, string version)
        {
            // Arrange
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(schema));
            Assert.That(jsonRoot.Version, Is.EqualTo(version));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
        }

        /// <summary>
        /// Tests the JsonRoot constructor with very long strings for schema and version.
        /// Verifies that long strings are accepted and assigned correctly.
        /// </summary>
        [Test]
        public void Constructor_VeryLongStrings_InitializesCorrectly()
        {
            // Arrange
            var schema = new string('s', 10000);
            var version = new string('v', 10000);
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(schema));
            Assert.That(jsonRoot.Version, Is.EqualTo(version));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
        }

        /// <summary>
        /// Tests that the Transactions property is initialized as an empty list that can be modified.
        /// Verifies that items can be added to the Transactions list after construction.
        /// </summary>
        [Test]
        public void Constructor_TransactionsProperty_IsInitializedAsEmptyModifiableList()
        {
            // Arrange
            var schema = "schema";
            var version = "1.0";
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");
            var jsonRoot = new JsonRoot(schema, version, header);

            // Act
            var deposit = new Deposit("en-US", "BRANCH001", "SCAN001", "SCANNER1", "Type1", "Chain1");
            var micr = new Micr("Z4Value", "Z3Value", "Z2Value");
            var check = new Check("en-US", 1000, "PROV001", 1, micr);
            var transaction = new Transaction(1000, "UNIT001", "DESK001", "ACC123", deposit, check);
            jsonRoot.Transactions.Add(transaction);

            // Assert
            Assert.That(jsonRoot.Transactions, Has.Count.EqualTo(1));
            Assert.That(jsonRoot.Transactions[0], Is.SameAs(transaction));
        }

        /// <summary>
        /// Tests that TimeStamp property is set to a recent time when the constructor is called.
        /// Verifies that the TimeStamp is not default DateTime value and is within reasonable bounds.
        /// </summary>
        [Test]
        public void Constructor_TimeStampProperty_IsSetToCurrentTime()
        {
            // Arrange
            var schema = "schema";
            var version = "1.0";
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");
            var beforeCreation = DateTime.Now.AddSeconds(-2);

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);
            var afterCreation = DateTime.Now.AddSeconds(2);

            // Assert
            Assert.That(jsonRoot.TimeStamp, Is.Not.EqualTo(default(DateTime)));
            Assert.That(jsonRoot.TimeStamp, Is.GreaterThanOrEqualTo(beforeCreation));
            Assert.That(jsonRoot.TimeStamp, Is.LessThanOrEqualTo(afterCreation));
        }

        /// <summary>
        /// Tests that the Header parameter reference is maintained in the JsonRoot instance.
        /// Verifies that modifications to the Header object after construction are reflected in JsonRoot.
        /// </summary>
        [Test]
        public void Constructor_HeaderParameter_MaintainsReferenceToOriginalObject()
        {
            // Arrange
            var schema = "schema";
            var version = "1.0";
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");
            var jsonRoot = new JsonRoot(schema, version, header);

            // Act
            header.BankCode = "NEWBANK";

            // Assert
            Assert.That(jsonRoot.Header.BankCode, Is.EqualTo("NEWBANK"));
        }

        /// <summary>
        /// Tests the JsonRoot constructor with various combinations of edge case inputs.
        /// Verifies proper initialization across multiple boundary scenarios.
        /// </summary>
        [TestCase("", "1.0")]
        [TestCase("schema", "")]
        [TestCase("", "")]
        [TestCase("a", "b")]
        [TestCase("https://example.com/very/long/path/to/schema.json", "1.0.0-alpha+20230101")]
        public void Constructor_VariousInputCombinations_InitializesCorrectly(string schema, string version)
        {
            // Arrange
            var header = new Header("BANK001", "UNIT001", "PROV001", "en-US", "Eligibility", "Flow1");

            // Act
            var jsonRoot = new JsonRoot(schema, version, header);

            // Assert
            Assert.That(jsonRoot.Schema, Is.EqualTo(schema));
            Assert.That(jsonRoot.Version, Is.EqualTo(version));
            Assert.That(jsonRoot.Header, Is.SameAs(header));
            Assert.That(jsonRoot.Transactions, Is.Not.Null);
            Assert.That(jsonRoot.Transactions, Is.Empty);
            Assert.That(jsonRoot.TimeStamp, Is.GreaterThan(DateTime.MinValue));
        }
    }

    [TestFixture]
    public class ZoneTests
    {
        /// <summary>
        /// Tests that the Zone constructor correctly assigns valid string values to both Key and Value properties.
        /// </summary>
        /// <param name="key">The key parameter value.</param>
        /// <param name="value">The value parameter value.</param>
        [TestCase("Z4", "12345")]
        [TestCase("Z3", "67890")]
        [TestCase("Z2", "ABCDE")]
        [TestCase("", "someValue")]
        [TestCase("someKey", "")]
        [TestCase("", "")]
        [TestCase(" ", "value")]
        [TestCase("key", " ")]
        [TestCase("\t", "\n")]
        [TestCase("key with spaces", "value with spaces")]
        [TestCase("key!@#$%^&*()", "value!@#$%^&*()")]
        public void Constructor_WithValidStrings_AssignsPropertiesCorrectly(string key, string value)
        {
            // Arrange & Act
            var zone = new Zone(key, value);

            // Assert
            Assert.That(zone.Key, Is.EqualTo(key));
            Assert.That(zone.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Zone constructor correctly handles null value parameter.
        /// Value property is nullable, so null should be accepted and assigned.
        /// </summary>
        [Test]
        public void Constructor_WithNullValue_AssignsNullToValueProperty()
        {
            // Arrange
            var key = "TestKey";
            string? value = null;

            // Act
            var zone = new Zone(key, value!);

            // Assert
            Assert.That(zone.Key, Is.EqualTo(key));
            Assert.That(zone.Value, Is.Null);
        }

        /// <summary>
        /// Tests that the Zone constructor handles very long strings correctly.
        /// </summary>
        [Test]
        public void Constructor_WithVeryLongStrings_AssignsPropertiesCorrectly()
        {
            // Arrange
            var longKey = new string('K', 10000);
            var longValue = new string('V', 10000);

            // Act
            var zone = new Zone(longKey, longValue);

            // Assert
            Assert.That(zone.Key, Is.EqualTo(longKey));
            Assert.That(zone.Value, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that the Zone constructor handles strings with control characters and special Unicode.
        /// </summary>
        [TestCase("key\0", "value\0", TestName = "Constructor_WithSpecialCharacters_NullChar")]
        [TestCase("key\r\n", "value\r\n")]
        [TestCase("key\u0001\u0002", "value\u0003\u0004", TestName = "Constructor_WithSpecialCharacters_ControlChars")]
        [TestCase("key™©®", "value™©®")]
        [TestCase("key中文", "value日本語")]
        public void Constructor_WithSpecialCharacters_AssignsPropertiesCorrectly(string key, string value)
        {
            // Arrange & Act
            var zone = new Zone(key, value);

            // Assert
            Assert.That(zone.Key, Is.EqualTo(key));
            Assert.That(zone.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Zone constructor handles whitespace-only strings correctly.
        /// </summary>
        [TestCase("   ", "   ")]
        [TestCase("\t\t\t", "\t\t\t")]
        [TestCase("\r\n\r\n", "\r\n\r\n")]
        [TestCase("  \t  \n  ", "  \t  \n  ")]
        public void Constructor_WithWhitespaceStrings_AssignsPropertiesCorrectly(string key, string value)
        {
            // Arrange & Act
            var zone = new Zone(key, value);

            // Assert
            Assert.That(zone.Key, Is.EqualTo(key));
            Assert.That(zone.Value, Is.EqualTo(value));
        }
    }

    /// <summary>
    /// Unit tests for the Micr class.
    /// </summary>
    [TestFixture]
    public class MicrTests
    {
        /// <summary>
        /// Tests that the Micr constructor creates a Zone list with exactly 3 Zone objects
        /// with the correct keys and values for valid non-null string inputs.
        /// </summary>
        /// <param name="z4">The value for zone Z4</param>
        /// <param name="z3">The value for zone Z3</param>
        /// <param name="z2">The value for zone Z2</param>
        [TestCase("12345", "67890", "ABCDE")]
        [TestCase("a", "b", "c")]
        [TestCase("", "", "")]
        [TestCase("   ", "\t", "\n")]
        [TestCase("!@#$%^&*()", "<>?:\"{}", "[]\\|;',./")]
        public void Constructor_ValidStringInputs_CreatesZoneListWithThreeElements(string z4, string z3, string z2)
        {
            // Arrange & Act
            var micr = new Micr(z4, z3, z2);

            // Assert
            Assert.That(micr.Zone, Is.Not.Null);
            Assert.That(micr.Zone.Count, Is.EqualTo(3));
            Assert.That(micr.Zone[0].Key, Is.EqualTo("Z4"));
            Assert.That(micr.Zone[0].Value, Is.EqualTo(z4));
            Assert.That(micr.Zone[1].Key, Is.EqualTo("Z3"));
            Assert.That(micr.Zone[1].Value, Is.EqualTo(z3));
            Assert.That(micr.Zone[2].Key, Is.EqualTo("Z2"));
            Assert.That(micr.Zone[2].Value, Is.EqualTo(z2));
        }

        /// <summary>
        /// Tests that the Micr constructor correctly handles null values for zone parameters.
        /// </summary>
        /// <param name="z4">The value for zone Z4</param>
        /// <param name="z3">The value for zone Z3</param>
        /// <param name="z2">The value for zone Z2</param>
        [TestCase(null, "value3", "value2")]
        [TestCase("value4", null, "value2")]
        [TestCase("value4", "value3", null)]
        [TestCase(null, null, null)]
        [TestCase(null, null, "value2")]
        [TestCase(null, "value3", null)]
        [TestCase("value4", null, null)]
        public void Constructor_NullInputs_CreatesZoneListWithNullValues(string? z4, string? z3, string? z2)
        {
            // Arrange & Act
            var micr = new Micr(z4!, z3!, z2!);

            // Assert
            Assert.That(micr.Zone, Is.Not.Null);
            Assert.That(micr.Zone.Count, Is.EqualTo(3));
            Assert.That(micr.Zone[0].Key, Is.EqualTo("Z4"));
            Assert.That(micr.Zone[0].Value, Is.EqualTo(z4));
            Assert.That(micr.Zone[1].Key, Is.EqualTo("Z3"));
            Assert.That(micr.Zone[1].Value, Is.EqualTo(z3));
            Assert.That(micr.Zone[2].Key, Is.EqualTo("Z2"));
            Assert.That(micr.Zone[2].Value, Is.EqualTo(z2));
        }

        /// <summary>
        /// Tests that the Micr constructor correctly handles very long string inputs.
        /// </summary>
        [Test]
        public void Constructor_VeryLongStrings_CreatesZoneListWithLongValues()
        {
            // Arrange
            var z4 = new string('A', 10000);
            var z3 = new string('B', 10000);
            var z2 = new string('C', 10000);

            // Act
            var micr = new Micr(z4, z3, z2);

            // Assert
            Assert.That(micr.Zone, Is.Not.Null);
            Assert.That(micr.Zone.Count, Is.EqualTo(3));
            Assert.That(micr.Zone[0].Value, Is.EqualTo(z4));
            Assert.That(micr.Zone[1].Value, Is.EqualTo(z3));
            Assert.That(micr.Zone[2].Value, Is.EqualTo(z2));
        }

        /// <summary>
        /// Tests that the Micr constructor preserves the order of zones as Z4, Z3, Z2.
        /// </summary>
        [Test]
        public void Constructor_AlwaysCreatesZonesInOrder_Z4_Z3_Z2()
        {
            // Arrange & Act
            var micr = new Micr("first", "second", "third");

            // Assert
            Assert.That(micr.Zone[0].Key, Is.EqualTo("Z4"));
            Assert.That(micr.Zone[1].Key, Is.EqualTo("Z3"));
            Assert.That(micr.Zone[2].Key, Is.EqualTo("Z2"));
        }

        /// <summary>
        /// Tests that the Micr constructor handles strings with control characters correctly.
        /// </summary>
        [Test]
        public void Constructor_StringsWithControlCharacters_PreservesValues()
        {
            // Arrange
            var z4 = "value\u0000with\u0001control\u0002chars";
            var z3 = "\r\n\t";
            var z2 = "normal";

            // Act
            var micr = new Micr(z4, z3, z2);

            // Assert
            Assert.That(micr.Zone[0].Value, Is.EqualTo(z4));
            Assert.That(micr.Zone[1].Value, Is.EqualTo(z3));
            Assert.That(micr.Zone[2].Value, Is.EqualTo(z2));
        }
    }

    /// <summary>
    /// Unit tests for the Transaction class constructor.
    /// </summary>
    [TestFixture]
    public class TransactionTests
    {
        /// <summary>
        /// Tests that the Transaction constructor properly initializes all properties with valid inputs.
        /// </summary>
        [Test]
        public void Constructor_ValidInputs_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();
            var beforeCreation = DateTime.Now;

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);
            var afterCreation = DateTime.Now;

            // Assert
            Assert.That(transaction.Amount, Is.EqualTo(amount));
            Assert.That(transaction.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(transaction.DeskCode, Is.EqualTo(deskCode));
            Assert.That(transaction.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(transaction.Deposit, Is.SameAs(deposit));
            Assert.That(transaction.IsTechnicalAccount, Is.False);
            Assert.That(transaction.RiskInformation, Is.EqualTo(string.Empty));
            Assert.That(transaction.AccountHolders, Is.Not.Null);
            Assert.That(transaction.AccountHolders, Is.Empty);
            Assert.That(transaction.Checks, Is.Not.Null);
            Assert.That(transaction.Checks.Count, Is.EqualTo(1));
            Assert.That(transaction.Checks[0], Is.SameAs(cheque));
            Assert.That(transaction.Date, Is.InRange(beforeCreation, afterCreation));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles edge case numeric values for amount parameter.
        /// </summary>
        /// <param name="amount">The amount value to test.</param>
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1000)]
        [TestCase(1)]
        public void Constructor_EdgeCaseAmounts_AssignsAmountCorrectly(int amount)
        {
            // Arrange
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.Amount, Is.EqualTo(amount));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles empty string for bankUnitCode parameter.
        /// </summary>
        [Test]
        public void Constructor_EmptyBankUnitCode_AssignsEmptyString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = string.Empty;
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.BankUnitCode, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles whitespace string for bankUnitCode parameter.
        /// </summary>
        [Test]
        public void Constructor_WhitespaceBankUnitCode_AssignsWhitespaceString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "   ";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.BankUnitCode, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles very long string for bankUnitCode parameter.
        /// </summary>
        [Test]
        public void Constructor_VeryLongBankUnitCode_AssignsLongString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = new string('X', 10000);
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(transaction.BankUnitCode.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles empty string for deskCode parameter.
        /// </summary>
        [Test]
        public void Constructor_EmptyDeskCode_AssignsEmptyString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = string.Empty;
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.DeskCode, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles whitespace string for deskCode parameter.
        /// </summary>
        [Test]
        public void Constructor_WhitespaceDeskCode_AssignsWhitespaceString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "   ";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.DeskCode, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles very long string for deskCode parameter.
        /// </summary>
        [Test]
        public void Constructor_VeryLongDeskCode_AssignsLongString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = new string('D', 10000);
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.DeskCode, Is.EqualTo(deskCode));
            Assert.That(transaction.DeskCode.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles empty string for accountNumber parameter.
        /// </summary>
        [Test]
        public void Constructor_EmptyAccountNumber_AssignsEmptyString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = string.Empty;
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.AccountNumber, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles whitespace string for accountNumber parameter.
        /// </summary>
        [Test]
        public void Constructor_WhitespaceAccountNumber_AssignsWhitespaceString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "   ";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.AccountNumber, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles very long string for accountNumber parameter.
        /// </summary>
        [Test]
        public void Constructor_VeryLongAccountNumber_AssignsLongString()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = new string('A', 10000);
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(transaction.AccountNumber.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly handles strings with special characters.
        /// </summary>
        [Test]
        public void Constructor_SpecialCharactersInStrings_AssignsSpecialCharacters()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU@#$%^&*()";
            var deskCode = "DESK\t\r\n";
            var accountNumber = "ACC\u0000\u0001\u0002";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(transaction.DeskCode, Is.EqualTo(deskCode));
            Assert.That(transaction.AccountNumber, Is.EqualTo(accountNumber));
        }

        /// <summary>
        /// Tests that the Transaction constructor sets Date property to current time.
        /// </summary>
        [Test]
        public void Constructor_SetsDateToCurrentTime()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);
            var afterCreation = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(transaction.Date, Is.GreaterThan(beforeCreation));
            Assert.That(transaction.Date, Is.LessThan(afterCreation));
        }

        /// <summary>
        /// Tests that the Transaction constructor always sets IsTechnicalAccount to false.
        /// </summary>
        [Test]
        public void Constructor_AlwaysSetsIsTechnicalAccountToFalse()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.IsTechnicalAccount, Is.False);
        }

        /// <summary>
        /// Tests that the Transaction constructor always sets RiskInformation to empty string.
        /// </summary>
        [Test]
        public void Constructor_AlwaysSetsRiskInformationToEmpty()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.RiskInformation, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Transaction constructor initializes AccountHolders as an empty list.
        /// </summary>
        [Test]
        public void Constructor_InitializesAccountHoldersAsEmptyList()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.AccountHolders, Is.Not.Null);
            Assert.That(transaction.AccountHolders, Is.Empty);
            Assert.That(transaction.AccountHolders, Is.InstanceOf<List<object>>());
        }

        /// <summary>
        /// Tests that the Transaction constructor initializes Checks list with exactly one check.
        /// </summary>
        [Test]
        public void Constructor_InitializesChecksListWithSingleCheck()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.Checks, Is.Not.Null);
            Assert.That(transaction.Checks.Count, Is.EqualTo(1));
            Assert.That(transaction.Checks[0], Is.SameAs(cheque));
            Assert.That(transaction.Checks, Is.InstanceOf<List<Check>>());
        }

        /// <summary>
        /// Tests that the Transaction constructor correctly assigns the deposit reference.
        /// </summary>
        [Test]
        public void Constructor_AssignsDepositReference()
        {
            // Arrange
            var amount = 1000;
            var bankUnitCode = "BU001";
            var deskCode = "DESK001";
            var accountNumber = "ACC123456";
            var deposit = CreateValidDeposit();
            var cheque = CreateValidCheck();

            // Act
            var transaction = new Transaction(amount, bankUnitCode, deskCode, accountNumber, deposit, cheque);

            // Assert
            Assert.That(transaction.Deposit, Is.SameAs(deposit));
        }

        private Deposit CreateValidDeposit()
        {
            return new Deposit(
                culture: "en-US",
                remittingBranchCode: "RBC001",
                scanBranchCode: "SBC001",
                scanner: "SCANNER001",
                scanType: "TYPE001",
                chain: "CHAIN001"
            );
        }

        private Check CreateValidCheck()
        {
            var micr = new Micr("Z4Value", "Z3Value", "Z2Value");
            return new Check(
                culture: "en-US",
                amount: 500,
                providerId: "PROV001",
                sequenceNumber: 1,
                micr: micr
            );
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="Header"/> class.
    /// </summary>
    [TestFixture]
    public class HeaderTests
    {
        /// <summary>
        /// Verifies that the constructor correctly initializes all properties with valid input values.
        /// </summary>
        /// <param name="bankCode">The bank code to test.</param>
        /// <param name="bankUnitCode">The bank unit code to test.</param>
        /// <param name="providerCode">The provider code to test.</param>
        /// <param name="culture">The culture to test.</param>
        /// <param name="purpose">The purpose to test.</param>
        /// <param name="bankFlow">The bank flow to test.</param>
        [TestCase("BNK001", "UNIT01", "PROV01", "en-US", "Test", "Flow01")]
        [TestCase("", "", "", "", "", "")]
        [TestCase("   ", "\t", "\n", "  \r\n  ", "\t\t", "   ")]
        [TestCase("ABC", "DEF", "GHI", "JKL", "MNO", "PQR")]
        public void Constructor_ValidInputs_InitializesAllPropertiesCorrectly(
            string bankCode,
            string bankUnitCode,
            string providerCode,
            string culture,
            string purpose,
            string bankFlow)
        {
            // Arrange & Act
            var header = new Header(bankCode, bankUnitCode, providerCode, culture, purpose, bankFlow);

            // Assert
            Assert.That(header.BankCode, Is.EqualTo(bankCode));
            Assert.That(header.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(header.ProviderCode, Is.EqualTo(providerCode));
            Assert.That(header.Culture, Is.EqualTo(culture));
            Assert.That(header.Purpose, Is.EqualTo(purpose));
            Assert.That(header.BankFlow, Is.EqualTo(bankFlow));
        }

        /// <summary>
        /// Verifies that the constructor correctly handles strings with special characters.
        /// </summary>
        [Test]
        public void Constructor_SpecialCharactersInStrings_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var bankCode = "BNK!@#$%^&*()";
            var bankUnitCode = "UNIT<>?/\\|";
            var providerCode = "PROV\"'`~";
            var culture = "文化测试";
            var purpose = "Püřpøšé";
            var bankFlow = "Flow\u0001\u0002\u0003";

            // Act
            var header = new Header(bankCode, bankUnitCode, providerCode, culture, purpose, bankFlow);

            // Assert
            Assert.That(header.BankCode, Is.EqualTo(bankCode));
            Assert.That(header.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(header.ProviderCode, Is.EqualTo(providerCode));
            Assert.That(header.Culture, Is.EqualTo(culture));
            Assert.That(header.Purpose, Is.EqualTo(purpose));
            Assert.That(header.BankFlow, Is.EqualTo(bankFlow));
        }

        /// <summary>
        /// Verifies that the constructor correctly handles very long strings.
        /// </summary>
        [Test]
        public void Constructor_VeryLongStrings_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var bankCode = new string('A', 10000);
            var bankUnitCode = new string('B', 10000);
            var providerCode = new string('C', 10000);
            var culture = new string('D', 10000);
            var purpose = new string('E', 10000);
            var bankFlow = new string('F', 10000);

            // Act
            var header = new Header(bankCode, bankUnitCode, providerCode, culture, purpose, bankFlow);

            // Assert
            Assert.That(header.BankCode, Is.EqualTo(bankCode));
            Assert.That(header.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(header.ProviderCode, Is.EqualTo(providerCode));
            Assert.That(header.Culture, Is.EqualTo(culture));
            Assert.That(header.Purpose, Is.EqualTo(purpose));
            Assert.That(header.BankFlow, Is.EqualTo(bankFlow));
        }

        /// <summary>
        /// Verifies that the constructor correctly initializes properties with minimal valid data.
        /// </summary>
        [Test]
        public void Constructor_MinimalValidData_InitializesAllPropertiesCorrectly()
        {
            // Arrange
            var bankCode = "1";
            var bankUnitCode = "2";
            var providerCode = "3";
            var culture = "4";
            var purpose = "5";
            var bankFlow = "6";

            // Act
            var header = new Header(bankCode, bankUnitCode, providerCode, culture, purpose, bankFlow);

            // Assert
            Assert.That(header.BankCode, Is.EqualTo(bankCode));
            Assert.That(header.BankUnitCode, Is.EqualTo(bankUnitCode));
            Assert.That(header.ProviderCode, Is.EqualTo(providerCode));
            Assert.That(header.Culture, Is.EqualTo(culture));
            Assert.That(header.Purpose, Is.EqualTo(purpose));
            Assert.That(header.BankFlow, Is.EqualTo(bankFlow));
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="OtherReference"/> class.
    /// </summary>
    [TestFixture]
    public class OtherReferenceTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns the key parameter to the Key property
        /// for various valid string inputs including normal strings, empty strings, whitespace,
        /// special characters, and very long strings.
        /// </summary>
        /// <param name="key">The key value to pass to the constructor.</param>
        [TestCase("normalKey")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("key-with-special-chars-!@#$%^&*()")]
        [TestCase("Unicode🎉Test")]
        [TestCase("a")]
        public void Constructor_ValidStringInput_SetsKeyPropertyCorrectly(string key)
        {
            // Arrange & Act
            var otherReference = new OtherReference(key);

            // Assert
            Assert.That(otherReference.Key, Is.EqualTo(key));
        }

        /// <summary>
        /// Tests that the constructor correctly assigns an extremely long string to the Key property.
        /// </summary>
        [Test]
        public void Constructor_VeryLongString_SetsKeyPropertyCorrectly()
        {
            // Arrange
            var veryLongKey = new string('a', 10000);

            // Act
            var otherReference = new OtherReference(veryLongKey);

            // Assert
            Assert.That(otherReference.Key, Is.EqualTo(veryLongKey));
            Assert.That(otherReference.Key.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the constructor leaves the Value property as null when only key is provided.
        /// </summary>
        [Test]
        public void Constructor_WithKey_LeavesValuePropertyNull()
        {
            // Arrange
            var key = "testKey";

            // Act
            var otherReference = new OtherReference(key);

            // Assert
            Assert.That(otherReference.Value, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor accepts null input for the key parameter despite it being
        /// declared as non-nullable, documenting runtime behavior.
        /// </summary>
        [Test]
        public void Constructor_NullKey_SetsKeyPropertyToNull()
        {
            // Arrange
            string? key = null;

            // Act
            var otherReference = new OtherReference(key!);

            // Assert
            Assert.That(otherReference.Key, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor correctly handles strings with control characters.
        /// </summary>
        [Test]
        public void Constructor_StringWithControlCharacters_SetsKeyPropertyCorrectly()
        {
            // Arrange
            var key = "key\r\nwith\0control\bchars";

            // Act
            var otherReference = new OtherReference(key);

            // Assert
            Assert.That(otherReference.Key, Is.EqualTo(key));
        }
    }
}
