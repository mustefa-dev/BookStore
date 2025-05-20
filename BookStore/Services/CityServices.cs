
using AutoMapper;
using BookStore.Repository;
using BookStore.Services;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;


public interface ICityServices
{
Task<(City? city, string? error)> Create(CityForm cityForm );
Task<(List<CityDto> citys, int? totalCount, string? error)> GetAll(CityFilter filter);
Task<(City? city, string? error)> Update(Guid id , CityUpdate cityUpdate);
Task<(City? city, string? error)> Delete(Guid id);Task<(City? city, string? error)> GetCityByName(string name);
}

public class CityServices : ICityServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CityServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(City? city, string? error)> Create(CityForm cityForm)
    {

        var district = await _repositoryWrapper.District.Get(d => d.Id == cityForm.DistrictId);
        if (district == null) return (null, "Governorate Not Found");
        var city = _mapper.Map<City>(cityForm);
        var response = await _repositoryWrapper.City.Add(city);
        return response == null ? (null, "City") : (response, null);
    }

    public async Task<(List<CityDto> citys, int? totalCount, string? error)> GetAll(CityFilter filter)
    {
        var (city, totalCount) = await _repositoryWrapper.City.GetAll<CityDto>(
            x =>
                (filter.Name == null || x.Name.Contains(filter.Name))&&
                (filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                
            ,
            filter.PageNumber, filter.PageSize
        );
        
        return  (city, totalCount, null);
    }
    

    public async Task<(City? city, string? error)> Update(Guid id, CityUpdate cityUpdate)
    {
        var city = await _repositoryWrapper.City.Get(x => x.Id == id);
        if (city == null)
        {
            return (null, "City Not Found");
        }
        _mapper.Map(cityUpdate, city);
        var response = await _repositoryWrapper.City.Update(city);
        return response == null ? (null, "City") : (response, null);
    }

    public async Task<(City? city, string? error)> Delete(Guid id)
    {
        var city = await _repositoryWrapper.City.Get(x => x.Id == id);
        if (city == null)
        {
            return (null, "City Not Found");
        }
        var response = await _repositoryWrapper.City.SoftDelete(id);
        return response == null ? (null, "City") : (response, null);
    }

// create method to getCityByname
    public async Task<(City? city, string? error)> GetCityByName(string name)
    {
        var city = await _repositoryWrapper.City.Get(x => x.Name == name);
        if (city == null)
        {
            return (null, "City Not Found");
        }

        return (city, null);

    }
}
