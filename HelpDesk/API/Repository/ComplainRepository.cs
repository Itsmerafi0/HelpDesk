using API.Contexts;
using API.Contracs;
using API.Models;
using API.ViewModel.Complain;

namespace API.Repository
{
    public class ComplainRepository : GeneralRepository<Complain>, IComplainRepository
    {
        public ComplainRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

        }
        public IEnumerable<ComplainDetailVM> GetAllComplainDetail()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.ComplainGuid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  select new
                                  {
                                      c.Guid,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      t.CategoryName,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Description,
                                      r.Notes
                                  };
            var details = new List<ComplainDetailVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new ComplainDetailVM
                {
                    Guid = complainDetail.Guid,
                    Requester = complainDetail.Requester,
                    Email = complainDetail.Email,
                    CategoryName = complainDetail.CategoryName,
                    SubCategoryName = complainDetail.Name,
                    RiskLevel = complainDetail.RiskLevel,
                    StatusLevel = complainDetail.Status,
                    Description = complainDetail.Description,
                    ResolutionNote = complainDetail.Notes
                };
                details.Add(detail);
            }
            return details;
        }

        public IEnumerable<GetComplainForDevVM> GetAllComplainDev()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.ComplainGuid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where t.CategoryName == "Access"
                                  select new
                                  {
                                      c.Guid,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Attachment,
                                      c.Description,
                                      r.Notes
                                  };
            var details = new List<GetComplainForDevVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new GetComplainForDevVM
                {
                    Guid = complainDetail.Guid,
                    Requester = complainDetail.Requester,
                    Email = complainDetail.Email,
                    SubCategoryName = complainDetail.Name,
                    Attachment = complainDetail.Attachment,
                    RiskLevel = complainDetail.RiskLevel,
                    StatusLevel = complainDetail.Status,
                    Description = complainDetail.Description,
                    ResolutionNote = complainDetail.Notes
                };
                details.Add(detail);
            }
            return details;
        }

        public IEnumerable<GetComplainForFinanceVM> GetAllComplainFinance()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.ComplainGuid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where t.CategoryName == "Reimbursment"
                                  select new
                                  {
                                      c.Guid,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Attachment,
                                      c.Description,
                                      r.Notes
                                  };

            var details = new List<GetComplainForFinanceVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new GetComplainForFinanceVM
                {
                    Guid = complainDetail.Guid,
                    Requester = complainDetail.Requester,
                    Email = complainDetail.Email,
                    SubCategoryName = complainDetail.Name,
                    Attachment = complainDetail.Attachment,
                    RiskLevel = complainDetail.RiskLevel,
                    StatusLevel = complainDetail.Status,
                    Description = complainDetail.Description,
                    ResolutionNote = complainDetail.Notes
                };
                details.Add(detail);
            }
            return details;
        }

    }
   }
