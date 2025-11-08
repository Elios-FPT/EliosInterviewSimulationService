using System;

namespace InterviewSimulation.Contract.TransferObjects
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
