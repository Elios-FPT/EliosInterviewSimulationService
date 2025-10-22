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

    }
}
