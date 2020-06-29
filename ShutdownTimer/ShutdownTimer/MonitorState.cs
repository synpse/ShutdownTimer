namespace ShutdownTimer
{
    // Enum for monitor states
    // From https://stackoverflow.com/questions/713498/turn-on-off-monitor
    // And https://www.codeproject.com/Articles/12794/Complete-Guide-on-How-To-Turn-A-Monitor-On-Off-Sta
    public enum MonitorState
    {
        MonitorStateOn = -1,
        MonitorStateOff = 2,
        MonitorStateStandBy = 1
    }
}
