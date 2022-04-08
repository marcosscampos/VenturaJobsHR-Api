namespace VenturaJobsHR.CrossCutting.Enums;

public class CommonsEnum
{
    public enum Error
    {
        IdIsMissing = 1,
        NullOrEmptyObject = 2,
        DateGreaterThanAnotherDate = 3,
        EntityNotExist = 4,
        EntityMustHaveField = 5,
        FieldMinimumOfCharacteres = 6,
        FieldRequired = 7,
        EntityMustHaveValidField = 8,
        EntityDateMustBeBetween = 9,
        InvalidOrderBy = 10,
        InvalidConstructor = 11,
        ParameterInvalid = 12,
        EmptyFilter = 13,
        PermissionDenied = 14
    }
}
