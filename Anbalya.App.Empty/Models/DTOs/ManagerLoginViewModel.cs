public class ManagerLoginViewModel
{
    public ManagerLoginViewModel()
    {
        Input = new ManagerLoginDto();
    }

    public ManagerLoginDto Input { get; set; }

    public string? ErrorMessage { get; set; }
}
