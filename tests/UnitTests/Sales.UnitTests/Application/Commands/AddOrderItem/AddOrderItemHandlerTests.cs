using Bogus;
using MidR.Interfaces;
using Moq;
using Moq.AutoMock;
using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Notifications;
using static SalesSystem.Sales.Domain.Entities.Order;

namespace Sales.UnitTests.Application.Commands.AddOrderItem
{
    public class AddOrderItemHandlerTests
    {
        private readonly Faker _faker = new();
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificator> _notificatorMock;

        public AddOrderItemHandlerTests()
        {
            _orderRepositoryMock = _mocker.GetMock<IOrderRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _notificatorMock = _mocker.GetMock<INotificator>();
        }

        [Fact(DisplayName = "Add Order Item Draft Order With Success")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task AddOrderItemHandler_ExecuteAsync_ShouldAddItemToDraftOrderWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = CreateValidAddOrderItemCommand(customerId);

            var order = GetValidNewDraftOrder(customerId);

            _orderRepositoryMock
                .Setup(m => m.GetDraftOrderByCustomerIdAsync(customerId))
                .ReturnsAsync(order);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync())
                .ReturnsAsync(true);

            _orderRepositoryMock.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            var addOrderItemHandler = new AddOrderItemHandler(_notificatorMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await addOrderItemHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.AddOrderItem(It.IsAny<OrderItem>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add Order Item to New Order With Success")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task AddOrderItemHandler_ExecuteAsync_ShouldAddItemToNewOrderWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = CreateValidAddOrderItemCommand(customerId);

            _orderRepositoryMock
                .Setup(m => m.GetDraftOrderByCustomerIdAsync(customerId))
                .ReturnsAsync((Order?)null);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync())
                .ReturnsAsync(true);

            _orderRepositoryMock.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            var addOrderItemHandler = new AddOrderItemHandler(_notificatorMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await addOrderItemHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.Create(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add Existent Order Item to Draft Order With Success")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task AddOrderItemHandler_ExecuteAsync_ShouldAddExistentItemToDraftOrderWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var order = GetValidNewDraftOrder(customerId);
            var orderItem = new OrderItem(productId, "Product Test", 1, 10);
            order.AddItem(orderItem);

            var command = new AddOrderItemCommand(productId, orderItem.ProductName, orderItem.Quantity, orderItem.UnitPrice);
            command.SetCustomerId(customerId);

            _orderRepositoryMock
                .Setup(m => m.GetDraftOrderByCustomerIdAsync(customerId))
                .ReturnsAsync(order);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync())
                .ReturnsAsync(true);

            _orderRepositoryMock.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            var addOrderItemHandler = new AddOrderItemHandler(_notificatorMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await addOrderItemHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Invalid AddOrderItemCommand Should Fail")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task AddOrderItemHandler_ExecuteAsync_ShouldFailIfAddOrderItemCommandIsInvalid()
        {
            // Arrange
            var command = new AddOrderItemCommand(Guid.Empty, string.Empty, 0, 0);

            var addOrderItemHandler = new AddOrderItemHandler(_notificatorMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await addOrderItemHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert   
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors ?? []);
            Assert.Equal(5, result.Errors?.Count);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Never);
        }

        private AddOrderItemCommand CreateValidAddOrderItemCommand(Guid customerId)
        {
            var command = new AddOrderItemCommand(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(1, 5), _faker.Random.Decimal(1, 100));
            command.SetCustomerId(customerId);
            return command;
        }

        private Order GetValidNewDraftOrder(Guid customerId)
             => OrderFactory.NewDraftOrder(customerId);
    }
}
