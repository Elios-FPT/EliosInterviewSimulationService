using InterviewSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewSimulation.Infrastructure.DataContext
{
	public class InterviewSimulationDataContext : DbContext
	{
		public InterviewSimulationDataContext(DbContextOptions<InterviewSimulationDataContext> options) : base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<InterviewSession> InterviewSessions { get; set; }
		public DbSet<SessionQuestionAnswer> SessionQuestionAnswers { get; set; }
		public DbSet<InterviewAnswer> InterviewAnswers { get; set; }
		public DbSet<Transcript> Transcripts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Category 1..many Questions
			modelBuilder.Entity<Question>()
				.HasOne(q => q.Category)
				.WithMany(c => c.Questions)
				.HasForeignKey(q => q.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			// InterviewSession 1..many SessionQuestionAnswer
			modelBuilder.Entity<SessionQuestionAnswer>()
				.HasOne(sqa => sqa.Session)
				.WithMany(s => s.SessionQuestionAnswers)
				.HasForeignKey(sqa => sqa.SessionId)
				.OnDelete(DeleteBehavior.Cascade);

			// Question 1..many SessionQuestionAnswer
			modelBuilder.Entity<SessionQuestionAnswer>()
				.HasOne(sqa => sqa.Question)
				.WithMany(q => q.SessionQuestionAnswers)
				.HasForeignKey(sqa => sqa.QuestionId)
				.OnDelete(DeleteBehavior.Restrict);

			// InterviewSession 1..many InterviewAnswer
			modelBuilder.Entity<InterviewAnswer>()
				.HasOne(ia => ia.Session)
				.WithMany(s => s.InterviewAnswers)
				.HasForeignKey(ia => ia.SessionId)
				.OnDelete(DeleteBehavior.Cascade);

			// SessionQuestionAnswer 1..many InterviewAnswer
			modelBuilder.Entity<InterviewAnswer>()
				.HasOne(ia => ia.SessionQuestion)
				.WithMany()
				.HasForeignKey(ia => ia.SessionQuestionId)
				.OnDelete(DeleteBehavior.Restrict);

			// InterviewAnswer 1..1 Transcript with shared primary key
			modelBuilder.Entity<InterviewAnswer>()
				.HasOne(ia => ia.Transcript)
				.WithOne(t => t.InterviewAnswer)
				.HasForeignKey<Transcript>(t => t.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}


