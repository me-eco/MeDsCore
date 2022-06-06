namespace MeDsCore.Utils;

/// <summary>
/// Вычисляет все права из данных битов
/// </summary>
public static class PermissionComputer
{
    private static Permission[] PermissionsArray = new[]
    {
        Permission.CreateInstantInvite, Permission.KickMembers, Permission.BanMembers, Permission.Administrator,
        Permission.ManageChannels, Permission.ManageGuild, Permission.AddReactions, Permission.ViewAuditLog,
        Permission.PrioritySpeaker, Permission.Stream, Permission.ViewChannel, Permission.SendMessages,
        Permission.SendTtsMessages, Permission.ManageMessages, Permission.EmbedLinks, Permission.AttachFiles,
        Permission.ReadMessageHistory,
        Permission.MentionEveryone, Permission.UseExternalEmojis, Permission.ViewGuildInsights, Permission.Connect,
        Permission.Speak, Permission.MuteMembers, Permission.DeafenMembers, Permission.MoveMembers, Permission.UseVad,
        Permission.ChangeNickname, Permission.ManageNicknames, Permission.ManageRoles, Permission.ManageWebhooks,
        Permission.ManageEmojisAndStickers, Permission.UseApplicationCommands, Permission.RequestToSpeak,
        Permission.ManageEvents,
        Permission.ManageThreads, Permission.CreatePublicThreads, Permission.CreatePrivateThreads,
        Permission.UseExternalStickers, Permission.SendMessagesInThreads, Permission.UseEmbeddedActivities,
        Permission.ModerateMembers
    };
    
    public static IEnumerable<Permission> ComputePermissions(long permissions)
    {
        for (var i = PermissionsArray.Length - 1; i >= 0; i--)
        {
            var permission = PermissionsArray[i];
            if ((permissions & (long)permission) == (long)permission) //Если при битовом И равняется исходному, то возвращаем роль
            {
                yield return permission;
            }
        }
    }
}