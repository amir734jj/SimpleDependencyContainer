namespace Container.Tests.Models
{

    public interface ICircularDependency
    {
        
    }

    public class CircularDependency : ICircularDependency
    {
        public CircularDependency(ICircularDependency circularDependency)
        {
            
        }
    }
}