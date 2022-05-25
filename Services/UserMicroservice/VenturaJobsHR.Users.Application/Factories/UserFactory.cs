using VenturaJobsHR.Users.Application.Records.User;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Application.Factories;

public static class UserFactory
{
    public static User CreateUser(CreateUserRecord user)
    {
        var address = new Address(
            user.Address.CompleteAddress,
            user.Address.Complement,
            user.Address.PostalCode,
            user.Address.City,
            user.Address.State
            );

        var legalRecord = new LegalRecord(
                user.LegalRecord.CorporateName,
                user.LegalRecord.CPF,
                user.LegalRecord.CNPJ
            );

        var createdUser = new User(
                user.FirebaseId,
                user.Name,
                user.Phone,
                user.Email,
                address,
                user.UserType,
                legalRecord
            );

        return createdUser;
    }

    public static void UpdateUser(UpdateUserRecord record, User user)
    {
        var address = record.Address != null ? new Address(
                record.Address.CompleteAddress,
                record.Address.Complement,
                record.Address.PostalCode,
                record.Address.City,
                record.Address.State
            ) : null;

        var legalRecord = record.LegalRecord != null ? new LegalRecord(
                user.LegalRecord.CorporateName,
                user.LegalRecord.CPF,
                user.LegalRecord.CNPJ
            ) : null;

        user.Update(record.Id, record.Name, record.Phone, record.Email, address, legalRecord);
    }
}
