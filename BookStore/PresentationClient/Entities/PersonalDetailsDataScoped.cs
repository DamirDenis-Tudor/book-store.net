using Persistence.DTO.Bill;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Entities
{
	public class PersonalDetailsDataScoped
	{
		[Required]
		public string? Address { get; set; } = null;
		[Required]
		public string? Telephone { get; set; } = null;
		[Required]
		public string? Country { get; set; } = null;
		[Required]
		public string? City { get; set; } = null;
		[Required]
		public string? PostalCode { get; set; } = null;

		public bool IsValid()
		{
			return Address != null && Telephone != null && Country != null && City != null && PostalCode != null;
		}

		public BillDto ConvertToDto()
		{
			return new BillDto() {
				Address = Address ?? "",
				Country = Country ?? "",
				City = City ?? "",
				PostalCode = PostalCode ?? "",
				Telephone = Telephone ?? ""
			};
		}
	}
}
