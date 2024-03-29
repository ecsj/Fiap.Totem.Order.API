using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;
[TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void ChangeStatus_Should_Update_Order_Status()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderProduct> { new OrderProduct() { Quantity = 1 }, new OrderProduct() { Quantity = 1 } }, OrderStatus.Pending, 100);

            // Act
            order.ChangeStatus(OrderStatus.Completed);

            // Assert
            Assert.AreEqual(OrderStatus.Completed, order.Status);
        }

        [TestMethod]
        public void UpdatePayment_Should_Update_Payment_Status_And_QRCode()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderProduct> { new OrderProduct() { Quantity = 1 }, new OrderProduct() { Quantity = 1 } }, OrderStatus.Pending, 100);
            var paymentStatus = PaymentStatus.Approved;
            var qrCode = "123456789";

            // Act
            order.UpdatePayment(paymentStatus, qrCode);

            // Assert
            Assert.AreEqual(paymentStatus, order.Payment.Status);
            Assert.AreEqual(qrCode, order.Payment.QrCode);
        }

        [TestMethod]
        public void FromOrderRequest_Should_Create_Order_From_OrderRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                ClientId = Guid.NewGuid(),
                Products = new List<OrderProductsRequest>
                {
                    new OrderProductsRequest
                    {
                        ProductId = "1",
                        Quantity = 1,
                        Total = 10,
                        Comments = ""
                        
                    }
                },
                Total = 25
            };

            // Act
            var order = Order.FromOrderRequest(orderRequest);

            // Assert
            Assert.AreEqual(orderRequest.ClientId, order.ClientId);
            Assert.AreEqual(orderRequest.Products.Count, order.Products.Count);
            // Add more assertions to verify the correctness of the created order
        }

        [TestMethod]
        public void Validate_Should_Throw_Exception_When_Products_Is_Null()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => new Order(Guid.NewGuid(), new List<OrderProduct> { new OrderProduct() { Quantity = 0 }, new OrderProduct() { Quantity = 1 } }, OrderStatus.Pending, 100));
        }

        [TestMethod]
        public void Validate_Should_Throw_Exception_When_Product_Quantity_Is_Zero()
        {
            // Arrange
            var products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = "1",
                    Quantity = 0,
                    Total = 10
                }
            };
            // Act & Assert
            Assert.ThrowsException<Exception>(() => new Order(Guid.NewGuid(), products, OrderStatus.Pending, 100));
        }

        [TestMethod]
        public void Validate_Should_Throw_Exception_When_TotalPrice_Is_Zero()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => new Order(Guid.NewGuid(), new List<OrderProduct>(), OrderStatus.Pending, 0));
        }
    }