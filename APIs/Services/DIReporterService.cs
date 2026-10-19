namespace APIs.Services
{
    public class DIReporterService
    {
        public ITransientService TransientService { get; }
        public IScopedService ScopedService { get; }
        public ISingletonService SingletonService { get; }

        public DIReporterService(ITransientService transientService, IScopedService scopedService, ISingletonService singletonService)
        {
            TransientService = transientService;
            ScopedService = scopedService;
            SingletonService = singletonService;
        }
    }
}
