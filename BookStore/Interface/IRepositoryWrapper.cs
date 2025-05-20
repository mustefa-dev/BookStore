namespace BookStore.Interface;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }

    // here to add

    ICityRepository City { get; }
    IAddressRepository Address { get; }
    IDistrictRepository District { get; }
    IGovernorateRepository Governorate { get; }
    IOrderRepository Order { get; }
    IOrderItemRepository OrderItem { get; }
    ICartRepository Cart { get; }
    ICartProductRepository CartProduct { get; }
    ICategoryRepository Category { get; }
    IBookRepository Book { get; }
}