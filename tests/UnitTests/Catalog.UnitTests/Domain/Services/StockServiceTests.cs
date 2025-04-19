using Bogus;
using Moq;
using Moq.AutoMock;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.Catalog.Domain.Services;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.DTOs;
using SalesSystem.SharedKernel.Events;

namespace Catalog.UnitTests.Domain.Services
{
    public class StockServiceTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediatorHandler> _mediator;

        public StockServiceTests()
        {
            _productRepository = _mocker.GetMock<IProductRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _mediator = _mocker.GetMock<IMediatorHandler>();
        }

        [Fact(DisplayName = "Add Stock Single Product With Success")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_AddStockAsync_ShouldReturnTrueIfProductIsValid()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var product = GenerateValidProduct().Generate();

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            var stockServiceObject = new StockService(_productRepository.Object, _mediator.Object);

            // Act
            var result = await stockServiceObject.AddStockAsync(product.Id, 10);

            // Assert
            Assert.True(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(product), Times.Once);
        }

        [Fact(DisplayName = "Add Stock Many Products With Success")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_AddListStockAsync_ShouldReturnTrueIfProductIsValid()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var products = GenerateOrderProductsListDTO();

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(GenerateValidProduct().Generate());

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.AddListStockAsync(products);

            // Assert
            Assert.True(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(5));
            _productRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Exactly(5));
        }

        [Fact(DisplayName = "Add Stock Many Products With Fail")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_AddStockAsync_ShouldReturnFalseIfProductIsNull()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var product = GenerateValidProduct().Generate();

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync((Product?)null);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            var stockServiceObject = new StockService(_productRepository.Object, _mediator.Object);

            // Act
            var result = await stockServiceObject.AddStockAsync(product.Id, 10);

            // Assert
            Assert.False(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(product), Times.Never);
        }

        [Fact(DisplayName = "Add Stock Many Products With Fail")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_AddListStockAsync_ShouldReturnFalseIfProductIsNull()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var products = GenerateOrderProductsListDTO();

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync((Product?)null);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.AddListStockAsync(products);

            // Assert
            Assert.False(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact(DisplayName = "Debit Stock Single Product With Success")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_DebitStockAsync_ShouldReturnTrueIfIsSuccess()
        {
            // Arrange
            var product = GenerateValidProduct().Generate();
            var stockService = _mocker.CreateInstance<StockService>();
            product.AddStock(50);

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.DebitStockAsync(product.Id, 10);

            // Assert
            Assert.True(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(product), Times.Once);
        }

        [Fact(DisplayName = "Debit Stock Many Products With Success")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_DebitListStockAsync_ShouldReturnTrueIfIsSuccess()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var products = GenerateOrderProductsListDTO();

            var product = GenerateValidProduct().Generate();
            product.AddStock(50);

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.DebitListStockAsync(products);

            // Assert
            Assert.True(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(5));
            _productRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Exactly(5));
        }

        [Fact(DisplayName = "Debit Stock Many Products With Fail")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_DebitListStockAsync_ShouldReturnFalseIfQuantityInStockIsLowerThanDebitQuantity()
        {
            // Arrange
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            var products = GenerateOrderProductsListDTO();

            var product = GenerateValidProduct().Generate();

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.DebitListStockAsync(products);

            // Assert
            Assert.False(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact(DisplayName = "Debit Stock Single Product With Fail")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_DebitStockAsync_ShouldReturnFalseIfQuantityInStockIsLowerThanDebitQuantity()
        {
            // Arrange
            var product = GenerateValidProduct().Generate();
            var stockService = _mocker.CreateInstance<StockService>();

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.DebitStockAsync(product.Id, 10);

            // Assert
            Assert.False(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(product), Times.Never);
        }

        [Fact(DisplayName = "Debit Stock Single Product With Success and Publish Notification")]
        [Trait("Catalog Domain Services", "Stock Service Tests")]
        public async Task StockService_DebitStockAsync_ShouldReturnTrueAndPublishNotificationIfQuantityInStockIsLessThan10()
        {
            // Arrange
            var product = GenerateValidProduct().Generate();
            var stockService = _mocker.CreateInstance<StockService>();
            product.AddStock(11);

            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(true);

            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(product);

            _productRepository.Setup(x => x.UnitOfWork)
                             .Returns(_unitOfWorkMock.Object);

            // Act
            var result = await stockService.DebitStockAsync(product.Id, 10);

            // Assert
            Assert.True(result);
            _productRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(x => x.Update(product), Times.Once);
            _mediator.Verify(x => x.PublishEventAsync(It.IsAny<Event>()), Times.Once);
        }

        private static Faker<Product> GenerateValidProduct()
        {
            return new Faker<Product>()
                        .CustomInstantiator(p => new Product(
                                p.Name.FirstName(),
                                p.Lorem.Sentence(),
                                p.Random.Decimal(1, 100),
                                p.Lorem.Sentence(),
                                Guid.NewGuid(),
                                new(p.Random.Int(1, 10), p.Random.Int(1, 10), p.Random.Int(1, 10))
                        ));
        }

        private static Faker<OrderProductsListDto> GenerateOrderProductsListDTO()
        {
            return new Faker<OrderProductsListDto>()
                        .CustomInstantiator(p => new OrderProductsListDto(
                                p.Random.Guid(),
                                GenerateItemDTO().Generate(5)
                        ));
        }

        private static Faker<ItemDto> GenerateItemDTO()
        {
            return new Faker<ItemDto>()
                        .CustomInstantiator(p => new ItemDto(
                                p.Random.Guid(),
                                p.Random.Int(1, 10)
                        ));
        }
    }
}