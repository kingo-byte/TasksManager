using COMMON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.DTOs.Dtos;
using static COMMON.Models.Model;
using Task = COMMON.Models.Task;

namespace COMMON.Extensions
{
    public static class ModelsExtensions
    {
        public static UserDto? ToDto(this User user) 
        {
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                LastModifiedDate = user.LastModifiedDate,
                Tasks = user.Tasks != null ? user.Tasks.Select(t => t.ToDto()!).ToList() : new List<TaskDto>()
            };
        }

        public static TaskDto? ToDto(this Task? task)
        {
            if (task == null) return null;
            
            return new TaskDto
            {
                Id = task.Id,
                UserId = task.UserId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                CreatedDate = task.CreatedDate,
                LastModifiedDate = task.LastModifiedDate
            };
        }
    }
}
