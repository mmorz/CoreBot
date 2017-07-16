﻿using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace CoreBot.Modules
{
    public class RandomModule : ModuleBase
    {
        [Command("random"), Summary("Generates a random number between two given values.")]
        public async Task Random(int firstNumber, int secondNumber)
        {
            var random = new Random();
            await ReplyAsync($"{random.Next(firstNumber, secondNumber)}");
        }
    }
}