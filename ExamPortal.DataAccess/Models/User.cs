using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Models;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("first_name")]
    [StringLength(255)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(255)]
    public string LastName { get; set; } = null!;

    [StringLength(500)]
    public string? Address { get; set; }
    
    [Column("zipcode")]
    [StringLength(10)]
    public string? Zipcode { get; set; }

    [Column("mobile_number")]
    [StringLength(20)]
    public string? MobileNumber { get; set; }

    [Column("password_hash", TypeName = "character varying")]
    public string PasswordHash { get; set; } = null!;

    [Column("profile_img", TypeName = "character varying")]
    public string? ProfileImg { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    public string? ResetToken { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }

    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; } = false;

    public ICollection<ExamAttempt> ExamAttempts { get; set; }
    public ICollection<UserSession> UserSessions { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }


}
