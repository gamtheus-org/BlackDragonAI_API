using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using Discord;
using Discord.WebSocket;

namespace BlackDragonAIAPI.Discord
{
    public interface IDiscordManager
    {
        Task Connect();
        Task<IEnumerable<StreamPlanning>> ReadStreamPlannings();
        Task WriteStreamPlanning(IEnumerable<StreamPlanning> streamPlannings);
        Task ShareUpdatedMessage();

        // event Action<IGuildUser> UserJoinedGuild;
        // Task ChangeRoleOfGuildUser(IGuildUser guildMember, ulong roleId);
    }
}