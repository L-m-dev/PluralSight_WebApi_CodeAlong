namespace CityInfo.API.Models
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore() {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited park ever."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "a 102-story skyscraper"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished"
                },
                new CityDto() {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower"
                }

            };


        }
    }
}

