using Bogus;
using Moq.AutoMock;
using Moq;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock;
using static SalesSystem.Sales.Domain.Entities.Order;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Enums;
using System.Net;

namespace Sales.UnitTests.Application.Handlers
{
    public class CancelOrderProcessingReverseStockHandlerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificator> _notificatorMock;

        public CancelOrderProcessingReverseStockHandlerTests()
        {
            _orderRepositoryMock = _mocker.GetMock<IOrderRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _notificatorMock = _mocker.GetMock<INotificator>();

        }

        [Fact(DisplayName = "Cancel Order Processing Reverse Stock Handler")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingReverseStockHandler_ExecuteAsync_ShouldCancelOrderProcessingReverseStockWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = GetValidNewDraftOrder(Guid.NewGuid());
            var command = new CancelOrderProcessingReverseStockCommand(order.Id, customerId);

            order.StartOrder();

            _orderRepositoryMock
                .Setup(m => m.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync())
                .ReturnsAsync(true);

            _orderRepositoryMock.Setup(x => x.UnitOfWork)
                              .Returns(_unitOfWorkMock.Object);

            var cancelOrderProcessingReverseStockHandler = new CancelOrderProcessingReverseStockHandler(_orderRepositoryMock.Object, _notificatorMock.Object);

            // Act
            var result = await cancelOrderProcessingReverseStockHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            Assert.Equal(EOrderStatus.Draft, order.Status);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Cancel Order Processing Reverse Stock Handler - Order Not Found")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingReverseStockHandler_ExecuteAsync_ShouldReturnNotFoundWhenOrderNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = new CancelOrderProcessingReverseStockCommand(Guid.NewGuid(), customerId);
            
            _orderRepositoryMock
                .Setup(m => m.GetByIdAsync(command.OrderId))
                .ReturnsAsync((Order?)null);

            var notifications = new List<Notification>();
            _notificatorMock.Setup(n => n.HandleNotification(It.IsAny<Notification>()))
                .Callback<Notification>(notifications.Add);

            _notificatorMock.Setup(n => n.GetNotifications())
                .Returns(() => notifications);

            var cancelOrderProcessingReverseStockHandler = new CancelOrderProcessingReverseStockHandler(_orderRepositoryMock.Object, _notificatorMock.Object);
            
            // Act
            var result = await cancelOrderProcessingReverseStockHandler.ExecuteAsync(command, CancellationToken.None);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors ?? []);
            Assert.Equal((int)HttpStatusCode.NotFound, result.Code);
            _unitOfWorkMock.Verify(m => m.CommitAsync(), Times.Never);
        }

        private Order GetValidNewDraftOrder(Guid customerId)
             => OrderFactory.NewDraftOrder(customerId);
    }
}
