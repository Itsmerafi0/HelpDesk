using API.Contexts;
using API.Contracs;
using API.Models;
using API.Utility;
using API.ViewModel.Ticket;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;

namespace API.Repository
{
    public class TicketRepository : GeneralRepository<Ticket>, ITicketRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TicketRepository(HelpDeskManagementDBContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext)
        {
            _contextAccessor = contextAccessor;
        }

        public bool CheckTicket(string value)
        {
            return _dbContext.Tickets.Any(e => e.TicketId == value);
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
                                      r.ResolvedBy,
                                      c.Description,
                                      c.Attachment,
                                      r.Notes,
                                      r.FinishedDate,
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
                    ResolvedBy = complainDetail.ResolvedBy,
                    Description = complainDetail.Description,
                    Attachment = complainDetail.Attachment,
                    ResolutionNote = complainDetail.Notes,
                    FinishedDate = complainDetail.FinishedDate,
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

/*            Guid employeeGuid = new Guid("69de0986-1bd4-4237-8af2-08db797341b9");
*/
/*            var employeeGuid = Guid.Parse(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
*/
            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.Guid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where t.CategoryName == "Access" && (r.Status == StatusLevel.Done || r.Status == StatusLevel.InProgress)
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
                    FinishedDate = complainDetail.FinishedDate
                };
                details.Add(detail);
            }
            return details;
        }  
        
        public IEnumerable<GetComplainForUserVM> GetAllComplainUser()
        {
            var complains = GetAll();
            var employees = _dbContext.Employees.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var resolutions = _dbContext.Resolutions.ToList();
            var categories = _dbContext.Categories.ToList();

/*            Guid employeeGuid = new Guid("69de0986-1bd4-4237-8af2-08db797341b9");
*/
            var employeeGuid = Guid.Parse(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var complainDetails = from c in complains
                                  join e in employees on c.EmployeeGuid equals e.Guid
                                  join r in resolutions on c.Guid equals r.Guid
                                  join s in subcategories on c.SubCategoryGuid equals s.Guid
                                  join t in categories on s.CategoryGuid equals t.Guid
                                  where e.Guid == employeeGuid 
                                  select new
                                  {
                                      c.Guid,
                                      c.TicketId,
                                      Requester = e.FirstName + " " + e.LastName,
                                      c.Description,
                                      c.Attachment,
                                      t.CategoryName,
                                      s.Name,
                                      r.Status
                                  };

            var details = new List<GetComplainForUserVM>();
            foreach (var complainDetail in complainDetails)
            {
                var detail = new GetComplainForUserVM
                {
                    Guid = complainDetail.Guid,
                    TicketId = complainDetail.TicketId,
                    Requester = complainDetail.Requester,
                    Description = complainDetail.Description,
                    Attachment = complainDetail.Attachment,
                    CategoryName = complainDetail.CategoryName,
                    SubCategoryName = complainDetail.Name,
                    StatusLevel = complainDetail.Status
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
                                  where t.CategoryName == "Reimbursement" && (r.Status == StatusLevel.Done || r.Status == StatusLevel.InProgress)
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
                    ResolutionNote = complainDetail.Notes,
                    FinishedDate = complainDetail.FinishedDate,
                    
                };
                details.Add(detail);
            }
            return details;
        }

        private string GenerateID(Guid subCategoryGuid)
        {
            var categories = _dbContext.Categories.ToList();
            var subcategories = _dbContext.SubCategories.ToList();
            var tickets = GetAll();

            var subCategory = subcategories.FirstOrDefault(s => s.Guid == subCategoryGuid);

            if (subCategory != null)
            {
             
                    string prefix = subCategory.Name.Substring(0, 1);

                    var lastId = (from t in tickets
                                  join s in subcategories on t.SubCategoryGuid equals s.Guid
                                  join c in categories on s.CategoryGuid equals c.Guid
                                  where t.SubCategoryGuid == subCategoryGuid
                                  orderby t.TicketId descending
                                  select t.TicketId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(lastId))
                    {
                        string numericPart = lastId.Substring(1);
                        if (int.TryParse(numericPart, out int lastTicketNumber))
                        {
                            return prefix + (lastTicketNumber + 1).ToString();
                        }
                    }

                    return prefix + "10000";
                }
            

            return string.Empty; // Jika tidak ditemukan subCategoryGuid yang valid, kembalikan string kosong.
        }


        public int CreateTicketResolution(TicketResoVM ticketresolutionVM)
        {
            try
            {
                var ticket = new Ticket
                {

                    SubCategoryGuid = ticketresolutionVM.SubCategoryGuid,
                    TicketId = GenerateID(ticketresolutionVM.SubCategoryGuid),
                    Description = ticketresolutionVM.Description,
                    Attachment = ticketresolutionVM.Attachment,
                    EmployeeGuid = ticketresolutionVM.EmployeeGuid,
                };
                Create(ticket);

                var resolution = new Resolution
                {
                    Guid = ticket.Guid,
                    Status = 0,
                    Notes = null,
                    FinishedDate = null
                };
                _dbContext.Resolutions.Add(resolution);
                _dbContext.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {

                return 0;
            }
        }

        public string FindIdByTicketGuid(Guid ticketGuid)
        {
            try
            {
                var ticket = GetByGuid(ticketGuid);
                return ticket.TicketId;
            }
            catch
            {
                return null;
            }
        }


        /*   public int CreateTicketResolution(TicketResoVM ticketresolutionVM)
           {
               try
               {
                   var ticket = new Ticket
                   {

                       SubCategoryGuid = ticketresolutionVM.SubCategoryGuid,
                       TicketId = GenerateID(ticketresolutionVM.SubCategoryGuid),
                       Description = ticketresolutionVM.Description,
                       Attachment = ticketresolutionVM.Attachment,
                       EmployeeGuid = ticketresolutionVM.EmployeeGuid,
                   };
                   Create(ticket);

                   var resolution = new Resolution
                   {
                       Guid = ticket.Guid,
                       Status = 0,
                       Notes = null,
                       FinishedDate = null
                   };
                   _dbContext.Resolutions.Add(resolution);
                   _dbContext.SaveChanges();
                   return 1;
               }
               catch (Exception e)
               {
                   Console.WriteLine(e.StackTrace);
                   return 0;
               }
           }*/
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
