namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public interface ITaskStateToggle
    {
        void TryClose(IRunningTastState state);
        void Close(IRunningTastState state);
        void Open(IRunningTastState state);
    }
}