namespace Bio.Domain.Enums
{
    [Flags]
    public enum EmployeesRange
    {
        None = 0,
        OneToFive = 1,    
        SixToTen = 2,         
        ElevenToTwenty = 4,     
        TwentyOneToFifty = 8,    
        FiftyOneToOneHundred = 16,
        OverOneHundred = 32      
    }
}
