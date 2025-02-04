namespace Infraestructure
{
    public class SubtractionException : Exception
    {
        public SubtractionException(string operation, double resultOp) : base(Messages.CheckOperation(operation, $"{resultOp:F2}"))
        {
            
        }
    }
}
