using System.ComponentModel.DataAnnotations;

namespace AspireExistingDB.ApiService;

/*
The SQL database will link the columns to these values, and thus the values can all be manipulated within C# as C# values.
*/

public sealed class HelloInformation {
    public string? name { get; set; }
    public int age { get; set; }
    public string? email { get; set; }
    public string? github { get; set; }
    public string? linkedin { get; set; }
    public string? birth_date { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Now;
}