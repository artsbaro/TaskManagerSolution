using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Extensions;

namespace TaskManagerAPI.Tests.Extensions
{
    public class BsonIdExtensionsTests
    {
        [Fact]
        public void IsValidBsonId_Should_Return_True_For_Valid_Id()
        {
            // Arrange
            var validId = "6592008029c8c3e4dc76256c";

            // Act
            var result = validId.IsValidBsonId();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidBsonId_Should_Return_False_For_Invalid_Id()
        {
            // Arrange
            var invalidId = "invalid-id";

            // Act
            var result = invalidId.IsValidBsonId();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidBsonId_Should_Return_False_For_Null_Or_Empty_Id()
        {
            // Act & Assert
            Assert.False(((string)null).IsValidBsonId());
            Assert.False(string.Empty.IsValidBsonId());
        }
    }

}
