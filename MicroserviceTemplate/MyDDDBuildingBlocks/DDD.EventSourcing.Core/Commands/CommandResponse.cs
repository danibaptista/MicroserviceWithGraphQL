namespace DDD.EventSourcing.Core.Commands
{
    public class CommandResponse
    {
        public static CommandResponse Fail = new CommandResponse { Success = false };
        public static CommandResponse Ok = new CommandResponse { Success = true };
        public bool Success { get; private set; }

        public CommandResponse(bool success = false)
        {
            Success = success;
        }
    }
}