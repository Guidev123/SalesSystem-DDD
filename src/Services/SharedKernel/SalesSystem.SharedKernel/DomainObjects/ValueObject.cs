namespace SalesSystem.SharedKernel.DomainObjects
{
    public abstract record ValueObject
    {
        public abstract void Validate();
    }
}