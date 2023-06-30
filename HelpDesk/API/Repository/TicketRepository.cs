using API.Contexts;
using API.Contracs;
using API.Models;
using API.ViewModel.Ticket;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace API.Repository
{
    public class TicketRepository : GeneralRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

        }
        public IEnumerable<TicketDetailVM> GetAllComplainDetail()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.Guid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  select new
                                  {
                                      c.Guid,
                                      c.TicketId,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      t.CategoryName,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Description,
                                      r.Notes
                                  };
            var details = new List<TicketDetailVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new TicketDetailVM
                {
                    Guid = complainDetail.Guid,
                    TicketId = complainDetail.TicketId,
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

        public IEnumerable<GetTicketForDevVM> GetAllComplainDev()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.Guid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where t.CategoryName == "Access"
                                  select new
                                  {
                                      c.Guid,
                                      c.TicketId,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Attachment,
                                      c.Description,
                                      r.Notes,
                                      r.FinishedDate
                                  };
            var details = new List<GetTicketForDevVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new GetTicketForDevVM
                {
                    Guid = complainDetail.Guid,
                    TicketId = complainDetail.TicketId,
                    Requester = complainDetail.Requester,
                    Email = complainDetail.Email,
                    SubCategoryName = complainDetail.Name,
                    Attachment = complainDetail.Attachment,
                    RiskLevel = complainDetail.RiskLevel,
                    StatusLevel = complainDetail.Status,
                    Description = complainDetail.Description,
                    ResolutionNote = complainDetail.Notes,
                    FinishDate = complainDetail.FinishedDate
                    
                };
                details.Add(detail);
            }
            return details;
        }

        public IEnumerable<GetTicketForFinanceVM> GetAllComplainFinance()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();


            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.Guid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where t.CategoryName == "Reimbursement"
                                  select new
                                  {
                                      c.Guid,
                                      c.TicketId,
                                      Requester = e.FirstName + " " + e.LastName,
                                      e.Email,
                                      s.Name,
                                      s.RiskLevel,
                                      r.Status,
                                      c.Attachment,
                                      c.Description,
                                      r.Notes
                                  };
            var details = new List<GetTicketForFinanceVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new GetTicketForFinanceVM
                {
                    Guid = complainDetail.Guid,
                    TicketId = complainDetail.TicketId,
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

        private string GenerateID()
        {
            var lastId = GetAll().OrderByDescending(e => int.Parse(e.TicketId)).FirstOrDefault();
            if (lastId != null)
            {
                int lastTicketNumber;
                if (int.TryParse(lastId.TicketId, out lastTicketNumber))
                {
                    return (lastTicketNumber + 1).ToString();
                }
            }
            return "10000";
        }

        public int CreateReso(TicketResoVM ticketresolutionVM)
        {
            try
            {
                var complain = new Ticket
                {
                    TicketId = GenerateID(),
                    SubCategoryGuid = ticketresolutionVM.SubCategoryGuid,
                    Description = ticketresolutionVM.Description,
                    Attachment = ticketresolutionVM.Attachment,
                    EmployeeGuid = ticketresolutionVM.EmployeeGuid,
                };
                Create(complain);

                var resolution = new Resolution
                {
                    Guid = complain.Guid,
                    Status = 0,
                    Notes = null,
                    FinishedDate = null
                };
                _dbContext.Resolutions.Add(resolution);
                _dbContext.SaveChanges();
                return 1;
            }
            catch 
            {

                return 0;
            }
        }
        public string FindEmailByComplainGuid(Guid complainGuid)
        {
            try
            {
                var complain = GetByGuid(complainGuid);
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Guid == complain.EmployeeGuid);
                if (employee == null)
                {
                    return null;
                }
                return employee.Email;
            }
            catch
            {
                return null;
            }
        }
    }
}
