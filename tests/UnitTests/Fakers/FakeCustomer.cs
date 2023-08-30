using Bogus;
using Domain.Entities;

namespace UnitTests.Fakers;

public static class FakeCustomer
{
    public static Customer Create(){
    {
        var faker = new Faker();
        return new Customer(faker.Person.Email);
    }}
}