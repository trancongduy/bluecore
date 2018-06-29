using Blue.Constract.Dtos;
using Blue.Data.Models;
using Framework.Constract.Interfaces;
using Framework.Data.SeedWork;

namespace Blue.DomainService
{
    public interface IContactService : IService<Contact, ContactDto>
    {
    }

    public class ContactService : Service<Contact, ContactDto>, IContactService
    {
        private readonly IRepository<Contact> _contactRepository;

        public ContactService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _contactRepository = unitOfWork.Repository<Contact>();
        }
    }
}
