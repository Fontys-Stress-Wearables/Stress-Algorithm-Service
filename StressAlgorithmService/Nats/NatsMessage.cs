public class NatsMessage<T>
{
    public string origin { get; set; } = "stress_algorithm_service";
    public string target { get; set; }
    public T message { get; set; }
}