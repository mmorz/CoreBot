﻿using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Source.Helpers;
using ServiceStack.OrmLite;

namespace CoreBot.Managers
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Adds the specified command.
        /// </summary>
        public async Task AddCommand(Command command)
        {
            await Database.Run().InsertAsync(command);
            Commands.Instance.CommandsList.Add(command);
        }

        /// <summary>
        /// Deletes the specified command.
        /// </summary>
        public async Task DeleteCommand(Command command)
        {
            await Database.Run().DeleteAsync(command);
            Commands.Instance.CommandsList.Remove(command);
        }

        /// <summary>
        /// Updates the Action property of the command.
        /// </summary>
        public async Task UpdateCommand(Command command, string newAction)
        {
            command.Action = newAction;
            await Database.Run().UpdateAsync(command);
        }
    }
}