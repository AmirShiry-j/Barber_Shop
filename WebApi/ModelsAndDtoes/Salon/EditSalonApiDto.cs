namespace WebApi.ModelsAndDtoes.Salon
{
    public class EditSalonApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string FullAddress { get; set; }
        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}
