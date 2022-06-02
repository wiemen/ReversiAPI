using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ReversiRestApi.Controllers;
using ReversiRestApi.Models;
using ReversiRestApi.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRestApi_Test.Controllers
{
    [TestFixture]
    public class SpelControllerTests
    {
        private readonly SpelController _spelController;

        public SpelControllerTests()
        {
            //var spelRepository = new SpelRepository();
            //_spelController = new SpelController(spelRepository);
        }

        [Test]
        public void GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler_WhenThereAreAwaitingPlayers_ReturnListWithDescription()
        {
            // Arrange

            // Act
            var result = _spelController.GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            var okObject = result.Result as OkObjectResult;

            Assert.IsNotNull(okObject.Value);
            var list = okObject.Value as IEnumerable<string>;

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() > 0);
        }

        [Test]
        public void GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler_WhenThereAreNoAwaitingPlayers_ReturnEmptyList()
        {
            //// Arrange
            //var repo = new SpelRepository();
            //repo.Spellen = new List<Spel>();
            //var controller = new SpelController(repo);

            //// Act
            //var result = controller.GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler();

            //// Assert
            //Assert.IsNotNull(result);
            //Assert.IsNotNull(result.Result);
            //var okObject = result.Result as OkObjectResult;

            //Assert.IsNotNull(okObject.Value);
            //var list = okObject.Value as IEnumerable<string>;

            //Assert.IsNotNull(list);
            //Assert.IsTrue(list.Count() == 0);
        }

        [Test]
        public void Create_WhenCorrectDTO_ReturnSpelList()
        {
            // Arrange
            var request = new SpelCreateDTO
            {
                Speler1Token = "token",
                Omschrijving = "Ik spel wil hebben"
            };

            // Act
            var result = _spelController.Create(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetByToken_WhenTokenExist_ReturnSpel()
        {
            // Arrange
            const string request = "spelToken";

            // Act
            var result = _spelController.GetByToken(request);

            // Assert
            Assert.IsNotNull(result);
            var okObject = result as OkObjectResult;

            Assert.IsNotNull(okObject.Value);
            var spel = okObject.Value as string;

            Assert.IsFalse(string.IsNullOrEmpty(spel));
            Assert.IsTrue(spel.Contains(request));
        }

        [Test]
        public void GetByToken_WhenTokenNotExist_ReturnEmpty()
        {
            // Arrange
            const string request = "hiu";

            // Act
            var result = _spelController.GetByToken(request);

            // Assert
            Assert.IsNotNull(result);
            var badRequest = result as BadRequestResult;

            Assert.IsNotNull(badRequest);
        }

        [Test]
        public void SpelSpeler_WhenTokenExist_ReturnSpel()
        {
            // Arrange
            const string request = "ghijkl";

            // Act
            var result = _spelController.SpelSpeler(request);

            // Assert
            Assert.IsNotNull(result);
            var okObject = result as OkObjectResult;

            Assert.IsNotNull(okObject.Value);
            var spel = okObject.Value as string;

            Assert.IsFalse(string.IsNullOrEmpty(spel));
            Assert.IsTrue(spel.Contains(request));
        }

        [Test]
        public void SpelSpeler_WhenTokenNotExist_ReturnEmpty()
        {
            // Arrange
            const string request = "hiu";

            // Act
            var result = _spelController.SpelSpeler(request);

            // Assert
            Assert.IsNotNull(result);
            var badRequest = result as BadRequestResult;

            Assert.IsNotNull(badRequest);
        }
    }
}
