using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class PayService : IPayService
    {
    /*    private decimal overTimeHours;
        private decimal contractualEarnings;

*/

        private decimal contratualEarnings;
        private decimal overtimeHours;
        private readonly ApplicationDbContext _context;
        public PayService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contratualEarnings = hoursWorked * hourlyRate;

            }
            else
            {
                contratualEarnings = contractualHours * hourlyRate;
            }
            return contratualEarnings;
        }
      
        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll() => _context.PaymentRecords.OrderBy(p => p.EmployeeId);
       
       

        public PaymentRecord GetById(int id) => _context.PaymentRecords.Where(x => x.Id == id).FirstOrDefault();


        public TaxYear GetTaxYearById(int id) => _context.TaxYears.Where(year => year.Id == id).FirstOrDefault();
       

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction) => totalEarnings - totalDeduction;
        

        public decimal OvertimeEarnings(decimal overtimeEarnings, decimal contractualEarnings) => overtimeEarnings * contractualEarnings;


        public decimal OverTimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if (hoursWorked <= contractualHours)
                overtimeHours = 0.00m;
            else if (hoursWorked > contractualHours)
                overtimeHours = hoursWorked - contractualHours;
            return overtimeHours;
        }
        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        => overtimeEarnings + contractualEarnings;
        public decimal OvertimeRate(decimal hourlyRate) => hourlyRate * 1.5m;

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees) => tax + nic + studentLoanRepayment + unionFees;

        IEnumerable<SelectListItem> IPayService.GetAllTaxYear()
        {
            var allTaxYears = _context.TaxYears.Select(taxYears => new SelectListItem
            {
                Text = taxYears.YearOfTax,
                Value = taxYears.Id.ToString()
            });
            return allTaxYears;
        }
    }
}
