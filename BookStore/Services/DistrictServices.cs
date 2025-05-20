
using AutoMapper;
using BookStore.Repository;
using BookStore.Services;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;


public interface IDistrictServices
{
Task<(District? district, string? error)> Create(DistrictForm districtForm );
Task<(List<DistrictDto> districts, int? totalCount, string? error)> GetAll(DistrictFilter filter);
Task<(District? district, string? error)> Update(Guid id , DistrictUpdate districtUpdate);
Task<(District? district, string? error)> Delete(Guid id);
}

public class DistrictServices : IDistrictServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public DistrictServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(District? district, string? error)> Create(DistrictForm districtForm )
{
    var governorate = await _repositoryWrapper.Governorate.Get(g => g.Id == districtForm.GovernorateId);
    if (governorate == null) return (null, "Governorate Not Found");
    var district = _mapper.Map<District>(districtForm);
    var response = await _repositoryWrapper.District.Add(district);
    return response == null ? (null, "District"): (response,null);
}

public async Task<(List<DistrictDto> districts, int? totalCount, string? error)> GetAll(DistrictFilter filter)
    {
        var (district,totalCount) = await _repositoryWrapper.District.GetAll<DistrictDto>(
            x =>
                (filter.Name == null || x.Name.Contains(filter.Name)) ,
            filter.PageNumber, filter.PageSize
        );
            
        return (district, totalCount, null);
    }

public async Task<(District? district, string? error)> Update(Guid id ,DistrictUpdate districtUpdate)
    {
        var district = await _repositoryWrapper.District.Get(x => x.Id == id);
        if (district == null)
        {
            return (null, "District Not Found");
        }
        _mapper.Map(districtUpdate, district);
        var response = await _repositoryWrapper.District.Update(district);
        return response == null ? (null, "District") : (response, null);
    }

public async Task<(District? district, string? error)> Delete(Guid id)
    {
        var district = await _repositoryWrapper.District.Get(x => x.Id == id);
        if (district == null)
        {
            return (null, "District Not Found");
        }
        var response = await _repositoryWrapper.District.SoftDelete(id);
        return response == null ? (null, "District") : (response, null);
    }

}
