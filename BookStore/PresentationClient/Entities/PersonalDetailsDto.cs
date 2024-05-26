using Business.BTO;
using Persistence.DTO.Bill;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Entities
{
	public class PersonalDetailsDto
	{
		[Required]
		public string Address { get; set; }
		[Required]
		public string Telephone { get; set; }
		[Required]
		public string Country { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string PostalCode { get; set; }

		public void Deserialize(BillDto bill)
		{
			Address = bill.Address;
			Telephone = bill.Telephone;
			Country = bill.Country;
			City = bill.City;
			PostalCode = bill.PostalCode;
		}
	}
}
