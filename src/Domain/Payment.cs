namespace OrderingApi.Domain;

public class Payment
{
    private string method;

    public Payment(string method)
    {
        Method = this.method = method;
    }

    public string Method
    {
        get => method;
        set => SetPaymentMethod(value);
    }

    private void SetPaymentMethod(string method)
    {
        if (method != "BOLETO" && method != "CREDIT_CARD")
        {
            throw new Exception("Invalid payment method");
        }

        this.method = method;
    }
}
