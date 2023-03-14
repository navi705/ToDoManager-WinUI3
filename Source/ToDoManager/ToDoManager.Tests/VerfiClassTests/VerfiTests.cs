using ToDoManager.HelpClasses;
using ToDoManager.HelpClasses.Verify;
using Xunit;

namespace ToDoManager.Tests.VerfiClassTests
{
    public class VerfiTests
    {
        //I don't think this test needs to be in production. This class is very simple.
        // AAA Test format Arrange-Act-Assert
        // Fact is a separate unit test that accepts no parameters.
        [Fact]
        public void IsEmail_ReturnTrue() 
        {
            //Arrange - sets the initial conditions for running the test
            string testEmail = "TestEmail@test.test";
            // Act - runs a test
            bool answer = Verfy.IsEmail(testEmail);
            //Assert - verifies the test result
            Assert.True(answer);
        }

        //Theory is a test that accepts parameters, and there can be several scenarios.
        [Theory]
        [InlineData("@test.test")]
        public void IsEmail_ReturnFalse(string email)
        {
            //Arrange none we have inline data
            //Act
            bool answer = Verfy.IsEmail(email);
            //Assert
            Assert.False(answer);
        }

        [Theory]
        [InlineData("123456","123456")]
        [InlineData("aboba1","aboba1")]
        public void IsPasswordsMatch_ReturnTrue(string password, string confirmPassword)
        {
            bool answer = Verfy.IsPasswordsMatch(password, confirmPassword);
            Assert.True(answer);
        }

        [Theory]
        [InlineData("12345", "1234")]
        [InlineData("aboba", "abob1")]
        public void IsPasswordsMatch_ReturnFalse(string password, string confirmPassword)
        {
            bool answer = Verfy.IsPasswordsMatch(password, confirmPassword);
            Assert.False(answer);
        }

        [Theory]
        [InlineData("123456")]
        public void IsPasswordValid_ReturnTrue(string password)
        {
            bool answer = Verfy.IsPasswordValid(password);
            Assert.True(answer);
        }

        [Theory]
        [InlineData("12346")]
        public void IsPasswordValid_ReturnFalse(string password)
        {
            bool answer = Verfy.IsPasswordValid(password);
            Assert.False(answer);
        }

        [Theory]
        [InlineData("sfsa@asd.sad","123456","123456")]
        public void FieldsIsValid_ReturnTrue(string email,string password, string confirmPassword)
        {
            string answer = Verfy.FieldsIsValid(email,password,confirmPassword);
            Assert.Equal("",answer);
        }
    }
}