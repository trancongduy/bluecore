using Framework.Constract.SeedWork;

namespace Blue.Constract.Dtos
{
    public class ContactDto : BaseDto
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Pinterest { get; set; }
        public string Google { get; set; }
    }
}
