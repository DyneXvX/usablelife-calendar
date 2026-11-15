using System.ComponentModel.DataAnnotations;
using UsableLifeCalendar.Api.DTOs;

namespace UsableLifeCalendar.Api.Tests
{
    public class ValidationTests
    {
        private static List<ValidationResult> Validate(object model)
        {
            List<ValidationResult> results = [];
            ValidationContext context = new(model);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void CreateUserRequest_WithValidData_PassesValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CreateUserRequest_WithEmptyFirstName_FailsValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithFirstNameTooLong_FailsValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = new string('A', 26),
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithLogonNameTooShort_FailsValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "abc",
                Password = "Password1!"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithLogonNameContainingSpecialCharacters_FailsValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith!",
                Password = "Password1!"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithPasswordTooShort_FailsValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "abc"
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithNoMiddleName_PassesValidation()
        {
            // Arrange
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!",
                MiddleName = null
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CreateEventRequest_WithValidData_PassesValidation()
        {
            // Arrange
            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = "Main Gym",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CreateEventRequest_WithEmptyTitle_FailsValidation()
        {
            // Arrange
            CreateEventRequest request = new()
            {
                Title = "",
                Description = "Games and food.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateEventRequest_WithNoLocation_PassesValidation()
        {
            // Arrange
            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = null,
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            // Act
            List<ValidationResult> results = Validate(request);

            // Assert
            Assert.Empty(results);
        }
    }
}