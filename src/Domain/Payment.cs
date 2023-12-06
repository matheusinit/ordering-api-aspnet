namespace OrderingApi.Domain;

public class Payment
{
    private string method;
    private string? cardNumber;
    private int? expirationMonth;
    private int? expirationYear;
    private int? cvc;

    public Payment(string method)
    {
        Method = this.method = method;
    }

    public Payment(
        string method,
        string? cardNumber,
        int? expirationMonth,
        int? expirationYear,
        int? cvc
    )
        : this(method)
    {
        CardNumber = cardNumber;
        ExpirationMonth = expirationMonth;
        ExpirationYear = expirationYear;
        Cvc = cvc;
    }

    public string Method
    {
        get => method;
        set => SetPaymentMethod(value);
    }

    public string? CardNumber
    {
        get => cardNumber;
        set => SetCardNumber(value);
    }

    public int? ExpirationMonth
    {
        get => expirationMonth;
        set => SetExpirationMonth(value);
    }

    public int? ExpirationYear
    {
        get => expirationYear;
        set => SetExpirationYear(value);
    }

    public int? Cvc
    {
        get => cvc;
        set => SetCvc(value);
    }

    private void SetPaymentMethod(string method)
    {
        if (!IsValidPaymentMethod(method))
        {
            throw new Exception("Invalid payment method");
        }

        this.method = method;
    }

    private Boolean IsValidPaymentMethod(string method)
    {
        return method == "BOLETO" || method == "CREDIT_CARD";
    }

    private void SetCardNumber(string? cardNumber)
    {
        if (this.method == "CREDIT_CARD" && cardNumber == null)
        {
            throw new Exception(
                "Invalid payment method. Credit card method is required to set card number"
            );
        }

        this.cardNumber = cardNumber;
    }

    private void SetExpirationMonth(int? expirationMonth)
    {
        if (this.method == "CREDIT_CARD" && expirationMonth == null)
        {
            throw new Exception(
                "Invalid payment method. Credit card method is required to set expiration month"
            );
        }

        this.expirationMonth = expirationMonth;
    }

    private void SetExpirationYear(int? expirationYear)
    {
        if (this.method == "CREDIT_CARD" && expirationYear == null)
        {
            throw new Exception(
                "Invalid payment method. Credit card method is required to set expiration year"
            );
        }

        this.expirationYear = expirationYear;
    }

    private void SetCvc(int? cvc)
    {
        if (this.method == "CREDIT_CARD" && cvc == null)
        {
            throw new Exception(
                "Invalid payment method. Credit card method is required to set cvc"
            );
        }

        this.cvc = cvc;
    }
}
