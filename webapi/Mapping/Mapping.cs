using AutoMapper;
using webapi.Models;
using webapi.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Налаштування мапінгу між доменною моделлю і DTO
        CreateMap<Accident, AccidentDto>();
        CreateMap<Person, PersonDto>();
        CreateMap<Victim, VictimDto>();
        CreateMap<Violation, ViolationDto>();

        // Можна також налаштовувати зворотний мапінг
        CreateMap<AccidentDto, Accident>();
        CreateMap<PersonDto, Person>();
        CreateMap<VictimDto, Victim>();
        CreateMap<ViolationDto, Violation>();
    }
}
