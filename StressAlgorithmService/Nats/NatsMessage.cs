public class NatsMessage<T>
{
    public string origin { get; set; } = "Stress Algorithm Service";
    public string target { get; set; }
    public T message { get; set; }
}