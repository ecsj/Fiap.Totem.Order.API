using API.Controllers;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Controllers
{
    [TestClass]

    public class OrderControllerTests
    {
        [TestMethod]
        public void GetOrders_ReturnsOk()
        {
            // Arrange
            var mockRepo = new Mock<IOrderUseCase>();
            mockRepo.Setup(repo => repo.Get())
                .Returns(It.IsAny<IQueryable<Order>>());
            var controller = new OrderController(mockRepo.Object);

            // Act
            var result = controller.Get();

            result.Should().BeOfType<OkObjectResult>().Subject.Should();
        }

    }
}
