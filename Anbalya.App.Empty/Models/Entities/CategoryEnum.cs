using System.ComponentModel.DataAnnotations;



namespace Models.Entities
{
    public enum Category
    {



        Relaxing,

        //  [Display(Name = "Adventure", ResourceType = typeof(/Users/sinahajianmanesh/TourAntalya/Anbalya.App.Empty/Resources))]
        Adventure,

        //  [Display(Name = "History", ResourceType = typeof(Resources.Category))]
        History,

        // [Display(Name = "Nature", ResourceType = typeof(Resources.Category))]
        Nature,

        //  [Display(Name = "Family", ResourceType = typeof(Resources.Category))]
        Family,

        //  [Display(Name = "Special", ResourceType = typeof(Resources.Category))]
        Special,

    }
}