namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    enum MemoryMode // режим работы памяти
    {
        GET, // считывание памяти
        SET, // сохранение памяти
        INIT // инициализация памяти

    }

    enum NeironType // тип нейрона
    {
        Hidden, // скрытый
        Output // выходной
    }

    enum NetworkMode // режим работы сети
    {
        Train, // обучение
        Test, // проверка
        Demo // распознавание
    }
}
