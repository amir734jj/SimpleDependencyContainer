namespace Container.Tests.Models
{
    public interface IDummyInterface
    {
        
    }

    public class DummyClass: IDummyInterface
    {
        
    }
    
    public class ModelWithInterface
    {
        private readonly IDummyInterface _dummyInterface;

        public ModelWithInterface(IDummyInterface dummyInterface)
        {
            _dummyInterface = dummyInterface;
        }
    }
}