using Models.Entities;

public record ManagerDto(
    int Id,
    string UserName,
    string UserEmail,
    string Name
)
{
    public static ManagerDto FromEntity(Manager manager)
        => new(manager.Id, manager.UserName, manager.UserEmail, manager.Name);
}
