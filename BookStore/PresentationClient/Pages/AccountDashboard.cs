using Persistence.DAL;
using Persistence.DTO.Order;

namespace PresentationClient.Pages
{
	public partial class AccountDashboard
	{
		private readonly IList<OrderSessionDto> _orders = PersistenceFacade.Instance.OrderRepository.GetAllOrdersByUsername("john_doe123").SuccessValue;
	}
}
