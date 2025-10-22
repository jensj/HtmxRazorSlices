# Database Storage Options

This application supports multiple storage backends for ToDo data through the `IToDoDb` interface.

## Available Storage Types

### 1. InMemory (Default)
Non-persistent storage using an in-memory list. Data is lost when the application restarts.

**Configuration:**
```json
{
  "DbStorage": {
    "StorageType": "InMemory"
  }
}
```

### 2. FileSystem
Stores data as JSON files on the local file system, one file per user.

**Configuration:**
```json
{
  "DbStorage": {
    "StorageType": "FileSystem",
    "LocalStoragePath": "./data"
  }
}
```

**Features:**
- Data persists between application restarts
- Each user's todos are stored in a separate JSON file: `todos_{userId}.json`
- Thread-safe operations using semaphore locking
- Automatically creates the storage directory if it doesn't exist

### 3. AzureBlob
Stores data as JSON files in Azure Blob Storage, one blob per user.

**Configuration:**
```json
{
  "DbStorage": {
    "StorageType": "AzureBlob",
    "AzureBlobConnectionString": "your-connection-string-here",
    "AzureBlobContainerName": "todos"
  }
}
```

**Features:**
- Cloud-based storage with high availability
- Each user's todos are stored in a separate blob: `todos_{userId}.json`
- Thread-safe operations using semaphore locking
- Automatically creates the container if it doesn't exist

**Azure Setup:**
1. Create an Azure Storage Account
2. Get the connection string from Azure Portal
3. Configure the connection string in appsettings.json or user secrets
4. The container will be created automatically on first use

## Implementation Details

All implementations follow the `IToDoDb` interface:
- `GetAllToDosAsync(userId, cancellationToken)` - Returns all todos for a user, ordered by due date
- `GetToDoAsync(id, userId, cancellationToken)` - Returns a specific todo
- `CreateToDoAsync(todo, cancellationToken)` - Creates a new todo
- `UpdateToDoAsync(todo, cancellationToken)` - Updates an existing todo
- `DeleteToDoAsync(id, userId, cancellationToken)` - Deletes a todo

## Security Considerations

- User IDs are sanitized before being used in file/blob names
- For Azure Blob storage, use Azure Key Vault or User Secrets to store connection strings
- Never commit connection strings to source control
