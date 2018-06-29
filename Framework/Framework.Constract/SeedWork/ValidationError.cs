namespace Framework.Constract.SeedWork
{
    public class ValidationError
    {
        public string Key { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string InnerException { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
    }
}
