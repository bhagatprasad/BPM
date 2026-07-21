using BPM.Web.API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("refreshtoken")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("userid")]
    public Guid UserId { get; set; }

    [Column("refreshtokenvalue")]
    public string RefreshTokenValue { get; set; } = string.Empty;

    [Column("jwttokenid")]
    public string? JwtTokenId { get; set; }

    [Column("createdon")]
    public DateTime CreatedOn { get; set; }

    [Column("expireson")]
    public DateTime ExpiresOn { get; set; }

    [Column("revokedon")]
    public DateTime? RevokedOn { get; set; }

    [Column("replacedbytoken")]
    public string? ReplacedByToken { get; set; }

    [Column("isrevoked")]
    public bool IsRevoked { get; set; }

    [Column("ipaddress")]
    public string? IpAddress { get; set; }

    [Column("useragent")]
    public string? UserAgent { get; set; }

    [Column("createdby")]
    public Guid? CreatedBy { get; set; }

    [Column("modifiedby")]
    public Guid? ModifiedBy { get; set; }

    [Column("modifiedon")]
    public DateTime? ModifiedOn { get; set; }
}