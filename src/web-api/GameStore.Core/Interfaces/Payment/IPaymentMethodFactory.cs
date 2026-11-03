namespace GameStore.Core.Interfaces.Payment
{
    internal interface IPaymentMethodFactory
    {
        IPaymentMethod GetStrategy(string methodName);
    }
}
