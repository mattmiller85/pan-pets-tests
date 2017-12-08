
using System;
using System.Collections.Generic;
using Moq;
using pan_pets.api.Controllers;
using pan_pets.core;
using Xunit;

namespace pan_pets.api.tests {
    public class PetsControllerTests {
        [Fact]
        public void GetShouldReturnAllPets () {
            var mockRepo = new Mock<IPetsRepository> ();
            mockRepo.Setup (r => r.GetAll ()).Returns (new List<Pet> { new Pet (), new Pet (), new Pet () });

            Assert.Equal (3, new PetsController (mockRepo.Object).Get ().Count);
        }

        [Fact]
        public void GetWithIdShouldReturnSpecificPet () {
            
            var guidToFind = Guid.NewGuid ().ToString();
            var guidToIgnore = Guid.NewGuid ().ToString();

            var mockRepo = new Mock<IPetsRepository> ();
            mockRepo.Setup (r => r.Get (guidToFind)).Returns (new Pet { Id = guidToFind, Type = PetType.Bird });
            mockRepo.Setup (r => r.Get (guidToIgnore)).Returns (new Pet { Id = guidToIgnore });

            Assert.Equal(PetType.Bird, new PetsController (mockRepo.Object).Get (guidToFind.ToString ()).Type);
        }

        [Fact]
        public void PostingWithNoPetShouldError () {
            var mockRepo = new Mock<IPetsRepository> ();
            Assert.Throws<ArgumentNullException>(() => new PetsController(mockRepo.Object).Post(null));
        }

        [Fact]
        public void PostingShouldResetTheIdToANewGuid () {
            var mockRepo = new Mock<IPetsRepository> ();
            var pet = new Pet{ Type = PetType.Cat, Id = "xxxx" };
            var result = new PetsController(mockRepo.Object).Post(pet);

            Assert.NotEqual("xxxx", pet.Id);
            Assert.True(Guid.TryParse(pet.Id, out var parsed));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void PuttingWithNoIdShouldError (string badId) {
            var mockRepo = new Mock<IPetsRepository> ();
            Assert.Throws<ArgumentNullException>(() => new PetsController(mockRepo.Object).Put(badId, new Pet()));
        }

        [Fact]
        public void PuttingWithIdNotInRepoShouldError () {
            var mockRepo = new Mock<IPetsRepository> ();
            mockRepo.Setup (r => r.HasId (It.IsAny<string>())).Returns (false);
            Assert.Throws<ArgumentException>(() => new PetsController(mockRepo.Object).Put("xxxx", new Pet()));
        }
    }
}