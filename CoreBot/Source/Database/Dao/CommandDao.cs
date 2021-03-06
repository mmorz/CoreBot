﻿using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandDao
    {
        /// <summary>
        /// Adds the specified command.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public async Task AddCommandAsync(Command command)
        {
            using (var connection = DbConnection.Open())
            {
                await connection.InsertAsync(command);
                Commands.Instance.CommandsList.Add(command);
            }
        }

        /// <summary>
        /// Deletes the specified command.
        /// </summary>
        /// <param name="command">Command to delete.</param>
        public async Task DeleteCommandAsync(Command command)
        {
            using (var connection = DbConnection.Open())
            {
                await connection.DeleteAsync(command);
                Commands.Instance.CommandsList.Remove(command);
            }
        }

        /// <summary>
        /// Updates the Action property of the command.
        /// </summary>
        /// <param name="command">Command to update.</param>
        /// <param name="newAction">The new action of the command.</param>
        public async Task UpdateCommandAsync(Command command, string newAction)
        {
            using (var connection = DbConnection.Open())
            {
                command.Action = newAction;
                await connection.UpdateAsync(command);
            }
        }
    }
}
