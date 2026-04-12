using AutoMapper;
using CFMS.Domain.Entities;
using CFMS.Domain.ValueObjects;
using CFMS.Application.DTOs.Patients;
using CFMS.Application.DTOs.Doctors;
using CFMS.Application.DTOs.Drugs;
using CFMS.Application.DTOs.Sales;
using CFMS.Application.DTOs.Expenses;

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
        
        // Sale mappings
        CreateMap<Sale, SaleDto>();
        CreateMap<SaleItem, SaleItemDto>();
        
        // Expense mappings
        CreateMap<Expense, ExpenseDto>();
        CreateMap<CreateExpenseRequest, Expense>();
        CreateMap<UpdateExpenseRequest, Expense>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Loan mappings (for future use)
        // CreateMap<Loan, LoanDto>();
        // CreateMap<CreateLoanRequest, Loan>();
    }
}
