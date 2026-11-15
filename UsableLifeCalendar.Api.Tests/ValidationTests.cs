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
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            List<ValidationResult> results = Validate(request);

            Assert.Empty(results);
        }

        [Fact]
        public void CreateUserRequest_WithEmptyFirstName_FailsValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = "",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithFirstNameTooLong_FailsValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = new string('A', 26),
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!"
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithLogonNameTooShort_FailsValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "abc",
                Password = "Password1!"
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithLogonNameContainingSpecialCharacters_FailsValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith!",
                Password = "Password1!"
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithPasswordTooShort_FailsValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "abc"
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserRequest_WithNoMiddleName_PassesValidation()
        {
            CreateUserRequest request = new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                Password = "Password1!",
                MiddleName = null
            };

            List<ValidationResult> results = Validate(request);

            Assert.Empty(results);
        }

        [Fact]
        public void CreateEventRequest_WithValidData_PassesValidation()
        {
            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = "Main Gym",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            List<ValidationResult> results = Validate(request);

            Assert.Empty(results);
        }

        [Fact]
        public void CreateEventRequest_WithEmptyTitle_FailsValidation()
        {
            CreateEventRequest request = new()
            {
                Title = "",
                Description = "Games and food.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            List<ValidationResult> results = Validate(request);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateEventRequest_WithNoLocation_PassesValidation()
        {
            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = null,
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            List<ValidationResult> results = Validate(request);

            Assert.Empty(results);
        }
    }
}
