namespace Business.ServicesInterfaces.Models.PresenceViewModels
{
    public class PresenceEditModel
    {
        public string Name { get; set; }

        public PresenceEditModel(string name)
        {
            Name = name;
            //Week = week;
        }
    }
}