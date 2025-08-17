using EventAPI.Models;
using Microsoft.EntityFrameworkCore;
using EventAPI.Models.Associations;

namespace EventAPI.Data
{
    public class EventDbContext(DbContextOptions<EventDbContext> options):DbContext(options)
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<TaskEntity> Tasks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<EventParticipant> EventParticipants { get; set; }

        public DbSet<EventTasks> EventTasks { get; set; }

        public DbSet<TaskUser> TaskUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Many-to-many: Event <-> User(Participants)
            modelBuilder.Entity<Event>()
               .HasMany(e => e.Participants)
               .WithMany(e => e.Events)
               .UsingEntity<EventParticipant>();


            // Many-to-many: Task <-> User
            modelBuilder.Entity<User>()
               .HasMany(e => e.Tasks)
               .WithMany(e => e.Users)
               .UsingEntity<TaskUser>();

            // Many-to-many: Event <-> Task via EventTask
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Tasks)
                .WithMany(e => e.Events)
                .UsingEntity<EventTasks>();

            // CommentEntity.EventID references Event.Id 
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Comments)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            //CommentEntity.UserID references User.Id
            modelBuilder.Entity<Comment>()
               .HasOne<User>()
               .WithMany()
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
