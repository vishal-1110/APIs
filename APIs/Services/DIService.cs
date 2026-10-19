namespace APIs.Services
{
    public interface  ITransientService
    {
        Guid Id { get; }
    }
    public interface IScopedService
    {
        Guid Id { get; }
    }
    public interface ISingletonService
    {
        Guid Id { get; }
    }
    public class DIService: ITransientService, IScopedService, ISingletonService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
    
}
