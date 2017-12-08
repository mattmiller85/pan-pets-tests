
using System;
using System.Collections.Generic;
using Moq;
using pan_pets.api.Controllers;
using pan_pets.core;
using Xunit;

namespace pan_pets.api.tests {
    public class InMemoryPetsRepositoryTests {
        
        [Fact]
        public void NewInstanceShouldSeedPetDataWith6Pets () {
            var repo = new InMemoryPetsRepository();
            Assert.Equal(6, repo.GetAll().Count);
        }

        [Fact]
        public void GetWithIdNotInRepoShouldReturnThrowKeyNotFound () {
            var repo = new InMemoryPetsRepository();
            Assert.Throws<PetNotFoundException>(() => repo.Get("id-not-in-repo"));
        }

        [Fact]
        public void GetWithValidIdShouldReturnExpectedPet () {
            var repo = new InMemoryPetsRepository();
            Assert.Equal("Bert", repo.Get("736dd077-f65f-47bb-ad5e-7677b4f4bf56").Name);
        }

        [Fact]
        public void SaveShouldValidatePet () {
            var repo = new InMemoryPetsRepository();
            var result = repo.Save(new Pet());
            Assert.False(result.IsValid);
            Assert.Null(result.PetId);
        }

        [Fact]
        public void SaveShouldSaveValidPet () {
            var guid = "guid-here-ei3jkd03-323";
            var repo = new InMemoryPetsRepository();
            var result = repo.Save(new Pet { Id = guid, Type = PetType.Bird, Name = "Polly" });
        
            Assert.True(result.IsValid);
            Assert.NotNull(result.PetId);
            Assert.NotNull(repo.Get(guid));
        }

        [Fact]
        public void ShouldReturnFalseForHasIdWithGuidNotInRepo() {
            var repo = new InMemoryPetsRepository();
            Assert.False(repo.HasId("inval-id-guid-222-3cxd"));
        }

        [Fact]
        public void ShouldReturnTrueForHasIdWithGuidInRepo() {
            var repo = new InMemoryPetsRepository();
            Assert.True(repo.HasId("736dd077-f65f-47bb-ad5e-7677b4f4bf56"));
        }
    }
}