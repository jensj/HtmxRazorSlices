namespace HtmxRazorSlices.Data;

public class DbStorageOptions
{
    public const string SectionName = "DbStorage";
    
    public string StorageType { get; set; } = "InMemory";
    public string? LocalStoragePath { get; set; }
    public string? AzureBlobConnectionString { get; set; }
    public string? AzureBlobContainerName { get; set; }
}
