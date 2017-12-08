
using System;
using System.Collections.Generic;
using System.Net.Http;
using Moq;
using pan_pets.api.Controllers;
using pan_pets.core;
using Xunit;
using Newtonsoft.Json;
using System.Text;

namespace pan_pets.api.tests {
    public class PetsApiIntegrationTests : IDisposable {

        // These are here to make sure the routes are configured as expected and return good responses.

        public PetsApiIntegrationTests(){
            Program.StartAsync(new string[] {}); // Start Kestrel
        }

        public void Dispose() {
            Program.Stop();
        }

        [Fact]
        public async void GetShouldReturnAllPets () {
            var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5000/api/Pets");
            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            
            Assert.NotEmpty(jsonString);
            var list = JsonConvert.DeserializeObject<List<object>>(jsonString);
            Assert.True(list.Count > 0);
        }

        [Fact]
        public async void GetWithIdShouldReturnSpecificPet () {
            var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5000/api/Pets/0aea7d8d-9fe2-4180-ae60-21d89e22b36d");
            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            
            Assert.NotEmpty(jsonString);
            var pet = JsonConvert.DeserializeObject<Pet>(jsonString);
            Assert.Equal("Wendell", pet.Name);
        }

        [Fact]
        public async void PostWithValidPetReturnGoodResult () {
            var client = new HttpClient();
            var newPet = new Pet { Type = PetType.Pig, Name="Esther" };
            var response = await client.PostAsync("http://localhost:5000/api/Pets", new StringContent(JsonConvert.SerializeObject(newPet), Encoding.UTF8, "application/json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            Assert.NotEmpty(jsonString);
            var result = JsonConvert.DeserializeObject<ValidationResult>(jsonString);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async void PutWithValidPetReturnGoodResult () {
            var client = new HttpClient();
            var newPet = new Pet { Id = "0aea7d8d-9fe2-4180-ae60-21d89e22b36d", Type = PetType.Pig, Name="Wendell Updated" };
            var response = await client.PutAsync("http://localhost:5000/api/Pets/0aea7d8d-9fe2-4180-ae60-21d89e22b36d", new StringContent(JsonConvert.SerializeObject(newPet), Encoding.UTF8, "application/json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            Assert.NotEmpty(jsonString);
            var result = JsonConvert.DeserializeObject<ValidationResult>(jsonString);
            Assert.True(result.IsValid);
        }
    }
}