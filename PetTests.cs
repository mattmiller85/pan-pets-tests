
using System;
using System.Collections.Generic;
using Moq;
using pan_pets.api.Controllers;
using pan_pets.core;
using Xunit;

namespace pan_pets.core.tests {
    public class PetTests {
        [Fact]
        public void ShouldBeInvalidPetIfNoType () {
            var result = new Pet().Validate();
            Assert.False(result.IsValid);
            Assert.Contains("Pet type not specified.", result.Messages);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldBeInvalidPetIfNoName (string invalidName) {
            var result = new Pet { Name = invalidName }.Validate();
            Assert.False(result.IsValid);
            Assert.Contains("Name cannot be empty.", result.Messages);
        }

        [Fact]
        public void ShouldSetPetIdIfValid () {
            var petId = "xxxx-1234";
            var result = new Pet { Id= petId, Name = "Test Name", Type = PetType.Pig }.Validate();
            Assert.Equal(petId, result.PetId);
            Assert.True(result.IsValid);
            Assert.Empty(result.Messages);
        }
    }
}