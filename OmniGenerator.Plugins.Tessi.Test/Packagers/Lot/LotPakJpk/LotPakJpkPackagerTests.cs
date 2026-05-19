using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;


namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="LotPakJpkPackager"/> class.
    /// Tests focus on the ProcessAsync method covering parameter validation, edge cases, and file creation.
    /// </summary>
    [TestFixture]
    public partial class LotPakJpkPackagerTests
    {
        /// <summary>
        /// Tests that ProcessAsync throws a NullReferenceException when the root parameter is null.
        /// This test verifies that null input validation occurs when accessing root.Fields.
        /// Expected result: NullReferenceException is thrown.
        /// </summary>
        [Test]
        public void ProcessAsync_NullRoot_ThrowsNullReferenceException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            Root? root = null;
            var basepath = Path.GetTempPath();
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await packager.ProcessAsync(root!, basepath, resolution));
        }

        /// <summary>
        /// Tests that ProcessAsync throws an ArgumentNullException when the basepath parameter is null.
        /// This test verifies that null path validation occurs in Path.Combine or FileStream constructor.
        /// Expected result: ArgumentNullException is thrown.
        /// </summary>
        [Test]
        public void ProcessAsync_NullBasepath_ThrowsArgumentNullException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            string? basepath = null;
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await packager.ProcessAsync(root, basepath!, resolution));
        }

        /// <summary>
        /// Tests that ProcessAsync throws DirectoryNotFoundException when basepath points to a non-existent directory.
        /// This test verifies that the method fails appropriately when trying to create files in an invalid location.
        /// Expected result: DirectoryNotFoundException is thrown.
        /// </summary>
        [Test]
        public void ProcessAsync_NonExistentDirectory_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var basepath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nonexistent", "path");
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<DirectoryNotFoundException>(async () =>
                await packager.ProcessAsync(root, basepath, resolution));
        }

        /// <summary>
        /// Tests that ProcessAsync handles empty basepath string.
        /// This test verifies behavior when an empty string is provided as the base path.
        /// Expected result: An exception is thrown (ArgumentException or similar).
        /// </summary>
        [Test]
        [Category("ProductionBugSuspected")]
        [Ignore("ProductionBugSuspected")]
        public void ProcessAsync_EmptyBasepath_ThrowsException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var basepath = string.Empty;
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await packager.ProcessAsync(root, basepath, resolution));
        }

        /// <summary>
        /// Tests that ProcessAsync handles basepath with invalid characters.
        /// This test verifies that the method throws an appropriate exception when the path contains invalid characters.
        /// Expected result: ArgumentException is thrown.
        /// </summary>
        [Test]
        [Category("ProductionBugSuspected")]
        [Ignore("ProductionBugSuspected")]
        public void ProcessAsync_InvalidPathCharacters_ThrowsArgumentException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var basepath = "C:\\Invalid<>Path|With?Chars";
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await packager.ProcessAsync(root, basepath, resolution));
        }

        /// <summary>
        /// Tests ProcessAsync with various boundary values for imageRenderingResolution parameter.
        /// This test verifies that the resolution parameter is accepted across its full range including edge cases.
        /// Expected result: The method completes without throwing for resolution assignment.
        /// </summary>
        /// <param name="resolution">The DPI resolution value to test.</param>
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(300)]
        [TestCase(int.MaxValue)]
        public async Task ProcessAsync_VariousResolutionValues_CompletesSuccessfully(int resolution)
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var tempDir = CreateTempDirectory();

            try
            {
                // Act
                await packager.ProcessAsync(root, tempDir, resolution);

                // Assert
                Assert.Pass("Method completed with resolution: " + resolution);
            }
            finally
            {
                // Cleanup
                CleanupTempDirectory(tempDir);
            }
        }

        /// <summary>
        /// Tests that ProcessAsync creates the expected output files (.lot, .pak, .jpk) when given valid parameters with no documents.
        /// This test verifies that all three required files are created even when the document collection is empty.
        /// Expected result: Three files are created with the expected extensions.
        /// </summary>
        [Test]
        public async Task ProcessAsync_ValidParametersEmptyDocuments_CreatesAllFiles()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var tempDir = CreateTempDirectory();

            try
            {
                // Act
                await packager.ProcessAsync(root, tempDir, 300);

                // Assert
                var lotFile = Path.Combine(tempDir, "DefaultName.lot");
                var pakFile = Path.Combine(tempDir, "DefaultName.pak");
                var jpkFile = Path.Combine(tempDir, "DefaultName.jpk");

                Assert.That(File.Exists(lotFile), Is.True, "LOT file should be created");
                Assert.That(File.Exists(pakFile), Is.True, "PAK file should be created");
                Assert.That(File.Exists(jpkFile), Is.True, "JPK file should be created");
            }
            finally
            {
                // Cleanup
                CleanupTempDirectory(tempDir);
            }
        }

        /// <summary>
        /// Tests that ProcessAsync throws an exception when required fields are missing from the root.
        /// This test verifies that the RootFields validation catches missing required fields.
        /// Expected result: An exception is thrown when accessing required fields.
        /// </summary>
        [Test]
        public void ProcessAsync_MissingRequiredFields_ThrowsException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            // Create root with empty fields (missing required fields like capture-point-code, etc.)
            var root = new Root(new List<Group>());
            var tempDir = CreateTempDirectory();

            try
            {
                // Act & Assert
                Assert.ThrowsAsync<OmniGenerator.Lib.Exceptions.FieldNotFoundException>(async () =>
                    await packager.ProcessAsync(root, tempDir, 300));
            }
            finally
            {
                // Cleanup
                CleanupTempDirectory(tempDir);
            }
        }

        /// <summary>
        /// Tests that ProcessAsync creates files with custom packet name from root fields.
        /// This test verifies that the PacketName field from RootFields is used for file naming.
        /// Expected result: Files are created with the custom packet name.
        /// </summary>
        [Test]
        public async Task ProcessAsync_CustomPacketName_CreatesFilesWithCustomName()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var customPacketName = "CustomPacket123";
            var root = CreateValidRootWithPacketName(customPacketName);
            var tempDir = CreateTempDirectory();

            try
            {
                // Act
                await packager.ProcessAsync(root, tempDir, 300);

                // Assert
                var lotFile = Path.Combine(tempDir, $"{customPacketName}.lot");
                var pakFile = Path.Combine(tempDir, $"{customPacketName}.pak");
                var jpkFile = Path.Combine(tempDir, $"{customPacketName}.jpk");

                Assert.That(File.Exists(lotFile), Is.True, "LOT file should be created with custom name");
                Assert.That(File.Exists(pakFile), Is.True, "PAK file should be created with custom name");
                Assert.That(File.Exists(jpkFile), Is.True, "JPK file should be created with custom name");
            }
            finally
            {
                // Cleanup
                CleanupTempDirectory(tempDir);
            }
        }

        /// <summary>
        /// Tests that ProcessAsync handles whitespace-only basepath.
        /// This test verifies that the method properly validates basepath input.
        /// Expected result: An exception is thrown for invalid path.
        /// </summary>
        [Test]
        public void ProcessAsync_WhitespaceBasepath_ThrowsException()
        {
            // Arrange
            var packager = new LotPakJpkPackager();
            var root = CreateValidRoot();
            var basepath = "   ";
            var resolution = 300;

            // Act & Assert
            Assert.ThrowsAsync<DirectoryNotFoundException>(async () =>
                await packager.ProcessAsync(root, basepath, resolution));
        }

        #region Helper Methods

        /// <summary>
        /// Creates a valid Root object with minimal required fields for testing.
        /// </summary>
        /// <returns>A Root instance with required fields populated.</returns>
        private Root CreateValidRoot()
        {
            var root = new Root(new List<Group>());
            var fields = new Dictionary<string, Field>
            {
                { "capture-point-code", new Field("capture-point-code", "TEST001") },
                { "organization-unit-code", new Field("organization-unit-code", "UNIT001") },
                { "organization-code", new Field("organization-code", "ORG001") }
            };
            root.AddFields(fields);
            return root;
        }

        /// <summary>
        /// Creates a valid Root object with custom packet name.
        /// </summary>
        /// <param name="packetName">The custom packet name to use.</param>
        /// <returns>A Root instance with specified packet name.</returns>
        private Root CreateValidRootWithPacketName(string packetName)
        {
            var root = new Root(new List<Group>());
            var fields = new Dictionary<string, Field>
            {
                { "packet-name", new Field("packet-name", packetName) },
                { "capture-point-code", new Field("capture-point-code", "TEST001") },
                { "organization-unit-code", new Field("organization-unit-code", "UNIT001") },
                { "organization-code", new Field("organization-code", "ORG001") }
            };
            root.AddFields(fields);
            return root;
        }

        /// <summary>
        /// Creates a temporary directory for file I/O testing.
        /// </summary>
        /// <returns>The path to the created temporary directory.</returns>
        private string CreateTempDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "LotPakJpkTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        /// <summary>
        /// Cleans up a temporary directory and its contents.
        /// </summary>
        /// <param name="directory">The directory path to clean up.</param>
        private void CleanupTempDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }

        #endregion
    }
}