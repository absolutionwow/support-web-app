using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SupportWebApp.Models;

public enum SupportCategory
{
    [Display(Name = "Teknisk spørgsmål vedr. køb af ny cykel")]
    TekniskSpoergsmaal,

    [Display(Name = "Gør-det-selv / reservedele")]
    Reservedele,

    [Display(Name = "Forslag til ændringer / nye features")]
    Forslag,

    [Display(Name = "Nærmeste forhandler")]
    Forhandler,

    [Display(Name = "Send mig det nyeste katalog")]
    Katalog,

    [Display(Name = "Andet")]
    Andet
}

public class SupportMessage
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Kontaktdata
    [Required(ErrorMessage = "Navn er påkrævet")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail er påkrævet")]
    [EmailAddress(ErrorMessage = "Ugyldig e-mailadresse")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Ugyldigt telefonnummer")]
    public string? Phone { get; set; }

    // Selve henvendelsen
    [Required(ErrorMessage = "Kategori er påkrævet")]
    [JsonConverter(typeof(StringEnumConverter))]
    public SupportCategory Category { get; set; }

    [Required(ErrorMessage = "Beskrivelse er påkrævet")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Beskrivelsen skal være mellem 10 og 2000 tegn")]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}