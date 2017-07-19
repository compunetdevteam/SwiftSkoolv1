using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftSkoolv1.WebUI.APIRepository.RepoAbstractions
{
    public interface IStudentRepository : IRepository
    {
        IQueryable<Student> GetAll();

        IList<StudentClientViewModel> MapToVMSequence(Student entity);

        //StudentClientViewModel MapToVM(Student entity);

        Task<Student> GetByIdAsync(string id);

        Task<Student> GetByIdUnTrackedAsync(string id);

        void Add(Student entity);

        void Update(Student entity);

        void Delete(Student entity);
    }
}
