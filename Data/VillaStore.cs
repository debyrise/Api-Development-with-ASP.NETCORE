﻿using WebApiDemo.Model.Dto;

namespace WebApiDemo.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
            {
                new VillaDto {Id = 1, Name = "pool view", sqft = 100, Occupancy = 4},
                new VillaDto {Id = 2, Name = "Beach view", sqft = 300, Occupancy = 3}
            };
    };

};
