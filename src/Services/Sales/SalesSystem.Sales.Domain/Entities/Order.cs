using FluentValidation.Results;
using SalesSystem.Sales.Domain.Enums;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public const int MAX_ITEM_QUANTITY = 15;
        public const int MIN_ITEM_QUANTITY = 1;

        private Order(Guid customerId)
        {
            CustomerId = customerId;
            Code = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.Now;
            Validate();
        }

        protected Order() => _orderItems = [];

        public string Code { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherIsUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public EOrderStatus Status { get; private set; }
        public Voucher? Voucher { get; private set; }
        private readonly List<OrderItem> _orderItems = [];
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public void AddItem(OrderItem item)
        {
            AssertionConcern.EnsureNotNull(item, "Order item must be not null.");
            AssertionConcern.EnsureInRange(item.Quantity, MIN_ITEM_QUANTITY, MAX_ITEM_QUANTITY,
                $"Order item quantity must be between {MIN_ITEM_QUANTITY} and {MAX_ITEM_QUANTITY}.");

            item.AssociateOrder(Id);

            if (ItemAlreadyExists(item))
            {
                var itemQuantity = item.Quantity;
                var existentItem = GetExistentItem(item);

                AssertionConcern.EnsureFalse(itemQuantity + existentItem.Quantity > MAX_ITEM_QUANTITY,
                    $"The maximum quantity of items in the order is {MAX_ITEM_QUANTITY}.");

                existentItem.AddUnities(item.Quantity);
                item = existentItem;

                _orderItems.Remove(existentItem);
            }

            item.CalculatePrice();
            _orderItems.Add(item);

            CalculateOrderPrice();
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            AssertionConcern.EnsureNotNull(voucher, "Cannot use a non-existent voucher.");

            var validation = voucher.IsValidToApply();
            if (!validation.IsValid) return validation;

            Voucher = voucher;
            VoucherIsUsed = true;
            CalculateTotalPriceDiscount();

            voucher.DebitVoucherQuantity();

            return validation;
        }

        public void CalculateOrderPrice()
        {
            Price = OrderItems.Sum(x => x.CalculatePrice());
            CalculateTotalPriceDiscount();
        }

        public void CalculateTotalPriceDiscount()
        {
            if (!VoucherIsUsed || Voucher is null) return;
            ApplyDiscount(Voucher.Type, Price, Voucher.Percentual ?? 0, Voucher.Value ?? 0);
        }

        private void ApplyDiscount(EVoucherType voucherType, decimal value, decimal percentualVoucher, decimal valueVoucher)
        {
            switch (voucherType)
            {
                case EVoucherType.Percentual:
                    AssertionConcern.EnsureNotNull(percentualVoucher, "Percentual voucher cannot be null.");
                    AssertionConcern.EnsureGreaterThan(percentualVoucher, 0, "Percentual voucher discount must be greater than 0.");
                    ApplyPercentualVoucher(value, percentualVoucher);
                    return;

                case EVoucherType.Value:
                    AssertionConcern.EnsureNotNull(valueVoucher, "Value voucher cannot be null.");
                    AssertionConcern.EnsureGreaterThan(valueVoucher, 0, "Value voucher discount must be greater than 0.");
                    ApplyValueVoucher(value, valueVoucher);
                    return;
            }
        }

        private void ApplyPercentualVoucher(decimal value, decimal voucherPercentual)
        {
            AssertionConcern.EnsureGreaterThan(value, 0, "Value must be not null.");
            AssertionConcern.EnsureGreaterThan(voucherPercentual, 0, "Voucher Percentual must be not null.");

            var discount = (value * voucherPercentual) / 100;
            value -= discount;

            Price = value < 0 ? 0 : value;
            Discount = discount;
        }

        private void ApplyValueVoucher(decimal value, decimal voucherValue)
        {
            AssertionConcern.EnsureGreaterThan(value, 0, "Value must be not null.");
            AssertionConcern.EnsureGreaterThan(voucherValue, 0, "Voucher Value must be not null.");

            var discount = voucherValue;
            value -= discount;

            Price = value < 0 ? 0 : value;
            Discount = discount;
        }

        public bool ItemAlreadyExists(OrderItem item)
            => _orderItems.Any(x => x.ProductId == item.ProductId);

        public void RemoveItem(OrderItem item)
        {
            item.Validate();

            var existentItem = GetExistentItem(item);
            _orderItems.Remove(existentItem);

            CalculateOrderPrice();
        }

        public void UpdateItem(OrderItem item)
        {
            item.Validate();

            item.AssociateOrder(Id);

            var existentItem = GetExistentItem(item);
            _orderItems.Remove(existentItem);
            _orderItems.Add(item);

            CalculateOrderPrice();
        }

        private OrderItem GetExistentItem(OrderItem item)
            => OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId)
            ?? throw new DomainException("This item does not belong to the order.");

        public void UpdateUnities(OrderItem item, int quantity)
        {
            item.Validate();

            item.UpdateUnities(quantity);
            UpdateItem(item);
        }

        public void PayOrder() => Status = EOrderStatus.Paid;

        public void RefundOrder() => Status = EOrderStatus.Refunded;

        public void StartOrder() => Status = EOrderStatus.Started;

        public void CancelOrder() => Status = EOrderStatus.Canceled;

        public void DraftOrder() => Status = EOrderStatus.Draft;

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Code, "The order code cannot be empty.");
            AssertionConcern.EnsureNotNull(CustomerId, "The customer ID cannot be null.");
            AssertionConcern.EnsureNotNull(CreatedAt, "The order creation date cannot be null.");
            AssertionConcern.EnsureNotNull(Status, "The order status cannot be null.");
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order(customerId);

                order.DraftOrder();

                return order;
            }
        }
    }
}