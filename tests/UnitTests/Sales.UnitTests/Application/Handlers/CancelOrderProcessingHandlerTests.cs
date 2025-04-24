using Moq;
using Moq.AutoMock;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessing;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Notifications;
using static SalesSystem.Sales.Domain.Entities.Order;

namespace Sales.UnitTests.Application.Handlers
{
    public class CancelOrderProcessingHandlerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificator> _notificatorMock;

        public CancelOrderProcessingHandlerTests()
        {
            _orderRepositoryMock = _mocker.GetMock<IOrderRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _notificatorMock = _mocker.GetMock<INotificator>();
        }

        [Fact(DisplayName = "Cancel Processing Order With Success")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingHandler_ExecuteAsync_ShouldCancelProcessingOrderWithSuccess()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new CancelOrderProcessingCommand(orderId, customerId);

            var order = GetValidNewDraftOrder(Guid.NewGuid()); 
            order.StartOrder(); 

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
                                .ReturnsAsync(order);

            _unitOfWorkMock.Setup(u => u.CommitAsync())
                           .ReturnsAsync(true);

            _orderRepositoryMock.Setup(r => r.UnitOfWork)
                                .Returns(_unitOfWorkMock.Object);

            var handler = new CancelOrderProcessingHandler(_orderRepositoryMock.Object, _notificatorMock.Object);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Cancel Order - Order Not Found")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingHandler_ExecuteAsync_ShouldFailWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new CancelOrderProcessingCommand(orderId, customerId);

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
                                .ReturnsAsync((Order)null!);

            var handler = new CancelOrderProcessingHandler(_orderRepositoryMock.Object, _notificatorMock.Object);
            var notifications = new List<Notification>();
            _notificatorMock.Setup(n => n.HandleNotification(It.IsAny<Notification>()))
                .Callback<Notification>(notifications.Add);

            _notificatorMock.Setup(n => n.GetNotifications())
                 .Returns(() => notifications);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors ?? []);
            _orderRepositoryMock.Verify(r => r.Update(It.IsAny<Order>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Cancel Order - Validation Fails")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingHandler_ExecuteAsync_ShouldFailWhenValidationFails()
        {
            // Arrange
            var command = new CancelOrderProcessingCommand(Guid.Empty, Guid.Empty);
            var handler = new CancelOrderProcessingHandler(_orderRepositoryMock.Object, _notificatorMock.Object);
            var notifications = new List<Notification>();
            _notificatorMock.Setup(n => n.HandleNotification(It.IsAny<Notification>()))
                .Callback<Notification>(notifications.Add);

            _notificatorMock.Setup(n => n.GetNotifications())
                 .Returns(() => notifications);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors ?? []);
            _orderRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact(DisplayName = "Cancel Order - Commit Fails")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task CancelOrderProcessingHandler_ExecuteAsync_ShouldFailWhenCommitFails()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new CancelOrderProcessingCommand(orderId, customerId);
            var order = GetValidNewDraftOrder(customerId);

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
                                .ReturnsAsync(order);

            _orderRepositoryMock.Setup(r => r.UnitOfWork)
                                .Returns(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.CommitAsync())
                           .ReturnsAsync(false); 

            var handler = new CancelOrderProcessingHandler(_orderRepositoryMock.Object, _notificatorMock.Object);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            _orderRepositoryMock.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        private Order GetValidNewDraftOrder(Guid customerId)
             => OrderFactory.NewDraftOrder(customerId);
    }
}
