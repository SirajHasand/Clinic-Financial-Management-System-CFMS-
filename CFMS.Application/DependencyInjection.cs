using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace CFMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Register services
        services.AddScoped<Services.Patients.IPatientService, Services.Patients.PatientService>();
        services.AddScoped<Services.Doctors.IDoctorService, Services.Doctors.DoctorService>();
        services.AddScoped<Services.Drugs.IDrugService, Services.Drugs.DrugService>();
        services.AddScoped<Services.Sales.ISaleService, Services.Sales.SaleService>();
        services.AddScoped<Services.Expenses.IExpenseService, Services.Expenses.ExpenseService>();
        
        return services;
    }
}
