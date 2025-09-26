namespace Models.Interface
{
    public interface ILanguageResolver
    {
        // خروجی: کد زبان مثل "fa","de","en"
        string Resolve(HttpContext http);
    }

}