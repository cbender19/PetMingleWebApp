using Microsoft.EntityFrameworkCore;
using PetMingle.Entities;

namespace PetMingle.Data.Repositories.Admin
{
    public class CompanyRepository : IDisposable
    {
        private readonly DataContext _ctx;

        public CompanyRepository(DataContext ctx)
        {
            _ctx = ctx;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            _ctx.Companys.Add(company);
            await _ctx.SaveChangesAsync();
            return company;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _ctx.Companys.Include(a => a.Products).FirstAsync(a => a.ID == id);
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            var companys = await _ctx.Companys.Include(a => a.Products).ToListAsync();
            return companys;
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await GetCompanyByIdAsync(id);
            _ctx.Companys.Remove(company);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Company> UpdateCompanyAsync(Company updatedCompany)
        {
            var company = await GetCompanyByIdAsync(updatedCompany.ID);
            company.Name = updatedCompany.Name;
            company.Description = updatedCompany.Description;
            company.StockQuantity = updatedCompany.StockQuantity;
            await _ctx.SaveChangesAsync();
            return company;
        }
    }
}
