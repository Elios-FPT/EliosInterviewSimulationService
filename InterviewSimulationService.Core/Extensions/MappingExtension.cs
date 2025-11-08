using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Domain.Entities;
using InterviewSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterviewSimulation.Core.Extensions
{
    public static class MappingExtension
    {
        public static NotificationDto ToDto(this Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
                Url = notification.Url,
                Metadata = notification.Metadata
            };
        }

        public static QuestionDto ToDto(this Question question)
        {
            return new QuestionDto
            {
                Id = question.Id,
                CategoryId = question.CategoryId,
                Title = question.Title,
                Difficulty = question.Difficulty,
                QuestionText = question.QuestionText,
                QuestionVideoUrl = question.QuestionVideoUrl,
                Prefix = question.Prefix,
                Filename = question.Filename,
                PublicUrl = question.PublicUrl,
                IsActive = question.IsActive,
                CreatedAt = question.CreatedAt
            };
        }

        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

    }
}
