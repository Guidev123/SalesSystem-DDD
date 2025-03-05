using FluentValidation.Results;
using MediatR;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductHandler(IProductRepository productRepository,
                                             INotificator notificator)
                                           : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Response<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validate = request.Validate(request);
            if (!validate.IsValid)
            {
                GetValidationErrors(validate);
                return Response<CreateProductResponse>.Success(null);
            }

            var product = request.MapToEntity();
            _productRepository.Create(product);

            if(!await _productRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<CreateProductResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<CreateProductResponse>.Success(new(product.Id));
        }

        private void GetValidationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                _notificator.HandleNotification(new(error.ErrorMessage));
        }
    }
}
