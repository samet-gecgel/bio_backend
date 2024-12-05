namespace Bio.Domain.Enums
{
    [Flags]
    public enum EducationLevel
    {
        None = 0,
        Primary = 1,
        Middle = 2,
        HighSchool = 4,
        Bachelor = 8,
        Master = 16,
        Doctorate = 32
    }
}
