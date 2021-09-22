using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BLL.Services
{
    public interface IQnAService
    {
        PagedResult<QnAsViewModel> GetAll(int pageNumber, int pageSize);
        Task<QnAsViewModel> AddAsync(QnAsViewModel qnAVM);
        IEnumerable<QnAsViewModel> GetAllQnAsByExam(int examId);
        bool IsExamAttended(int examId, int studentId);
    }
}
