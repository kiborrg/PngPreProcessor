namespace PngPreProcessor.Models
{
    public enum StatusEnum
    {
        Created,            //Создан
        Upload,             //Загружается
        InQueueToProcess,  //В очереди на 
        InProcess,         //Обрабатывается
        Finished,           //Готово
        Canceled,           //Отменено
        Error               //Ошибка
    }
}
