using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Interface;
using BookStore.Repository;

namespace BookStore.Services;

public interface IAddressServices
{
    Task<(AddressDto? address, string? error)> Create(AddressForm addressForm,Guid userId);
    Task<(List<AddressDto> addresss, int? totalCount, string? error)> GetAll(AddressFilter filter,Guid userId);
    Task<(AddressDto? address, string? error)> Update(Guid id, AddressUpdate addressUpdate,Guid userId);
    Task<(Address? address, string? error)> Delete(Guid id,Guid userId);
}

public class AddressServices : IAddressServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AddressServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(AddressDto? address, string? error)> Create(AddressForm addressForm, Guid userId)
    {
        var user = await _repositoryWrapper.User.GetById(userId);
        if (user == null) return (null, "User does not exist");
        var address = _mapper.Map<Address>(addressForm);
        address.AppUserId = userId;
        var response = await _repositoryWrapper.Address.Add(address);
        if (response == null) return (null, "address couldn't be added");
    
        var addressDto = _mapper.Map<AddressDto>(response);
        return (addressDto, null);
    }

    public async Task<(List<AddressDto> addresss, int? totalCount, string? error)> GetAll(AddressFilter filter,Guid userId)
    {
        var user =await _repositoryWrapper.User.GetById(userId);
        if (user ==null) return (null,null, "User does not exist");
        var (car,totalCount) =  await _repositoryWrapper.Address.GetAll<AddressDto>(
            x =>
                (filter.Name == null || x.Name.Contains(filter.Name)),
            filter.PageNumber, filter.PageSize
        );
        var responseDto = _mapper.Map<List<AddressDto>>(car);
        return (responseDto, totalCount, null);
    }   

    public async Task<(AddressDto? address, string? error)> Update(Guid id, AddressUpdate addressUpdate ,Guid userId)
    {
        var address = await _repositoryWrapper.Address.Get(x => x.Id==id);
        if (address==null) return (null, "address not found");
        address = _mapper.Map(addressUpdate, address);
        var response = await _repositoryWrapper.Address.Update(address);
        var responseDto = _mapper.Map<AddressDto>(response);
        return response == null ? (null, "address couldn't be updated") : (responseDto, null);
        
    }

    public async Task<(Address? address, string? error)> Delete(Guid id,Guid userId)
    {
        var address = await _repositoryWrapper.Address.Get(x => x.Id==id);
        if (address==null) return (null, "address not found");
        address= _mapper.Map<Address>(address);
        var response = await _repositoryWrapper.Address.SoftDelete(id);
        return response == null ? (null, "address couldn't be deleted") : (address, null);
    }
}