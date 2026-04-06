namespace CFMS.Domain.ValueObjects;

public class Address
{
    public string Province { get; private set; } = string.Empty;
    public string District { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;

    // Required for EF Core
    private Address() { }

    public Address(string province, string district, string street)
    {
        Province = province;
        District = district;
        Street = street;
    }

    public override string ToString()
    {
        return $"{Province}, {District}, {Street}";
    }
}