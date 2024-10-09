namespace NArtadoSearch.Entities.Models;

public class AutoCompleteResults
{
    public string AppliedQuery { get; set; } = string.Empty;
    public List<string> Suggestions { get; set; } = new List<string>();
}