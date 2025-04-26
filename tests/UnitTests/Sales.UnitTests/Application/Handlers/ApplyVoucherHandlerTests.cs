using Bogus;
using Moq;
using Moq.AutoMock;
using SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Enums;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Notifications;
using System.Net;
using static SalesSystem.Sales.Domain.Entities.Order;

namespace Sales.UnitTests.Application.Handlers
{
    public class ApplyVoucherHandlerTests
    {
        private readonly Faker _faker = new();
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificator> _notificatorMock;
        public ApplyVoucherHandlerTests()
        {
            _orderRepositoryMock = _mocker.GetMock<IOrderRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _notificatorMock = _mocker.GetMock<INotificator>();
        }

        [Fact(DisplayName = "Apply Voucher to Order With Success")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task ApplyVoucherHandler_ExecuteAsync_ShouldApplyVoucherToOrderWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var voucher = GetValidValueVoucher();
            var order = GetValidNewDraftOrder(customerId);
            var command = new ApplyVoucherCommand(voucher.Code);
            command.SetCustomerId(customerId);

            order.AddItem(GetValidOrderItem());

            order.ApplyVoucher(voucher);

            SetUpVoucherRetrieval(voucher, command);

            var applyVoucherHandler = SetUpApplyVoucherHandlerSuccess(customerId, order);

            // Act
            var result = await applyVoucherHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
            _orderRepositoryMock.Verify(m => m.GetVoucherByCodeAsync(voucher.Code), Times.Once);
        }


        [Fact(DisplayName = "Apply Voucher to Order Should Fail When Voucher Does not Exists")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task ApplyVoucherHandler_ExecuteAsync_ShouldFailWhenVoucherDoesNotExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var voucher = GetValidValueVoucher();
            var order = GetValidNewDraftOrder(customerId);
            var command = new ApplyVoucherCommand(voucher.Code);
            command.SetCustomerId(customerId);

            order.AddItem(GetValidOrderItem());

            order.ApplyVoucher(voucher);

            _orderRepositoryMock
                .Setup(m => m.GetVoucherByCodeAsync(command.VoucherCode))
                .ReturnsAsync((Voucher?)null);

            SetUpNotificationErrors();

            var applyVoucherHandler = SetUpApplyVoucherHandlerSuccess(customerId, order);

            // Act
            var result = await applyVoucherHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.Code);
            Assert.NotEmpty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Never);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Never);
            _orderRepositoryMock.Verify(m => m.GetVoucherByCodeAsync(voucher.Code), Times.Once);
        }


        [Fact(DisplayName = "Apply Voucher to Order Should Update Order Price")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task ApplyVoucherHandler_ExecuteAsync_ShouldUpdateOrderPrice()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var voucher = GetValidValueVoucher();
            var order = GetValidNewDraftOrder(customerId);
            var command = new ApplyVoucherCommand(voucher.Code);
            var orderItem = GetValidOrderItem();
            command.SetCustomerId(customerId);

            order.AddItem(orderItem);
            var orderPriceBeforeVoucher = orderItem.CalculatePrice();

            SetUpVoucherRetrieval(voucher, command);

            var applyVoucherHandler = SetUpApplyVoucherHandlerSuccess(customerId, order);

            // Act
            var result = await applyVoucherHandler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.Equal(orderPriceBeforeVoucher - voucher.Value, order.Price);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
            _orderRepositoryMock.Verify(m => m.GetVoucherByCodeAsync(voucher.Code), Times.Once);
        }

        [Fact(DisplayName = "Apply Voucher to Order Should Update Order Price")]
        [Trait("Sales Application", "Handlers Tests")]
        public async Task ApplyVoucherHandler_ExecuteAsync_ShouldResetOrderPriceWhenDiscountIsBiggerThanOrderPrice()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var voucher = new Voucher("TEST", null, 100, 100, EVoucherType.Value, DateTime.UtcNow.AddMonths(10));
            var order = GetValidNewDraftOrder(customerId);

            var command = new ApplyVoucherCommand(voucher.Code);
            command.SetCustomerId(customerId);

            var orderItem = new OrderItem(Guid.NewGuid(), "TESTE", 5, 10);
            order.AddItem(orderItem);

            var priceBeforeVoucher = orderItem.CalculatePrice();

            SetUpVoucherRetrieval(voucher, command);

            var applyVoucherHandler = SetUpApplyVoucherHandlerSuccess(customerId, order);

            // Assert
            var result = await applyVoucherHandler.ExecuteAsync(command, CancellationToken.None);

            // Act
            Assert.Equal(0, order.Price);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
            _orderRepositoryMock.Verify(m => m.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.UnitOfWork.CommitAsync(), Times.Once);
            _orderRepositoryMock.Verify(m => m.GetVoucherByCodeAsync(voucher.Code), Times.Once);
        }

        #region Private Methods

        private Order GetValidNewDraftOrder(Guid customerId)
            => OrderFactory.NewDraftOrder(customerId);

        private Voucher GetValidValueVoucher()
            => new(Guid.NewGuid().ToString("N"), null, 150, 100, EVoucherType.Value, DateTime.Now.AddDays(1));

        private OrderItem GetValidOrderItem()
            => new(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(MIN_ITEM_QUANTITY, MAX_ITEM_QUANTITY), _faker.Random.Decimal(1, 1000));

        private ApplyVoucherHandler SetUpApplyVoucherHandlerSuccess(Guid customerId, Order order)
        {
            _orderRepositoryMock
               .Setup(m => m.GetDraftOrderByCustomerIdAsync(customerId))
               .ReturnsAsync(order);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync())
                .ReturnsAsync(true);

            _orderRepositoryMock.Setup(x => x.UnitOfWork)
                   .Returns(_unitOfWorkMock.Object);

            return new ApplyVoucherHandler(_orderRepositoryMock.Object, _notificatorMock.Object);
        }
        private void SetUpVoucherRetrieval(Voucher voucher, ApplyVoucherCommand command)
        {
            _orderRepositoryMock
                .Setup(m => m.GetVoucherByCodeAsync(command.VoucherCode))
                .ReturnsAsync(voucher);
        }
        private void SetUpNotificationErrors()
        {
            var notifications = new List<Notification>();
            _notificatorMock.Setup(n => n.HandleNotification(It.IsAny<Notification>()))
                .Callback<Notification>(notifications.Add);

            _notificatorMock.Setup(n => n.GetNotifications())
                .Returns(() => notifications);
        }
        #endregion
    }
}
