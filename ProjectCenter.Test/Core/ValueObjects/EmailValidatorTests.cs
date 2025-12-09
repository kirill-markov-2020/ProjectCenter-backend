using Xunit;
using ProjectCenter.Core.ValueObjects;

namespace ProjectCenter.Tests.Unit.Core.ValueObjects
{
    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("student@college.ru")]
        [InlineData("teacher@gmail.com")]
        [InlineData("admin@domain.org")]
        public void Validate_ShouldAccept_ValidEmailAddresses(string validEmail)
        {
            var validationErrors = EmailValidator.Validate(validEmail);
            Assert.Empty(validationErrors);
        }

        [Fact]
        public void Validate_ShouldReject_EmailWithCyrillicCharacters()
        {
            string invalidEmail = "пользователь@почта.рф";
            var validationErrors = EmailValidator.Validate(invalidEmail);
            Assert.Contains("Email должен содержать только латинские символы.",
                validationErrors);
        }
    }
}