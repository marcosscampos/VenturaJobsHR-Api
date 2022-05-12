namespace VenturaJobsHR.Message.Dto.Common;

public class MessageQueueDto<T>
{
    public MessageQueueDto(List<T> listEntity)
    {
        ListEntity = listEntity;
    }

    public MessageQueueDto()
    {

    }

    public List<T> ListEntity { get; set; }
}
