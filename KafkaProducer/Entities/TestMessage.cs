namespace KafkaProducer.Entities
{
    public class TestMessage
    {
        public int RetryCount { get; set; }
        public bool Sucess { get; set; }
        public TestMessage()
        {
            RetryCount = 5;
            Sucess = false;
        }
    }
}