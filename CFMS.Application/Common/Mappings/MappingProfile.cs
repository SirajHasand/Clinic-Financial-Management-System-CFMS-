using AutoMapper;
using CFMS.Domain.Entities;
using CFMS.Domain.ValueObjects;
using CFMS.Application.DTOs.Patients;
using CFMS.Application.DTOs.Doctors;
using CFMS.Application.DTOs.Drugs;
using CFMS.Application.DTOs.Sales;

namespace CFMS.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Address mappings
        CreateMap<AddressDto, Address>();
        CreateMap<Address, AddressDto>();
        
        // Patient mappings
        CreateMap<Patient, PatientDto>();
        CreateMap<CreatePatientRequest, Patient>();
        CreateMap<UpdatePatientRequest, Patient>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Doctor mappings
        CreateMap<Doctor, DoctorDto>();
        CreateMap<CreateDoctorRequest, Doctor>();
        CreateMap<UpdateDoctorRequest, Doctor>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Drug mappings
        CreateMap<Drug, DrugDto>();
        CreateMap<CreateDrugRequest, Drug>();
        CreateMap<UpdateDrugRequest, Drug>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Customer mappings (for future use)
        // CreateMap<Customer, CustomerDto>();
        
        // Sale mappings (for future use)
        CreateMap<Sale, SaleDto>();
        CreateMap<SaleItem, SaleItemDto>();
        // CreateMap<Sale, SaleDto>();
        // CreateMap<CreateSaleRequest, Sale>();
        
        // Expense mappings (for future use)
        // CreateMap<Expense, ExpenseDto>();
        // CreateMap<CreateExpenseRequest, Expense>();
        
        // Loan mappings (for future use)
        // CreateMap<Loan, LoanDto>();
        // CreateMap<CreateLoanRequest, Loan>();
    }
}