using Microsoft.VisualStudio.TestTools.UnitTesting;
using KillerDex.Core.Models;
using KillerDex.Core.Validators;
using KillerDex.Tests.Mocks;

namespace KillerDex.Tests
{
    [TestClass]
    public class AllyValidatorTests
    {
        private MockAllyRepository _repository;
        private AllyValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _repository = new MockAllyRepository();
            _validator = new AllyValidator(_repository);
        }

        #region Name Required Tests

        [TestMethod]
        public void ValidateForCreate_NullName_ReturnsError()
        {
            // Arrange
            var ally = new Ally { Name = null };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "Name is required.");
        }

        [TestMethod]
        public void ValidateForCreate_EmptyName_ReturnsError()
        {
            // Arrange
            var ally = new Ally { Name = "" };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "Name is required.");
        }

        [TestMethod]
        public void ValidateForCreate_WhitespaceOnlyName_ReturnsError()
        {
            // Arrange
            var ally = new Ally { Name = "   " };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "Name is required.");
        }

        [TestMethod]
        public void ValidateForCreate_ValidName_ReturnsSuccess()
        {
            // Arrange
            var ally = new Ally { Name = "Marco" };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        #endregion

        #region Duplicate Name Tests

        [TestMethod]
        public void ValidateForCreate_DuplicateName_ReturnsError()
        {
            // Arrange
            _repository.SeedData(new Ally { Name = "Marco" });
            var ally = new Ally { Name = "Marco" };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "An ally with this name already exists.");
        }

        [TestMethod]
        public void ValidateForCreate_DuplicateNameCaseInsensitive_ReturnsError()
        {
            // Arrange
            _repository.SeedData(new Ally { Name = "Marco" });
            var ally = new Ally { Name = "MARCO" };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "An ally with this name already exists.");
        }

        [TestMethod]
        public void ValidateForCreate_DuplicateNameWithSpaces_ReturnsError()
        {
            // Arrange
            _repository.SeedData(new Ally { Name = "Marco" });
            var ally = new Ally { Name = "  Marco  " };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "An ally with this name already exists.");
        }

        [TestMethod]
        public void ValidateForCreate_UniqueName_ReturnsSuccess()
        {
            // Arrange
            _repository.SeedData(new Ally { Name = "Marco" });
            var ally = new Ally { Name = "Luca" };

            // Act
            var result = _validator.ValidateForCreate(ally);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        #endregion

        #region Update Tests

        [TestMethod]
        public void ValidateForUpdate_SameNameSameAlly_ReturnsSuccess()
        {
            // Arrange
            var existingAlly = new Ally { Name = "Marco" };
            _repository.SeedData(existingAlly);

            // Act
            var result = _validator.ValidateForUpdate(existingAlly);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ValidateForUpdate_DifferentAllyWithSameName_ReturnsError()
        {
            // Arrange
            var existingAlly = new Ally { Name = "Marco" };
            var allyToUpdate = new Ally { Name = "Luca" };
            _repository.SeedData(existingAlly, allyToUpdate);

            // Try to rename Luca to Marco (which already exists)
            allyToUpdate.Name = "Marco";

            // Act
            var result = _validator.ValidateForUpdate(allyToUpdate);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "An ally with this name already exists.");
        }

        [TestMethod]
        public void ValidateForUpdate_EmptyName_ReturnsError()
        {
            // Arrange
            var ally = new Ally { Name = "" };

            // Act
            var result = _validator.ValidateForUpdate(ally);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.Contains(result.Errors, "Name is required.");
        }

        [TestMethod]
        public void ValidateForUpdate_UniqueNewName_ReturnsSuccess()
        {
            // Arrange
            var existingAlly = new Ally { Name = "Marco" };
            var allyToUpdate = new Ally { Name = "Luca" };
            _repository.SeedData(existingAlly, allyToUpdate);

            // Rename Luca to Giovanni (unique name)
            allyToUpdate.Name = "Giovanni";

            // Act
            var result = _validator.ValidateForUpdate(allyToUpdate);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        #endregion
    }
}