﻿using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;
using Serilog;

namespace CoreBot.Modules
{
    public class WeatherModule : ModuleBase
    {
        private readonly WeatherService _weatherService;

        public WeatherModule(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Command("weather"), Summary("Displays weather data for the given location.")]
        [Alias("sää")]
        public async Task Weather([Remainder] string location)
        {
            Log.Information($"Getting weather data for the given location: {location}.");

            var weather = await _weatherService.GetWeatherDataAsync(location);
            await ReplyAsync(weather);
        }
    }
}